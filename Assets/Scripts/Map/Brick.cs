using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private Map _map;
    public Player owner;
    public int x;
    public int z;

/*    private Material _defaultMat;
    private Material _pOneMat;
    private Material _pTwoMat;
    private Material _pTreeMat;
    private Material _pFourMat; */
    // Start is called before the first frame update
    void Start()
    {
        _map = transform.root.GetChild(0).GetComponent<Map>();
        /*_defaultMat = Resources.Load<Material>("Materials/Default");
        _pOneMat = Resources.Load<Material>("Materials/One");
        _pTwoMat = Resources.Load<Material>("Materials/Two");
        _pTreeMat = Resources.Load<Material>("Materials/Tree");
        _pFourMat = Resources.Load<Material>("Materials/Four");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBarriers() {
        for (int i = 0; i < 4; i++)
        {
            Barrier barriere = transform.GetChild(0).GetChild(i).GetComponent<Barrier>();
            barriere.check(i);
        }
    }

    public void setFloor() {
        if (tag != "Floor") {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                -1,
                transform.localPosition.z

            );

            transform.root.GetComponent<Game>().maxScore += 1;
            tag = "Floor";
            gameObject.layer = 13;
        }
    }

    public void setWall() {
        if (tag != "Wall") {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                0,
                transform.localPosition.z
            );

            //transform.root.GetComponent<Game>().maxScore -= 1;
            tag = "Wall";
        }        
    }

    public void setOwnerShip(Player player) {
        if (tag != "Wall") {
            tag = "Floor";

            if (!owner) {
                if (owner) {
                    owner.rmScore();
                }

                owner = player;

                transform.GetComponent<MeshRenderer>().material.SetColor("playerColor", owner.color);
                owner.addScore();
                
                setBarriers();
            }
        }
    }

    public float isAvailable () {
        if (tag != "Wall") {
            return 1;
        } else {
            return 0;
        }
    }
}
