using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGun : Weapon
{
    // Start is called before the first frame update
     public override void Awake()
    {
        base.Awake();
        key = "z";
    }

    public override void Update() {
        base.Update();
    }
}
