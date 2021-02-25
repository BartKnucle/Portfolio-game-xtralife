using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGunAmmo : Ammo
{
    private bool _isSticked = false;
    private float _timeBeforeExplode = 3;

    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), true);
        GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
        GetComponent<MeshRenderer>().material.SetColor("playerColor", owner.color);
    }

    // Update is called once per frame
    void Update()
    {
         if (_isSticked) {
            _timeBeforeExplode -= Time.deltaTime;
            transform.localScale = Vector3.one * ( 0.1f + _timeBeforeExplode - 3);
            //GetComponent<MeshRenderer>().material = owner.mat;
        }

        if (_timeBeforeExplode <= 0) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter (Collision co) {
        if (!_isSticked) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<SphereCollider>().isTrigger = true;
            _isSticked = true;
        }
    }

    private void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "Floor") {
            Brick brick = co.gameObject.GetComponent<Brick>();
            if (brick) {
                brick.setOwnerShip(owner);
            }
        }
    }
}
