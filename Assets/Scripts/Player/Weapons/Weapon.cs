using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Ammo ammo;
    [HideInInspector]
    public Player owner;
    public float cooldown = 1;
    [HideInInspector]
    public string key = "a";

    private GameObject _ammos;
    private float _timeBeforeShoot = 0;

    public virtual void Awake() {
        _ammos = transform.root.GetChild(2).gameObject; //  GameObject.Find("/" + transform.root.name + "/Ammos");
        Physics.IgnoreLayerCollision(10, 8);
        Physics.IgnoreLayerCollision(10, 10);
    }

    public virtual void Update() {
        _timeBeforeShoot += Time.deltaTime;
        if (Input.GetKeyDown(key) && _timeBeforeShoot >= cooldown) {
            shoot();
            _timeBeforeShoot = 0;
        }
    }

    public virtual void shoot() {
        GameObject _ammo = Instantiate(ammo.gameObject, transform.position, transform.rotation, transform);
        _ammo.SetActive(true);
        _ammo.GetComponent<Ammo>().owner = transform.parent.parent.GetComponent<Player>();
        _ammo.transform.parent = _ammos.transform;
    }
}
