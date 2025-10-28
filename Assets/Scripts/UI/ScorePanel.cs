using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public Image qrCodeImage;
    public Image aiGeneratedImage;
    public GameObject nextPanel;


    void OnEnable()
    {
        scoreText.text = "Score : " + uiData.playerScore.ToString();
        // Assuming uiData.qrCodeSprite and uiData.aiGeneratedSprite are of type Sprite
        qrCodeImage.sprite = uiData.qrCodeSprite;
        aiGeneratedImage.sprite = uiData.aiGeneratedSprite;
    }


    public void NextBtnCLicked()
    {
        Debug.Log("Next Button Clicked. Proceeding to the next step.");
        // Implement logic to proceed to the next step
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
