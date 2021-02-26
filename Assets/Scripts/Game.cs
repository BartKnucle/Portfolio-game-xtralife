using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CotcSdk;

public class Game : MonoBehaviour
{
    private Map _map;
    private Player _player;
    public string ID = "";
    public bool teams;
    public bool bot;
    public int totalScore = 0;
    public int maxScore = 0;
    public float time = 0;
    public float bestScore = 0;

    public List<Color> Colors = new List<Color>();

    public void Awake() {
        _map = transform.GetChild(0).GetComponent<Map>();
        _player = transform.GetChild(1).GetChild(0).GetComponent<Player>();
        _player.color = Colors[0];
    }

    /*void Start() {
        _map.generateMaze();

        _player.name = "Player0";
        _player.reset();
    }*/

    void Update () {
        time += Time.deltaTime;
        GameObject.Find("/UI/game/score").GetComponent<Text>().text = time.ToString().Split(',')[0];
        _checkEndGame();

        if (Input.GetKey("space")) {
            if (GameObject.Find("/UI/hello").GetComponent<Canvas>().enabled == true) {
              GameObject.Find("/UI/hello").GetComponent<Canvas>().enabled = false;
              Cotc.instance.init();
            }
        }
    }
    
    public void create(string id, int seed) {
        this.ID = id;
        _map.seed = seed;
    }

    public void reset() {
      postScore((long)time);
      time = 0;
      totalScore = 0;
      maxScore = 0;
      bestScore = 0;
      this.ID = "";
      //_map.generateMaze();
      _map.reset();
      transform.GetChild(3).GetComponent<Minimap>().reset();
      transform.GetChild(1).GetChild(0).GetComponent<Player>().reset();
    }

    private void _checkEndGame() {
        if (totalScore == maxScore && totalScore != 0) {
            reset();
        }
    }

    public void addScore() {
        totalScore += 1;
        //_checkEndGame();
    }

    public void rmScore() {
        totalScore += 1;
        //_checkEndGame();
    }

    void postScore(long score)
    {
        // currentGamer is an object retrieved after one of the different Login functions.

        Cotc.instance.gamer.Scores.Domain("private").Post(score, "intermediateMode", ScoreOrder.HighToLow,
        "context for score", false)
        .Done(postScoreRes => {
            Debug.Log("Post score: " + postScoreRes.ToString());
        }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }
}

