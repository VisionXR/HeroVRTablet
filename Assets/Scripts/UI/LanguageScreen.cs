using TMPro;
using UnityEngine;

public class LanguageScreen : MonoBehaviour
{

    public UIDataSO uiDataSO;

    [Header("Panels")]
    public GameObject NextPanel;
    public GameObject languageDD;



    public void NextBtnClicked()
    {
        uiDataSO.selectedLanguage = (Language)languageDD.GetComponent<TMP_Dropdown>().value;
        NextPanel.SetActive(true);       
        gameObject.SetActive(false);
    }


}
