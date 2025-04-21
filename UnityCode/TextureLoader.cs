// // using UnityEngine;
// // using UnityEngine.Networking;
// // using System.Collections;

// // public class TextureLoader : MonoBehaviour
// // {
// //     public GameObject targetObject; // Assign the GameObject with the material in the Inspector
// //     private string imageUrl = "https://ebvecgyezvakcxlegspv.supabase.co/storage/v1/object/public/image//Zi_Zhi_Tong_Jian.jpg"; // Set your URL here

// //     IEnumerator Start()
// //     {
// //         // Start the coroutine to load the new texture
// //         yield return StartCoroutine(LoadTextureFromURL(imageUrl));
// //     }

// //    private IEnumerator LoadTextureFromURL(string url)
// // {
// //     using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
// //     {
// //         // Send the request and wait for a response
// //         yield return www.SendWebRequest();

// //         // Check for errors
// //         if (www.result != UnityWebRequest.Result.Success)
// //         {
// //             Debug.LogError($"Error loading texture: {www.error}");
// //             yield break; // Exit if there's an error
// //         }

// //         // Get the new texture
// //         Texture2D newTexture = DownloadHandlerTexture.GetContent(www);
// //         if (newTexture == null)
// //         {
// //             Debug.LogError("Failed to load the texture.");
// //             yield break; // Exit if texture is null
// //         }

// //         // Get the Renderer component from the target object
// //         Renderer renderer = targetObject.GetComponent<Renderer>();
// //         if (renderer == null)
// //         {
// //             Debug.LogError("Renderer component not found on target object.");
// //             yield break; // Exit if no renderer
// //         }

// //         // Get the materials of the object
// //         Material[] materials = renderer.materials;

// //         // Loop through the materials to find 'Book01'
// //         foreach (Material material in materials)
// //         {
// //             Debug.Log($"Checking material: {material.name}"); // Debugging statement
// //             if (material.name == "Book05 (Instance)")
// //             {
// //                 material.SetTexture("_MainTex", newTexture); // Replace the Albedo texture
// //                 Debug.Log("Texture replaced successfully.");
// //                 break; // Exit loop after replacing
// //             }
// //         }
// //     }
// // }
// // }

// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;

// public class TextureCombiner : MonoBehaviour
// {
//     public GameObject targetObject; // Assign the GameObject with the material in the Inspector
//     public string imageUrl;
//     public Texture2D baseTexture; // Assign the base texture in the Inspector (should be square)

//     void Start()
//     {
//         // Example URL, replace with your actual URL
//         imageUrl = "https://ebvecgyezvakcxlegspv.supabase.co/storage/v1/object/public/image//Zi_Zhi_Tong_Jian.jpg";

//         // Ensure the base texture is square and set to 1024x1024 if needed
//         if (baseTexture != null)
//         {
//             Texture2D resizedBaseTexture = ResizeTexture(baseTexture, 1024, 1024);
//             StartCoroutine(LoadTextureFromURL(imageUrl, resizedBaseTexture));
//         }
//         else
//         {
//             Debug.LogError("Base texture is not assigned.");
//         }
//     }

//     private IEnumerator LoadTextureFromURL(string url, Texture2D resizedBaseTexture)
//     {
//         using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
//         {
//             yield return www.SendWebRequest();

//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError($"Error loading texture: {www.error}");
//                 yield break;
//             }

//             // Get the new texture
//             Texture2D newTexture = DownloadHandlerTexture.GetContent(www);
//             if (newTexture == null)
//             {
//                 Debug.LogError("Failed to load the texture from the URL.");
//                 yield break;
//             }

//             // Check if the texture is readable
//             if (!newTexture.isReadable)
//             {
//                 Debug.LogError("The loaded texture is not readable.");
//                 yield break;
//             }

//             // Combine the textures
//             Texture2D combinedTexture = CombineTextures(resizedBaseTexture, newTexture);
//             if (combinedTexture != null)
//             {
//                 // Set the combined texture as the Albedo of the material
//                 SetMaterialTexture(combinedTexture);
//             }
//         }
//     }

//     private Texture2D ResizeTexture(Texture2D original, int targetWidth, int targetHeight)
//     {
//         // Create a new texture with the target dimensions
//         Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight);

