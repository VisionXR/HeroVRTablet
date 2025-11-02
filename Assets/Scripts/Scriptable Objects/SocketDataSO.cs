using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SocketDataSO", menuName = "ScriptableObjects/SocketDataSO")]
public class SocketDataSO : ScriptableObject
{

    // Variables
    public string serverIP;
    public int serverPort;


    //Actions
    public Action ConnectToServerEvent;
    public Action DisconnectEvent;
    public Action<int> StartServerEvent;
    public Action StopServerEvent;
    public Action<string> ConnectionStatusChangedEvent;

    public Action<string> SendDataToServerEvent;


    //Methods
    public void ConnectToServer()
    {
        ConnectToServerEvent?.Invoke();
    }

    public void Disconnect()
    {
        DisconnectEvent?.Invoke();
    }


    public void StartServer()
    {
        StartServerEvent?.Invoke(serverPort);
    }

    public void StopServer()
    {
        StopServerEvent?.Invoke();
    }


    public void SendDataToServer(string message)
    {
        SendDataToServerEvent?.Invoke(message);
    }


    public void SetConnectionStatus(string status)
    {
        ConnectionStatusChangedEvent?.Invoke(status);
    }
}
