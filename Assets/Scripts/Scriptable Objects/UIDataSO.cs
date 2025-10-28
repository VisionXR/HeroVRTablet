using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIDataSO", menuName = "ScriptableObjects/UIDataSO")]
public class UIDataSO : ScriptableObject
{

   // Variables
    public string playerName;
    public string playerEmail;
    public Gender selectedGender;
    public City selectedCity;
    public Language selectedLanguage;

    public int playerScore;
    public int playerCount;
    public Sprite qrCodeSprite;
    public Sprite aiGeneratedSprite;


    //Actions

    public Action ShowScorePanelEvent;


    //Methods

    public void ShowScorePanel()
    {
        ShowScorePanelEvent?.Invoke();
    }
}

[Serializable]
public enum Gender { Male,Female}

[Serializable]
public enum City { Delhi, Milan, SaoPaulo, Manila }

[Serializable]
public enum Language { English, Italian }

[Serializable]
public enum EventCode { PlayerInfo =1, StartGame=2,  ScoreUpdate=3,Home = 4}

[Serializable]
public class PacketData
{
    public EventCode eventCode;
    public string jsonData;
}

[Serializable]
public class PlayerInfo
{
    public string playerName;
    public string playerEmail;
    public Gender selectedGender;
    public City selectedCity;
    public Language selectedLanguage;
}


