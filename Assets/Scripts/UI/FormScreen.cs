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


    private void OnEnable()
    {
        uiData.aiGeneratedImage = null;
        uiData.playerImage = null;
        submittedPreview.texture = null;
    }

    private void OnDisable()
    {
        submittedPreview.texture = null;
        camBtn.SetActive(true);
        camPanel.SetActive(false);
        capturePanel.SetActive(false);

    }
    public void OnPreviousButtonClick()
    {
        submittedPreview.texture = null;
        camBtn.SetActive(true);
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

            // Always select the back camera if available
            string backCamName = null;
            foreach (var device in WebCamTexture.devices)
            {
#if UNITY_ANDROID
                if (!device.isFrontFacing)
                {
                    backCamName = device.name;
                    break;
                }
#else
            // On non-Android, just use the first camera
            backCamName = device.name;
            break;
#endif
            }
            if (backCamName == null && WebCamTexture.devices.Length > 0)
                backCamName = WebCamTexture.devices[0].name;

            // Get the width and height from the cameraPreview RectTransform
            int camWidth = Mathf.RoundToInt(cameraPreview.rectTransform.rect.width);
            int camHeight = Mathf.RoundToInt(cameraPreview.rectTransform.rect.height);

            webCamTexture = new WebCamTexture(backCamName, camWidth, camHeight);
            webCamTexture.Play();
            cameraPreview.texture = webCamTexture;

            // No flipping for back camera
            cameraPreview.uvRect = new Rect(0, 0, 1, 1);

            // Always rotate preview -90 degrees
            cameraPreview.rectTransform.localEulerAngles = new Vector3(0, 0, -90);

            Debug.Log("Webcam started: " + backCamName);
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


    //void Update()
    //{
    //    if (webCamTexture != null && webCamTexture.isPlaying)
    //    {
    //        camBorder.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);
    //    }
    //}

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
            int width = webCamTexture.width;
            int height = webCamTexture.height;
            Color[] pixels = webCamTexture.GetPixels();

            // No mirroring or rotation, just swap width and height to match preview
            Texture2D captured = new Texture2D(height, width, TextureFormat.RGB24, false);

            // Copy pixels with 90-degree rotation (to match preview)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    captured.SetPixel(y, width - x - 1, pixels[y * width + x]);
                }
            }
            captured.Apply();

            uiData.playerImage = captured;

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
