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
    public Texture2D playerImage;
    public Texture2D aiGeneratedImage;
    public Texture2D qrImage;

    public int playerScore;
    public int playerCount;
    public Sprite qrCodeSprite;
    public Sprite aiGeneratedSprite;


    //Actions

    public Action ShowScorePanelEvent;
    public Action<Texture2D, int> GenerateAIImageEvent;
    public int id = 1;


    //Methods

    void OnEnable()
    {
            
    }

    public void ShowScorePanel()
    {
        ShowScorePanelEvent?.Invoke();
    }

    public void GenerateAIImage()
    {
        
        GenerateAIImageEvent?.Invoke(playerImage, ((int)selectedCity+1));
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


