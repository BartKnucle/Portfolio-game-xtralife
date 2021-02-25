using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using HybridWebSocket;

public class Network : MonoBehaviour
{
    public string server = "ws://localhost";
    public int port = 3000;
    public Queue messagesQ = new Queue();
    private Messages _messages;
    private Player _player;

    public WebSocket ws;
    Ui _ui;

    private bool _connected = false;

    void Awake() 
    {
        _messages = transform.root.GetChild(5).GetComponent<Messages>(); //GameObject.Find("/Messages").GetComponent<Messages>();
        _player = transform.root.GetChild(1).GetChild(0).GetComponent<Player>(); //GameObject.Find("/Area/Players/Player").GetComponent<Player>();
        _ui = transform.root.GetChild(3).GetComponent<Ui>(); //GameObject.Find("/UI").GetComponent<Ui>();
    }
    

    void Start()
    {
        _ui.setStatus("Connecting server");
        ws = WebSocketFactory.CreateInstance(server + ":" + port);

        // Add OnOpen event listener
        ws.OnOpen += _onOpen;
        // Add OnClose event listener
        ws.OnClose += _onClose;
        // Add OnMessage event listener
        ws.OnMessage += _onMessage;
        // Add OnError event listener
        //ws.OnError += _onError;
        // Connect to the server
        ws.Connect();
    }

    void OnApplicationQuit()
    {
        ws.Close();
    }

    public void send(string msg) {
        if (_connected) {
            ws.Send(Encoding.UTF8.GetBytes(msg));
        }
    }

    private void _onOpen() {
        _connected = true;
        _player.sendId();
        _ui.showLobby();
        _ui.setStatus("Connected");
    }

    private void _onClose(WebSocketCloseCode code) {
        _connected = false;
        _ui.setStatus("Disconnected");
    }

    private void _onMessage(byte[] msg) {
        string message = Encoding.UTF8.GetString(msg);
        messagesQ.Enqueue(message);
        //_messages.dispatch(message);
    }

    private void _onError(string errMsg) {
        Debug.Log("WS error: " + errMsg);
    }

    public void joinLobby() {
        send("network/joinLobby/" +_ui.inTeam + "/" + _ui.bots );
        _ui.hideLobby();
    }
}
