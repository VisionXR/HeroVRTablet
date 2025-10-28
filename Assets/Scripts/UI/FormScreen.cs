using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FormScreen : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_Dropdown genderDropdown;

    [Header("Cam Elements")]
    public GameObject camPanel;
    public GameObject capturePanel;
    [SerializeField] private RawImage cameraPreview;
    [SerializeField] private RawImage capturePreview;
    [SerializeField] private RawImage submittedPreview;
    public GameObject camBtn;
    [SerializeField] private TMP_Text countdownText;
    private WebCamTexture webCamTexture;

    public GameObject nextPanel;

    // Store the captured image for later use
    public Texture2D capturedImage { get; private set; }

    public void OnNextButtonClick()
    {
        string name = nameInputField.text;
        string email = emailInputField.text;
        string gender = genderDropdown.options[genderDropdown.value].text;
        uiData.playerName = name;
        uiData.playerEmail = email;
        uiData.selectedGender = (Gender)genderDropdown.value;

        Debug.Log($"Name: {name}, Email: {email}, Gender: {gender}");

        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CamButtonClicked()
    {
        if (webCamTexture == null)
        {
            camPanel.SetActive(true);
            webCamTexture = new WebCamTexture();
            webCamTexture.Play();
            cameraPreview.texture = webCamTexture;
            cameraPreview.uvRect = new Rect(1, 0, -1, 1); // <-- Add this line
            Debug.Log("Webcam started.");
        }
        else if (!webCamTexture.isPlaying)
        {
            camPanel.SetActive(true);
            webCamTexture.Play();
            cameraPreview.texture = webCamTexture;
            cameraPreview.uvRect = new Rect(1, 0, -1, 1); // <-- Add this line
        }
        else
        {
            webCamTexture.Stop();
            camPanel.SetActive(false);
        }
    }

    public void CaptureBtnClicked()
    {
        StartCoroutine(CaptureCountdownAndImage());
    }

    private IEnumerator CaptureCountdownAndImage()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            countdownText.gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            // Capture and mirror the image horizontally
            capturedImage = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
            Color[] pixels = webCamTexture.GetPixels();
            int width = webCamTexture.width;
            int height = webCamTexture.height;
            Color[] mirroredPixels = new Color[pixels.Length];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mirroredPixels[y * width + x] = pixels[y * width + (width - 1 - x)];
                }
            }

            capturedImage.SetPixels(mirroredPixels);
            capturedImage.Apply();

            CloseCamera();
            capturePanel.SetActive(true);
            capturePreview.texture = capturedImage;
        }
    }

    public void SubmitButtonClicked()
    {
        capturePanel.SetActive(false);
        submittedPreview.texture = capturedImage;
       // camBtn.SetActive(false);
    }

    public void RetakeBtnClicked()
    {
        CamButtonClicked();
        capturePanel.SetActive(false);
    }

    public void CloseCamera()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
            camPanel.SetActive(false);
        }
    }
}
