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
    public float realScore = 0;
    private bool _started = false;

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
        realScore = totalScore - Mathf.Round(time);
        GameObject.Find("/UI/game/score").GetComponent<Text>().text = realScore.ToString().Split(',')[0];
        _checkEndGame();

        if (Input.GetKey("space")) {
            if (GameObject.Find("/UI/warmup").GetComponent<Canvas>().enabled == true) {
              GameObject.Find("/UI/goodScore").GetComponent<Canvas>().enabled = false;
              GameObject.Find("/UI/badScore").GetComponent<Canvas>().enabled = false;
              GameObject.Find("/UI/warmup").GetComponent<Canvas>().enabled = false;
              GameObject.Find("/UI/hello").GetComponent<Canvas>().enabled = false;
              GameObject.Find("/UI/game/score").GetComponent<Text>().enabled = true;
              reset();
            }
        }
    }
    
    public void create(string id, int seed) {
        this.ID = id;
        _map.seed = seed;
    }

    void stop() {
      _started = false;
      GameObject.Find("/UI/game/score").GetComponent<Text>().enabled = false;
      postScore();
    }

    public void reset() {
      _started = true;
      time = 0;
      totalScore = 0;
      maxScore = 0;
      realScore = 0;
      this.ID = "";
      //_map.generateMaze();
      _map.reset();

      // reset ammos
      foreach (Transform trans in GameObject.Find("/Area/Ammos").transform)
      {
          Destroy(trans.gameObject);
      }

      transform.GetChild(3).GetComponent<Minimap>().reset();
      transform.GetChild(1).GetChild(0).GetComponent<Player>().reset();
    }

    private void _checkEndGame() {
        if (totalScore == maxScore && totalScore != 0 && _started == true) {
            stop();
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

    void postScore()
    {
        // currentGamer is an object retrieved after one of the different Login functions.

        Cotc.instance.gamer.Scores.Domain("private").Post((long)realScore, "intermediateMode", ScoreOrder.HighToLow,
        "context for score", false)
        .Done(postScoreRes => {
            Debug.Log("Post score: " + postScoreRes.ToString());
            getBestScore();
        }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }

    void getBestScore() {
      Cotc.instance.gamer.Scores.Domain("private").ListUserBestScores()
      .Done(listUserBestScoresRes => {
          foreach(var score in listUserBestScoresRes)
              if (score.Key == "intermediateMode") {
                if (score.Value.Value <= realScore) {
                  GameObject.Find("/UI/goodScore").GetComponent<Canvas>().enabled = true;
                  GameObject.Find("/UI/goodScore/good/txtScore").GetComponent<Text>().text = realScore.ToString();
                  GameObject.Find("/UI/warmup").GetComponent<Canvas>().enabled = true;
                } else {
                  GameObject.Find("/UI/badScore").GetComponent<Canvas>().enabled = true;
                  GameObject.Find("/UI/badScore/txtBad/txtCurrent").GetComponent<Text>().text = realScore.ToString().Split(',')[0];
                  GameObject.Find("/UI/badScore/txtBad/txtBest").GetComponent<Text>().text = score.Value.Value.ToString();
                  GameObject.Find("/UI/warmup").GetComponent<Canvas>().enabled = true;
                }
              }
      }, ex => {
          // The exception should always be CotcException
          CotcException error = (CotcException)ex;
          Debug.LogError("Could not get user best scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
      });
    }
}

