using TMPro;
using UnityEngine;

public class SubmitScreen : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public SocketDataSO socketData;

    [Header("UI Elements")]
    public TMP_Text nameText;
    public TMP_Text emailText;
    public PlayerInfo playerInfo;
    public GameObject nextPanel;

    void OnEnable()
    {
        nameText.text = "Name : "+uiData.playerName;
        emailText.text = "Email : "+uiData.playerEmail;

      

    }

   
    public void StartRideBtnClicked()
    {
         Debug.Log("Ride Started for " + uiData.playerName + " in " + uiData.selectedCity);

        playerInfo.playerName = uiData.playerName;
        playerInfo.playerEmail = uiData.playerEmail;
        playerInfo.selectedGender = uiData.selectedGender;
        playerInfo.selectedCity = uiData.selectedCity;
        playerInfo.selectedLanguage = uiData.selectedLanguage;

        string playerDataJson = JsonUtility.ToJson(playerInfo); 

        PacketData data = new PacketData();
        data.eventCode = EventCode.PlayerInfo;
        data.jsonData = playerDataJson;

        socketData.SendDataToServer(JsonUtility.ToJson(data));
        // Implement ride starting logic here

        nextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
