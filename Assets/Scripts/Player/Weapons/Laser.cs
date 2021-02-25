using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //public Vector3 position;
    private LineRenderer _line;
    public RaycastHit hit;

    private float _maxDistance = 0;
    void Awake()
    {
        _line = transform.GetComponent<LineRenderer>();
        _maxDistance = transform.root.GetChild(0).GetComponent<Map>().sizeX + transform.root.GetChild(0).GetComponent<Map>().sizeZ;
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance , layerMask))
        {
            //position = hit.transform.position;
            _line.SetPosition(1, Vector3.forward * hit.distance);
        }
    }
}
