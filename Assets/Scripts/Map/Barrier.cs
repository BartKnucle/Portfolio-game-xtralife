using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public void check (int numBarrier) {
        Brick brick = transform.parent.parent.GetComponent<Brick>();
        int x = brick.x; //(int)transform.parent.parent.position.x;
        int z = brick.z; // (int)transform.parent.parent.position.z;

        switch (numBarrier)
        {
            case 0:
                if (x < transform.root.GetChild(0).GetComponent<Map>().sizeX - 1) {
                    Brick nextBrick = transform.root.GetChild(0).GetChild(x + 1).GetChild(z).GetComponent<Brick>();
                    /*Brick nextBrick = GameObject.Find(
                        (x + 1) + ":" + (z)
                    ).GetComponent<Brick>();*/

                    if (brick.owner == nextBrick.owner) {
                        hide();
                        nextBrick.transform.GetChild(0).GetChild(2).GetComponent<Barrier>().hide();
                        
                    } else {
                        show();
                        nextBrick.transform.GetChild(0).GetChild(2).GetComponent<Barrier>().show();
                    }
                }                
                break;
            case 1:
                if (z > 0) {
                    Brick nextBrick = transform.root.GetChild(0).GetChild(x).GetChild(z - 1).GetComponent<Brick>();
                    /*
                    Brick nextBrick = GameObject.Find(
                        (x) + ":" + (z - 1)
                    ).GetComponent<Brick>();
                    */

                    if (brick.owner == nextBrick.owner) {
                        hide();
                        nextBrick.transform.GetChild(0).GetChild(3).GetComponent<Barrier>().hide();
                    } else {
                        show();
                        nextBrick.transform.GetChild(0).GetChild(3).GetComponent<Barrier>().show();
                    }
                }
                break;
            case 2:
                if (x > 0) {
                    Brick nextBrick = transform.root.GetChild(0).GetChild(x - 1).GetChild(z).GetComponent<Brick>();
                    /*Brick nextBrick = GameObject.Find(
                        (x - 1) + ":" + (z)
                    ).GetComponent<Brick>();*/

                    if (brick.owner == nextBrick.owner) {
                        hide();
                        nextBrick.transform.GetChild(0).GetChild(0).GetComponent<Barrier>().hide();
                    } else {
                        show();
                        nextBrick.transform.GetChild(0).GetChild(0).GetComponent<Barrier>().show();
                    }
                }
                break;
            case 3:
                if (z < transform.root.GetChild(0).GetComponent<Map>().sizeZ - 1) {
                    Brick nextBrick = transform.root.GetChild(0).GetChild(x).GetChild(z + 1).GetComponent<Brick>();

                    /*Brick nextBrick = GameObject.Find(
                        (x) + ":" + (z + 1)
                    ).GetComponent<Brick>();*/

                    if (brick.owner == nextBrick.owner) {
                        hide();
                        nextBrick.transform.GetChild(0).GetChild(1).GetComponent<Barrier>().hide();
                    } else {
                        show();
                        nextBrick.transform.GetChild(0).GetChild(1).GetComponent<Barrier>().show();
                    }
                }
                break;
        }
    }

    public void show() {
        if (transform.parent.parent.tag != "Wall") {
            //transform.GetComponent<MeshRenderer>().enabled = true;
            transform.GetComponent<MeshCollider>().enabled = true;
        }
    }

    public void hide() {
        //transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<MeshCollider>().enabled = false;
    }
}
