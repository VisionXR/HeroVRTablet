using System.Collections;
using TMPro;
using UnityEngine;

public class WelcomePanel : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public SocketDataSO socketData; // Your scriptable object holding IP/port config

    [Header(" UI Elements")]
    public GameObject NextPanel;
    public GameObject SettingsPanel;

    private bool isActive = true;

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
    }

    //void Update()
    //{
    //    if (!isActive) return;

    //    // For mouse click or screen tap
    //    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
    //    {
    //        StartBtnClicked();
    //    }
    //}

    public void StartBtnClicked()
    {
        NextPanel.SetActive(true);
        gameObject.SetActive(false);
        isActive = false;
    }


    public void SettingsBtnClicked()
    {
        if (SettingsPanel.activeInHierarchy)
        {
            SettingsPanel.SetActive(false);
        }
        else
        {
            SettingsPanel.SetActive(true);
        }
    }
}
