using QRCoder;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AIPhotoGenerator : MonoBehaviour
{
    // The API endpoint
    public UIDataSO uiData;
    private string apiUrl = "https://etechmy.com/Milan_Photobooth/web/user/UserApi.php/CreateNanoAI";
    private string qrUrl = "https://etechmy.com/Milan_Photobooth/web/user/UserApi.php/CreateURL";


    public RawImage qrCodeDisplay; // Assign in Inspector
    public RawImage aiImageDisplay;
    public RawImage aiGeneratedImage;
    public TMP_Text nameText;
    public RenderTexture renderTexture;

    [System.Serializable]
    public class AIImageResponse
    {
        public bool status;
        public string message;
        public string final_url;
    }

    [System.Serializable]
    public class QRResponse
    {
        public bool status;
        public string message;
        public string url;
    }

    private void OnEnable()
    {
        uiData.GenerateAIImageEvent += UploadTexture;
        uiData.GenerateQRCodeEvent += GenerateDefaultQR;
    }

    private void OnDisable()
    {
        uiData.GenerateAIImageEvent -= UploadTexture;
        uiData.GenerateQRCodeEvent -= GenerateDefaultQR;
    }

    /// <summary>
    /// Public method to start the upload process.
    /// </summary>
    /// <param name="texture">The Texture2D to upload.</param>
    /// <param name="imageId">The ID to send along with the image.</param>
    public void UploadTexture(Texture2D texture, int imageId)
    {
        Debug.Log(" uploading" + imageId);
        StartCoroutine(UploadImageCoroutine(texture, imageId));
    }

    private IEnumerator UploadImageCoroutine(Texture2D texture, int imageId)
    {
        if (texture == null)
        {
            Debug.LogError("Cannot upload. The provided Texture2D is null.");
            yield break;
        }

        byte[] imageData;
        try
        {
            imageData = texture.EncodeToPNG();
        }
        catch (UnityException ex)
        {
            Debug.LogError($"Error encoding texture. Is it 'Read/Write Enabled' in its import settings? \n{ex.Message}");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("imageid", imageId.ToString().Trim());
        form.AddBinaryData("image", imageData, "upload.png", "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            Debug.Log("Uploading image...");
            Debug.Log($"Uploading image with imageid: '{imageId}' (type: {imageId.GetType()})");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"API Error: {www.error}");
                Debug.Log($"Server Response: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Upload complete!");
                Debug.Log($"Server Response: {www.downloadHandler.text}");

                // Parse the JSON response
                AIImageResponse response = JsonUtility.FromJson<AIImageResponse>(www.downloadHandler.text);
                if (response != null && response.status && !string.IsNullOrEmpty(response.final_url))
                {
                    StartCoroutine(DownloadAIImage(response.final_url));
                }
                else
                {
                    Debug.LogError("Failed to parse AI image response or invalid response.");
                }
            }
        }
    }

    private IEnumerator DownloadAIImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            Debug.Log($"Downloading AI image from: {url}");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Image download error: {www.error}");
            }
            else
            {
                Texture2D aiImage = DownloadHandlerTexture.GetContent(www);
                Debug.Log("AI image downloaded successfully.");
                uiData.aiGeneratedImage = aiImage;
                
                aiGeneratedImage.texture = aiImage;
                if (uiData.playerName != "")
                {
                    nameText.text = uiData.playerName;
                }
                else
                {
                    nameText.text = "Guest User";
                }

                StartCoroutine(WaitAndGetFinalImage());
            }
        }
    }

    private IEnumerator WaitAndGetFinalImage()
    {
        yield return new WaitForSeconds(2f);
        RenderTexture.active = renderTexture;
        Texture2D finalTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        finalTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        finalTexture.Apply();
        RenderTexture.active = null;
        uiData.qrImage = finalTexture;
        aiImageDisplay.texture = finalTexture;
        UploadForQR(finalTexture, ((int)uiData.selectedCity+1));
    }


    private void GenerateDefaultQR()
    {
        if (uiData.playerName != "")
        {
            nameText.text = uiData.playerName;
        }
        else
        {
            nameText.text = "Guest User";
        }

        StartCoroutine(WaitAndGetFinalImage());
    }

    /// <summary>
    /// Public method to start the QR upload process.
    /// </summary>
    /// <param name="texture">The Texture2D to upload for QR code generation.</param>
    /// <param name="imageId">The ID to send along with the image.</param>
    public void UploadForQR(Texture2D texture, int imageId)
    {
        StartCoroutine(UploadImageForQRCoroutine(texture, imageId));
    }

    private IEnumerator UploadImageForQRCoroutine(Texture2D texture, int imageId)
    {
        if (texture == null)
        {
            Debug.LogError("Cannot upload for QR. The provided Texture2D is null.");
            yield break;
        }

        byte[] imageData;
        try
        {
            imageData = texture.EncodeToPNG();
        }
        catch (UnityException ex)
        {
            Debug.LogError($"Error encoding texture for QR. Is it 'Read/Write Enabled' in its import settings? \n{ex.Message}");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("imageid", imageId.ToString().Trim());
        form.AddBinaryData("image", imageData, "upload.png", "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(qrUrl, form))
        {
            Debug.Log("Uploading image for QR...");
            Debug.Log($"Uploading image with imageid: '{imageId}' (type: {imageId.GetType()})");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"QR API Error: {www.error}");
                Debug.Log($"QR Server Response: {www.downloadHandler.text}");
            }
            else
            {
                Debug.Log("QR Upload complete!");
                Debug.Log($"QR Server Response: {www.downloadHandler.text}");

                // Parse the JSON response
                QRResponse response = JsonUtility.FromJson<QRResponse>(www.downloadHandler.text);
                if (response != null && response.status && !string.IsNullOrEmpty(response.url))
                {
                   Debug.Log($"QR Code URL: {response.url}");
                    GenerateAndShowQRCode(response.url);    
                }
                else
                {
                    Debug.LogError("Failed to parse QR response or invalid response.");
                }
            }
        }
    }


    private void GenerateAndShowQRCode(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogWarning("GenerateAndShowQRCode: URL is empty.");
            return;
        }

        try
        {
            // Use QRCoder's PngByteQRCode to generate PNG bytes (no System.Drawing dependency)
            using (var qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                var pngWriter = new PngByteQRCode(qrCodeData);
                byte[] pngBytes = pngWriter.GetGraphic(20); // 20 pixels per module

                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (tex.LoadImage(pngBytes))
                {
                    tex.Apply();
                    if (qrCodeDisplay != null)
                    {
                        qrCodeDisplay.texture = tex;
                        //qrCodeDisplay.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tex.width);
                        //qrCodeDisplay.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tex.height);
                    }
                }
                else
                {
                    Debug.LogError("GenerateAndShowQRCode: Failed to LoadImage from PNG bytes.");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"GenerateAndShowQRCode: Exception generating QR: {ex}");
        }
    }
}