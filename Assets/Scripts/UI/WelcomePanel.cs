using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomePanel : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public SocketDataSO socketData; // Your scriptable object holding IP/port config

    [Header(" UI Panels")]
    public GameObject NextPanel;
    public GameObject SettingsPanel;

    [Header(" UI Elements")]
    public TMP_InputField IPInputField;
    public TMP_Text connectionStatusText;
    public Image connectionImage;

    private bool isActive = true;

    void OnEnable()
    {
        isActive = true;
        socketData.ConnectionStatusChangedEvent += ConnectionStatusChanges;
    }

    void OnDisable()
    {
        isActive = false;
        socketData.ConnectionStatusChangedEvent -= ConnectionStatusChanges;
    }

    private void ConnectionStatusChanges(string status)
    {
        connectionStatusText.text = "Connection Status: " + status;

        if(status == "Connected")
        {
            connectionImage.color = Color.green;
        }
        else if (status == "Not Connected")
        {
            connectionImage.color = Color.red;
        }
        else
        {
            connectionImage.color = Color.yellow;
        }
    }



    void Update()
    {
        if (!isActive) return;

    }

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

    public void ConnectBtnClicked()
    {
       if(IPInputField.text != "")
       {
            socketData.serverIP = IPInputField.text;
            socketData.ConnectToServer();
            Debug.Log("Server IP set to: " + socketData.serverIP);
       }
       else
       {
            Debug.LogWarning("Server IP input field is empty.");
        }
    }
}
