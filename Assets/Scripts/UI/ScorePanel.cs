using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public GameObject nextPanel;


    void OnEnable()
    {
        scoreText.text = "Score : " + uiData.playerScore.ToString();
        
    }


    public void NextBtnCLicked()
    {
        Debug.Log("Next Button Clicked. Proceeding to the next step.");
        // Implement logic to proceed to the next step
        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
