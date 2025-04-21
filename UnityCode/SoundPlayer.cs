using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SoundPlayer : MonoBehaviour
{
    public string baseUrl;
    public string textToSpeak; // Your Chinese text here
    private AudioSource audioSource;

    void Start()
    {
        baseUrl = "http://127.0.0.1:5000/sound/";
        audioSource = GetComponent<AudioSource>();
    }

    public void SetTextToSpeak(string text)
    {
        textToSpeak = text;
        StartCoroutine(DownloadSound());
    }

    IEnumerator DownloadSound()
    {
        // Encode the text for the URL
        string encodedText = Uri.EscapeDataString(textToSpeak);
        string soundUrl = baseUrl + encodedText;
        Debug.Log("Sound URL: " + soundUrl);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(soundUrl, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("This is error: >O<"+www.error);
            }
            else
            {
                                Debug.Log("PLAY SOUND!!!1");

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log("PLAY SOUND!!!2");
            }
        }
    }
}