//         // Resize the original texture to fit the new dimensions
//         for (int y = 0; y < targetHeight; y++)
//         {
//             for (int x = 0; x < targetWidth; x++)
//             {
//                 // Calculate original pixel coordinates
//                 int originalX = Mathf.FloorToInt((float)x / targetWidth * original.width);
//                 int originalY = Mathf.FloorToInt((float)y / targetHeight * original.height);

//                 // Set the pixel from the original texture
//                 resizedTexture.SetPixel(x, y, original.GetPixel(originalX, originalY));
//             }
//         }

//         resizedTexture.Apply();
//         return resizedTexture;
//     }

//     private Texture2D CombineTextures(Texture2D baseTex, Texture2D overlayTex)
//     {
//         // Create a new texture for the combined result
//         Texture2D combinedTexture = new Texture2D(baseTex.width, baseTex.height);

//         // Copy the base texture pixels to the combined texture
//         combinedTexture.SetPixels(baseTex.GetPixels());

//         // Calculate the size and position for the overlay texture
//         int overlayWidth = overlayTex.width;
//         int overlayHeight = overlayTex.height;

//         // Define the target size and position
//         int targetWidth = 430;  // Width (960 - 530)
//         int targetHeight = 600; // Height (820 - 220)

//         // Calculate scale factors
//         float widthScale = (float)targetWidth / overlayWidth;
//         float heightScale = (float)targetHeight / overlayHeight;
//         float scale = Mathf.Min(widthScale, heightScale); // Maintain aspect ratio

//         // New dimensions for the overlay texture
//         int newWidth = Mathf.FloorToInt(overlayWidth * scale);
//         int newHeight = Mathf.FloorToInt(overlayHeight * scale);

//         // Create a new texture for the resized overlay
//         Texture2D resizedOverlay = new Texture2D(newWidth, newHeight);
//         Color[] resizedPixels = new Color[newWidth * newHeight];

//         // Resize the overlay texture
//         for (int y = 0; y < newHeight; y++)
//         {
//             for (int x = 0; x < newWidth; x++)
//             {
//                 // Calculate the corresponding pixel in the original overlay texture
//                 int originalX = Mathf.FloorToInt(x / scale);
//                 int originalY = Mathf.FloorToInt(y / scale);

//                 if (originalX < overlayWidth && originalY < overlayHeight)
//                 {
//                     resizedPixels[x + y * newWidth] = overlayTex.GetPixel(originalX, originalY);
//                 }
//             }
//         }

//         resizedOverlay.SetPixels(resizedPixels);
//         resizedOverlay.Apply();

//         // Calculate the position to overlay the resized texture
//         int startX = 530; // X position in pixels
//         int startY = 270; // Y position in pixels

//         // Set the pixels of the resized overlay texture
//         for (int y = 0; y < newHeight; y++)
//         {
//             for (int x = 0; x < newWidth; x++)
//             {
//                 int combinedX = startX + x;
//                 int combinedY = startY + y;

//                 // Make sure we're within bounds
//                 if (combinedX < combinedTexture.width && combinedY < combinedTexture.height)
//                 {
//                     combinedTexture.SetPixel(combinedX, combinedY, resizedOverlay.GetPixel(x, y));
//                 }
//             }
//         }

//         // Apply changes to the combined texture
//         combinedTexture.Apply();
        
//         return combinedTexture;
//     }

