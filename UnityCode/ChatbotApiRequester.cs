using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

[System.Serializable]
public class ApiResponse
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public int index;
    public object logprobs;
    public string finish_reason;
    public Message message;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

public class ChatbotApiRequester : MonoBehaviour
{
    private string url = "";
    private SoundPlayer soundPlayer;
    public InputField userInputField;
    public Button sendButton;

    private UnityMessageReceiver messageReceiver;


    void Start()
    {
           int sampleRate = AudioSettings.outputSampleRate;
        
        // Log the sample rate to the console
        Debug.Log("Current Output Sample Rate: " + sampleRate);
        messageReceiver = FindObjectOfType<UnityMessageReceiver>();
        if (messageReceiver != null)
        {
            string currentLocalhost = messageReceiver.GetLocalhost();
            Debug.Log("Current Localhost IP: " + currentLocalhost);
            url = "http://"+currentLocalhost+":1234/v1/chat/completions";
        }
        else
        {
            Debug.LogError("UnityMessageReceiver not found!");
        }
        soundPlayer = GetComponent<SoundPlayer>();
        sendButton.onClick.AddListener(SendMessage);
    }

    void Update()
    {
        if (userInputField.isFocused && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            string userMessage = userInputField.text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                StartCoroutine(PostRequest(userMessage));
                userInputField.text = "";
                userInputField.DeactivateInputField();
            }
        }
    }
    private void SendMessage()
    {
        string userMessage = userInputField.text.Trim();
        if (!string.IsNullOrEmpty(userMessage))
        {
            StartCoroutine(PostRequest(userMessage));
            userInputField.text = "";
            userInputField.DeactivateInputField();
        }
    }
    
    private IEnumerator PostRequest(string userMessage)
    {
        string escapedMessage = EscapeJsonString(userMessage);
        string json = "{"
            + "\"model\": \"cantonesellmchat-v1.0-32b-i1\","
            + "\"messages\": ["
            + "{ \"role\": \"system\", \"content\": \"You are assistant. You only answer in cantonese.\" },"
            + "{ \"role\": \"user\", \"content\": \"" + escapedMessage + "\" }"
            + "],"
            + "\"temperature\": 0.7,"
            + "\"max_tokens\": -1,"
            + "\"stream\": false"
            + "}";

        Debug.Log("JSON to send: " + json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);

            if (response.choices.Length > 0)
            {
                string content = response.choices[0].message.content;
                content = content.Replace("\n", "");
                Debug.Log("Response Content: " + content);
                soundPlayer.SetTextToSpeak(content);
            }
            else
            {
                Debug.LogWarning("No choices found in the response.");
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        request.Dispose();
    }

    private string EscapeJsonString(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
