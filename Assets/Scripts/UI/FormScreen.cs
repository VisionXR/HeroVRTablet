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

    [Header("Game Objects")]
    public GameObject nextPanel;
    public GameObject previousPanel;
    public GameObject camPanel;
    public GameObject capturePanel;
    public GameObject camBtn;

    [Header("Cam Elements")]
    [SerializeField] private RawImage camBorder;
    [SerializeField] private RawImage cameraPreview;
    [SerializeField] private RawImage capturePreview;
    [SerializeField] private RawImage submittedPreview; 
    [SerializeField] private TMP_Text countdownText;
    private WebCamTexture webCamTexture;

   

    public void OnPreviousButtonClick()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnNextButtonClick()
    {
        string name = nameInputField.text;
        string email = emailInputField.text;
        string gender = genderDropdown.options[genderDropdown.value].text;
        uiData.playerName = name;
        uiData.playerEmail = email;
        uiData.selectedGender = (Gender)genderDropdown.value;


        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CamButtonClicked()
    {
        if (webCamTexture == null)
        {
            camPanel.SetActive(true);

            // Select back camera if available
            string backCamName = null;
            foreach (var device in WebCamTexture.devices)
            {
                if (!device.isFrontFacing)
                {
                    backCamName = device.name;
                    break;
                }
            }
            if (backCamName == null && WebCamTexture.devices.Length > 0)
                backCamName = WebCamTexture.devices[0].name;

            webCamTexture = new WebCamTexture(backCamName);
            webCamTexture.Play();
            cameraPreview.texture = webCamTexture;

            // Flip only if front camera or desktop
            bool shouldFlip = false;
#if UNITY_ANDROID
            shouldFlip = (backCamName == null || WebCamTexture.devices.Length == 0 || WebCamTexture.devices[0].isFrontFacing);
#else
        shouldFlip = true; // desktop/webcam
#endif
            cameraPreview.uvRect = shouldFlip ? new Rect(1, 0, -1, 1) : new Rect(0, 0, 1, 1);

            Debug.Log("Webcam started.");
        }
        else if (!webCamTexture.isPlaying)
        {
            camPanel.SetActive(true);
            webCamTexture.Play();
            cameraPreview.texture = webCamTexture;
        }
        else
        {
            webCamTexture.Stop();
            camPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            camBorder.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);
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
            uiData.playerImage = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
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

            uiData.playerImage.SetPixels(mirroredPixels);
            uiData.playerImage.Apply();

            CloseCamera();
            capturePanel.SetActive(true);
            capturePreview.texture = uiData.playerImage;
        }
    }

    public void SubmitButtonClicked()
    {
        capturePanel.SetActive(false);
        submittedPreview.texture = uiData.playerImage;   
         camBtn.SetActive(false);
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
