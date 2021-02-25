using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    private Button _join;
    private Text _status;

    public bool inTeam = false;
    public bool bots = false;

    void Awake() {
        _status = transform.GetChild(1).GetChild(0).GetComponent<Text>(); // GameObject.Find("/UI/Status/Text").GetComponent<Text>();
    }
    public void setStatus(string text) {
        _status.text = text;
    }

    public void showLobby() {
        transform.GetChild(0).GetComponent<Canvas>().enabled = true;
        //GameObject.Find("/UI/Lobby").GetComponent<Canvas>().enabled = true;
    }

    public void hideLobby() {
        transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        //GameObject.Find("/UI/Lobby").GetComponent<Canvas>().enabled = false;
    }

    public void setLobbyTeam() {
        inTeam = !inTeam;
    }

    public void setLobbyBots() {
        bots = !bots;
    }
}