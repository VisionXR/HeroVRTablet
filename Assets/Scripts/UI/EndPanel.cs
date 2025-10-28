using UnityEngine;

public class EndPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public SocketDataSO socketData;


    [Header(" GameObjects")]    
    public GameObject homePanel;

    public void HomeBtnClicked()
    {
        PacketData data = new PacketData();
        data.eventCode = EventCode.Home;
        data.jsonData = "GoToHome";
        socketData.SendDataToServer(JsonUtility.ToJson(data));

        homePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
