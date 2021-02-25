using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages : MonoBehaviour

{
    private Network _network;
    private Player _player;
    private Ui _ui;
    private Game _game;

    void Awake() {
        _ui = transform.root.GetChild(3).GetComponent<Ui>(); //GameObject.Find("/UI").GetComponent<Ui>();
        _network = transform.root.GetChild(4).GetComponent<Network>(); // GameObject.Find("/Network").GetComponent<Network>();
        _player = transform.root.GetChild(1).GetChild(0).GetComponent<Player>(); //GameObject.Find("/Area/Players/Player").GetComponent<Player>();
        _game = transform.root.GetComponent<Game>();
    }

    void Update() {
        while(_network.messagesQ.Count != 0) {
            dispatch((string)_network.messagesQ.Dequeue());
        }
    }

    public void dispatch(string msgs) {
        string[] messages = msgs.Split('/');

        switch (messages[0])
        {
            case "player":
                switch (messages[1])
                {
                    case "sendId":
                        _network.send(msgs);
                        break;
                    case "position":
                        _network.send(msgs);
                        break;
                    default:
                        break;
                }
                break;
            case "lobby":
                switch (messages[1])
                {
                    case "changeRank":
                        _ui.setStatus("Waiting for other players, rank:" + messages[2]);
                        break;
                    default:
                        break;
                }
                break;
            case "game":
                switch (messages[1])
                {
                    case "create":
                        _ui.setStatus("Game found: " + messages[2]);
                        _game.create(messages[2], int.Parse(messages[3]));
                        break;
                    case "delete":
                        _ui.setStatus("Game: " + messages[2] + " has been deleted");
                        _ui.showLobby();
                        break;
                    case "yourIndex":
                        _player.index = int.Parse(messages[2]);
                        _game.reset();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
