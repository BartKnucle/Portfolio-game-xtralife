using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private Transform _camera;

    void Awake() {
        _camera = transform.GetChild(0);
    }

    void Start()
    {
        reset();
    }

    public void reset() {
        _camera.GetComponent<Camera>().orthographicSize = transform.root.GetChild(0).GetComponent<Map>().sizeX / 2;
        _camera.localPosition = new Vector3(
            transform.root.GetChild(0).GetComponent<Map>().sizeX / 2,
            10,
            transform.root.GetChild(0).GetComponent<Map>().sizeZ / 2
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
