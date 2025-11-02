using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

/// <summary>
/// Manages TCP socket communication between Unity (Android) and a desktop server.
/// Provides methods to connect, disconnect, and send data.
/// Incoming data is read on a background thread and pushed to main thread.
/// </summary>
public class ClientManager : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public SocketDataSO socketData;
    public UIDataSO uiData;

    // TCP client variables
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader reader;
    private StreamWriter writer;
    private Thread receiveThread;
    public bool isRunning;


    private bool isMessageReceived = false;
    private string receivedMessage = string.Empty;
    private void OnEnable()
    {
        socketData.ConnectToServerEvent += ConnectToServer; 
        socketData.SendDataToServerEvent += SendData;
    }

    private void OnDisable()
    {
        Disconnect();
        socketData.ConnectToServerEvent -= ConnectToServer;
        socketData.SendDataToServerEvent -= SendData;
    }

    /// <summary>
    /// Connect to the server using TCP at given IP and port.
    /// </summary>
    public void ConnectToServer()
    {
        if (isRunning) return;

        try
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(socketData.serverIP, socketData.serverPort);
            networkStream = tcpClient.GetStream();
            reader = new StreamReader(networkStream, Encoding.UTF8);
            writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            isRunning = true;

            // Start a background thread for receiving
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            socketData.SetConnectionStatus("Connected");

        }
        catch (System.Exception ex)
        {
            socketData.SetConnectionStatus("Not Connected");
        }
    }

    /// <summary>
    /// Disconnect and stop the TCP client.
    /// </summary>
    public void Disconnect()
    {
        isRunning = false;

        try
        {
            receiveThread?.Abort();
            reader?.Close();
            writer?.Close();
            networkStream?.Close();
            tcpClient?.Close();
        }
        catch { }

        Debug.Log("[SocketManager] TCP Disconnected.");
    }

    /// <summary>
    /// Send a UTF8-encoded string message to the server.
    /// </summary>
    public void SendData(string data)
    {
        Debug.Log("[SocketManager] Sending data...");
        Debug.Log(data);

        if (!isRunning || tcpClient == null || !tcpClient.Connected)
        {
            Debug.Log("[SocketManager] Cannot send data. Not connected.");
            socketData.SetConnectionStatus("Not Connected");
            return;
        }

        try
        {
            writer.WriteLine(data);
            Debug.Log($"[SocketManager] Sent: {data}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SocketManager] Send failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Background loop for receiving TCP messages.
    /// </summary>
    private void ReceiveLoop()
    {
        while (isRunning)
        {
            try
            {
                string message = reader.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    Debug.Log($"[SocketManager] Received: {message}");
                    receivedMessage = message;
                    isMessageReceived = true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SocketManager] Receive failed: {ex.Message}");
                socketData.SetConnectionStatus("Not Connected");
                isRunning = false;
                Disconnect();
                break;
            }
        }
    }


    private void Update()
    {
        if (tcpClient != null && isRunning && !tcpClient.Connected)
        {
            socketData.SetConnectionStatus("Not Connected");
            isRunning = false;
        }

        if (isMessageReceived)
        {
            isMessageReceived = false;
            PacketData packet = JsonUtility.FromJson<PacketData>(receivedMessage);
            if(packet.eventCode == EventCode.ScoreUpdate)
            {
                int newScore = int.Parse(packet.jsonData);
                uiData.playerScore = newScore;
                uiData.ShowScorePanel();
            }

        }
    }
}
