using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScreen : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public SocketDataSO socketData;

    [Header("UI Elements")]
    public List<Sprite> defaultImages;
    public RawImage aIImage;
    public TMP_Text nameText;
    public TMP_Text emailText;
    public PlayerInfo playerInfo;

    [Header("GameObjects")]
    public GameObject nextPanel;
    public GameObject previousPanel;

    void OnEnable()
    {
        nameText.text = "Name : "+uiData.playerName;
        emailText.text = "Email : "+uiData.playerEmail;

     
    }

    public void OnPreviousButtonClick()
    {
        previousPanel.SetActive(true);
        gameObject.SetActive(false);
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


        if(uiData.playerImage != null)
        {
            uiData.GenerateAIImage();
        }
        else
        {
            aIImage.texture = defaultImages[(int)uiData.selectedCity].texture;
            uiData.GenerateQRCode();
        }


            // Implement ride starting logic here

          nextPanel.SetActive(true);
          gameObject.SetActive(false);
    }
}
