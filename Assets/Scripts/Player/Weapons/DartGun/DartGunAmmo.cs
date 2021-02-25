using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGunAmmo : Ammo
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), true);
        GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "Bubble") {
            Destroy(co.gameObject);
            Destroy(gameObject);
        }
    }
}
