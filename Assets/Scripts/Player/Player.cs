using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents.Sensors;

public class Player : MonoBehaviour
{

    //public Material material;

    [HideInInspector]
    public string id;
    public int index = 0;
    // Player speed
    public int speed = 4;
    // Player number on the map (0 - 3)

    public Color color;

    public int gridPositionX;
    public int gridPositionZ;

    private Messages _messages;
    private Map _map;
    private Game _game;

    // For AI training
    public float aiStartTime = 0f;
    public int aiScore = 0;
    public int aiMaxScore = 0;
    public int aicollisions = 0;

    public Vector4 aiDestinations = new Vector4(0, 0, 0, 0);

    void Awake() {
        id = PlayerPrefs.GetString("_id");
        _messages = transform.root.GetChild(5).GetComponent<Messages>(); //GameObject.Find("/Messages").GetComponent<Messages>();
        _map = transform.root.GetChild(0).GetComponent<Map>();
        _game = transform.root.GetComponent<Game>();

        if (!PlayerPrefs.HasKey("user")) {
            id = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("user", id);
            PlayerPrefs.Save();
        } else {
            id = PlayerPrefs.GetString("user");
        }

        //transform.GetComponent<CameraSensor>().Camera = transform.root.GetChild(6).GetChild(0).GetComponent<Camera>();
    }

    void start () {
        gridPositionX = 0;
        gridPositionZ = 0;
        reset();
    }

    void OnCollisionEnter(Collision collision)
    {
        aicollisions += 1;
    }

    void Update() {
        // Rotate
        if (Input.GetKey("left")) {
            //transform.Rotate(new Vector3(0, -Time.deltaTime * speed * 50, 0));
            move("left");
        }
        if (Input.GetKey("right")) {
            //transform.Rotate(new Vector3(0, Time.deltaTime * speed * 50, 0));
            move("right");
        }

        if (transform.localPosition.x < 0 || transform.localPosition.x > _map.sizeX || transform.localPosition.z < 0 || transform.localPosition.z > _map.sizeZ) {
            reset();
        }
    }

    void FixedUpdate() {
        // Supress the physics
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //transform.rotation = Quaternion.Euler(new Vector3(0,transform.localRotation.y,0));

        // Go forward / backward
        if (Input.GetKey("up")) {
            move("up");
        }
        if (Input.GetKey("down")) {
            move("down");
        }
    }

    public void  move(string key) {
        int OldGridPositionX = gridPositionX;
        int OldGridPositionZ = gridPositionZ;

        aiStartTime += Time.deltaTime;
        
        switch (key)
        {
            case "up":
                GetComponent<Rigidbody>().MovePosition(
                    transform.position + transform.forward * speed * Time.deltaTime
                );
                break;
            case "down":
                //GetComponent<Rigidbody>().velocity = -transform.forward * speed * 30 * Time.deltaTime;
                GetComponent<Rigidbody>().MovePosition(
                    transform.position - transform.forward * speed * Time.deltaTime
                );
                break;
            case "left":
                //GetComponent<Rigidbody>().AddTorque(-Vector3.up * Time.deltaTime * speed * 30);
                transform.Rotate(new Vector3(0, -Time.deltaTime * speed * 40, 0));
                break;
            case "right":
                //GetComponent<Rigidbody>().AddTorque(Vector3.up * Time.deltaTime * speed * 30);
                transform.Rotate(new Vector3(0, Time.deltaTime * speed * 40, 0));
                break;
        }

        gridPositionX = (int)Mathf.Round(transform.localPosition.x);
        gridPositionZ = (int)Mathf.Round(transform.localPosition.z);

        // If we change grid position
        /*if (OldGridPositionX != gridPositionX || OldGridPositionZ != gridPositionZ) {
            _checkDestinations();
        }*/

        // Change the map ownership
        //Map map = GameObject.Find("/" + transform.parent.name +  "/Map").GetComponent<Map>();
        //map.GetOwnerShip(this, gridPositionX, gridPositionZ);

        // Send the new if we are ingame
        
        if (_game.ID != "") {
            _messages.dispatch("player/position/" + _game.ID + "/" + transform.localPosition.x + "/" + transform.localPosition.z);
        }
    }

    private void _checkDestinations() {
        float left = _map.getItem(gridPositionX - 1, gridPositionZ).GetComponent<Brick>().isAvailable() * isMine(gridPositionX - 1, gridPositionZ);
        float right = _map.getItem(gridPositionX + 1, gridPositionZ).GetComponent<Brick>().isAvailable() * isMine(gridPositionX + 1, gridPositionZ);
        float forward = _map.getItem(gridPositionX, gridPositionZ + 1).GetComponent<Brick>().isAvailable() * isMine(gridPositionX, gridPositionZ + 1);
        float backward = _map.getItem(gridPositionX, gridPositionZ - 1).GetComponent<Brick>().isAvailable() * isMine(gridPositionX, gridPositionZ - 1);

        aiDestinations = new Vector4(left, forward, right, backward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")  {
            other.GetComponent<Brick>().setOwnerShip(this);   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Floor")  {
            other.GetComponent<Brick>().setBarriers();
        }
    }

    public void sendId() {
        // Notify the server to replace the old id by the new one;
        _messages.dispatch("player/sendId/" + id);
    }

    public void addScore() {
        aiScore += 1;
        Game _game = transform.root.GetComponent<Game>();
        if (aiScore > _game.bestScore) {
            _game.bestScore = aiScore;
        }
        _game.addScore();
        refreshScore();
    }

    public void rmScore() {
        Game _game = transform.root.GetComponent<Game>();
        if (aiScore == _game.bestScore) {
            _game.bestScore -= 1;
        }
        aiScore -= 1;
        _game.rmScore();
        refreshScore();
    }

    private void refreshScore() {
        GameObject.Find("txt" + name).GetComponent<Text>().text = aiScore.ToString();
    }

    // Is the player ower of the brick
    public float isMine(int x, int z) {
        if (_map.getItem(x, z).GetComponent<Brick>().owner == transform.GetComponent<Player>()) {
            return 1;
        } else {
            return 0;
        }
    }

    public void reset() {
        //Map map = transform.root.GetChild(0).GetComponent<Map>();
        switch (index)
        {
            case 0:
                //mat = Resources.Load<Material>("Materials/One");
                transform.localPosition = new Vector3(1, 0, 1);
                transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;
            case 1:
                //mat = Resources.Load<Material>("Materials/Two");
                transform.localPosition = new Vector3(_map.sizeX - 2, 0, 1);
                transform.localRotation = Quaternion.Euler(-Vector3.up * 90);
                break;
            case 2:
                //mat = Resources.Load<Material>("Materials/Tree");
                transform.localPosition = new Vector3(1, 0, _map.sizeZ -2);
                transform.localRotation = Quaternion.Euler(Vector3.up * 90);
                break;
            case 3:
                //mat = Resources.Load<Material>("Materials/Four");
                transform.localPosition = new Vector3(_map.sizeX -2, 0, _map.sizeZ -2);
                transform.localRotation = Quaternion.Euler(-Vector3.up * 180);
                break;
            
        }
        
        transform.GetChild(1).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(2).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(12).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(13).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(14).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(16).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(17).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(18).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(22).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(23).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(24).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        transform.GetChild(1).GetChild(25).GetComponent<SkinnedMeshRenderer>().material.SetColor("playerColor", color);
        
        aiScore = 0;
        aicollisions = 0;
        _checkDestinations();
    }
}

