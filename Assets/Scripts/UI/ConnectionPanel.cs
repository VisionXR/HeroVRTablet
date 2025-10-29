using TMPro;
using UnityEngine;

public class ConnectionPanel : MonoBehaviour
{
   public ClientManager clientManager;
   public TMP_InputField ipInputField;
    public TMP_Text debubText;

    public void ConnectBtnClicked()
    {
        string ip = ipInputField.text;
        if (!string.IsNullOrEmpty(ip))
        {
            clientManager.socketData.serverIP = ip;
            ConnectToServer();
        }
    }

    public void ConnectToServer()
   {
       clientManager.ConnectToServer(clientManager.socketData.serverIP, clientManager.socketData.serverPort);
   }

   public void DisconnectFromServer()
   {
       clientManager.Disconnect();
   }

    public void UpdateConnectionStatus(string message)
    {
        debubText.text = message;
    }
}
