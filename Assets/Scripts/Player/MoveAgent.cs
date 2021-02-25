using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MoveAgent : Agent
{
    private Player _player;
    private Map _map;
    private Game _game;
    EnvironmentParameters defaultParameters;

    public int previousScore = 0;
    public override void Initialize()
    {
        _player = transform.GetComponent<Player>();
        _map = transform.root.GetChild(0).transform.GetComponent<Map>();
        _game = transform.root.GetComponent<Game>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_player.transform.position);
        sensor.AddObservation(_player.transform.forward);
    }
 
    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-2f / MaxStep);

        if (vectorAction[0] == 1) { _player.move("up"); }
        if (vectorAction[0] == 2) { _player.move("down"); }

        if (vectorAction[1] == 1) { _player.move("left"); }
        if (vectorAction[1] == 2) { _player.move("right"); }

        if (_player.aiScore > previousScore ) {
            AddReward (_player.aiScore - previousScore);
            previousScore = _player.aiScore;
        }

        if (_game.totalScore == _game.maxScore && _game.totalScore != 0) {
            if (_game.bestScore == _player.aiScore) {
                AddReward(10f);
            } else {
                AddReward(-3f);
            }
            EndEpisode();
        }
    }
 
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = actionsOut[1] = 0;

        actionsOut[0] = Input.GetKey("up") ? 1 : 0;
        actionsOut[0] = Input.GetKey("down") ? 2 : 0;
        actionsOut[1] = Input.GetKey("left") ? 1 : 0;
        actionsOut[1] = Input.GetKey("right") ? 2 : 0;
    }
 
    public override void OnEpisodeBegin()
    {
        previousScore = 0;
        _game.reset();
    }
}