//     private void SetMaterialTexture(Texture2D texture)
//     {
//         Renderer renderer = targetObject.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             foreach (Material material in renderer.materials)
//             {
//                 if (material.name == "Book05 (Instance)")
//                 {
//                     material.SetTexture("_MainTex", texture); // Set the combined texture
//                     break; // Exit loop after replacing
//                 }
//             }
//         }
//     }
// }

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TextureCombiner : MonoBehaviour
{
    public GameObject targetObject; // Assign the GameObject with the material in the Inspector
    public string imageUrl; // Example: "http://example.com/your-image.png"
    public Texture2D baseTexture; // Assign the base texture in the Inspector (should be square)

    void Start()
    {
        imageUrl = "https://ebvecgyezvakcxlegspv.supabase.co/storage/v1/object/public/image//MeinKampf.jpg";
        // Ensure the base texture is square and set to 1024x1024 if needed
        if (baseTexture != null)
        {
            Texture2D resizedBaseTexture = ResizeTexture(baseTexture, 1024, 1024);
            //StartCoroutine(LoadTextureFromURL(imageUrl, resizedBaseTexture));
        }
        else
        {
            Debug.LogError("Base texture is not assigned.");
        }
    }

    public void RNsetImage(string url){
             imageUrl = url;
        // Ensure the base texture is square and set to 1024x1024 if needed
        if (baseTexture != null)
        {
            Texture2D resizedBaseTexture = ResizeTexture(baseTexture, 1024, 1024);
            StartCoroutine(LoadTextureFromURL(imageUrl, resizedBaseTexture));
        }
        else
        {
            Debug.LogError("Base texture is not assigned.");
        }
    }

    private IEnumerator LoadTextureFromURL(string url, Texture2D resizedBaseTexture)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error loading texture: {www.error}");
                yield break;
            }

            // Get the new texture
            Texture2D newTexture = DownloadHandlerTexture.GetContent(www);
            if (newTexture == null)
            {
                Debug.LogError("Failed to load the texture from the URL.");
                yield break;
            }

            // Check if the texture is readable
            if (!newTexture.isReadable)
            {
                Debug.LogError("The loaded texture is not readable.");
                yield break;
            }

            // Combine the textures
            Texture2D combinedTexture = CombineTextures(resizedBaseTexture, newTexture);
            if (combinedTexture != null)
            {
                // Set the combined texture as the Albedo of the material
                SetMaterialTexture(combinedTexture);
            }
        }
    }

    private Texture2D StretchTexture(Texture2D original, int targetWidth, int targetHeight)
    {
        // Create a new texture with the target dimensions
        Texture2D stretchedTexture = new Texture2D(targetWidth, targetHeight);

        // Stretch the original texture to fit the new dimensions
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // Calculate original pixel coordinates to stretch
                int originalX = Mathf.FloorToInt((float)x / targetWidth * original.width);
                int originalY = Mathf.FloorToInt((float)y / targetHeight * original.height);

                // Set the pixel from the original texture
                stretchedTexture.SetPixel(x, y, original.GetPixel(originalX, originalY));
            }
        }

        stretchedTexture.Apply();
        return stretchedTexture;
    }

    private Texture2D ResizeTexture(Texture2D original, int newWidth, int newHeight)
    {
        // Create a new texture with the target dimensions
        Texture2D resizedTexture = new Texture2D(newWidth, newHeight);

        // Resize the original texture to fit the new dimensions
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                // Calculate original pixel coordinates
                int originalX = Mathf.FloorToInt((float)x / newWidth * original.width);
                int originalY = Mathf.FloorToInt((float)y / newHeight * original.height);

                // Set the pixel from the original texture
                resizedTexture.SetPixel(x, y, original.GetPixel(originalX, originalY));
            }
        }

        resizedTexture.Apply();
        return resizedTexture;
    }

   private Texture2D CombineTextures(Texture2D baseTex, Texture2D overlayTex)
{
    // Create a new texture for the combined result
    Texture2D combinedTexture = new Texture2D(baseTex.width, baseTex.height);
    combinedTexture.SetPixels(baseTex.GetPixels());

    // Calculate the size and position for the overlay texture
    int overlayWidth = overlayTex.width;
    int overlayHeight = overlayTex.height;

    // Define the target size for the overlay
    int targetWidth = 410;  // Desired width (e.g., 200 pixels)
    int targetHeight = 550; // Desired height (e.g., 400 pixels)

    // Resize the overlay texture to the target dimensions
    Texture2D resizedOverlay = StretchTexture(overlayTex, targetWidth, targetHeight);

    // Set your custom starting position
    int startX = 530; // X position in pixels
    int startY = 240; // Y position in pixels

    // Set the pixels of the resized overlay texture
    for (int y = 0; y < resizedOverlay.height; y++)
    {
        for (int x = 0; x < resizedOverlay.width; x++)
        {
            int combinedX = startX + x;
            int combinedY = startY + y;

            // Make sure we're within bounds
            if (combinedX < combinedTexture.width && combinedY < combinedTexture.height)
            {
                combinedTexture.SetPixel(combinedX, combinedY, resizedOverlay.GetPixel(x, y));
            }
        }
    }

    // Apply changes to the combined texture
    combinedTexture.Apply();

    return combinedTexture;
}

    private void SetMaterialTexture(Texture2D texture)
    {
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            foreach (Material material in renderer.materials)
            {
                Debug.Log(material.name);
                if (material.name == "Book05 (Instance)")
                {
                    material.SetTexture("_MainTex", texture); // Set the combined texture
                    break; // Exit loop after replacing
                }
            }
        }
    }
}