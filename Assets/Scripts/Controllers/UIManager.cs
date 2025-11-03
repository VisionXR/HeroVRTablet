using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public SocketDataSO socketData;

    [Header("Panels")]
    public GameObject scorePanel;
    public GameObject homePanel;
    public List<GameObject> uiPanels;

    private void OnEnable()
    {
        uiData.ShowScorePanelEvent += ShowScorePanel;
    }

    private void OnDisable()
    {
        uiData.ShowScorePanelEvent -= ShowScorePanel;
    }

    private void ShowScorePanel()
    {
        Reset();
        scorePanel.SetActive(true);
    }


    public void HomeBtnClicked()
    {
        PacketData data = new PacketData();
        data.eventCode = EventCode.Home;
        data.jsonData = "GoToHome";
        socketData.SendDataToServer(JsonUtility.ToJson(data));

        Reset();
        homePanel.SetActive(true);
    }
    private void Reset()
    {
        for (int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].SetActive(false);
        }
    }

}
