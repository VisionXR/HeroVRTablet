using UnityEngine;

public class CitySelectionScreen : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;

    [Header("Selection Images")]
    public GameObject delhiBgImage;
    public GameObject milanBgImage;
    public GameObject saoPauloBgImage;
    public GameObject manilaBgImage;


    [Header(" GameObjects")]
    public GameObject nextPanel;
    public GameObject previousPanel;

    // local

    private void OnEnable()
    {
        ResetBg();
        DelhiBtnClicked();
    }

    public void OnPreviousButtonClick()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
    }


    public void DelhiBtnClicked()
    {
        ResetBg();
        delhiBgImage.SetActive(true);
        uiData.selectedCity = City.Delhi;
    }

    public void MilanBtnClicked()
    {
        ResetBg();
        milanBgImage.SetActive(true);
        uiData.selectedCity = City.Milan;
    }

    public void SaoPauloBtnClicked()
    {
        ResetBg();
        saoPauloBgImage.SetActive(true);
        uiData.selectedCity = City.SaoPaulo;
    }

    public void ManilaBtnClicked()
    {
        ResetBg();
        manilaBgImage.SetActive(true);
        uiData.selectedCity = City.Manila;
    }


    public void NextBtnClicked()
    {
               // Proceed to the next screen or perform any action needed
        Debug.Log($"Selected City: {uiData.selectedCity}");
        // Example: gameObject.SetActive(false); nextScreen.SetActive(true);

        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }


    private void ResetBg()
    {
        delhiBgImage.SetActive(false);
        milanBgImage.SetActive(false);
        saoPauloBgImage.SetActive(false);
        manilaBgImage.SetActive(false);
    }
}
