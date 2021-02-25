using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class ia : MonoBehaviour
{
    [Range(1, 20)]
    public int nb = 10;

    void Start()
    {
        GameObject area = GameObject.Find("/Area");

        for (int i = 0; i < nb; i++)
        {
            GameObject trainArea = GameObject.Instantiate(area);
            trainArea.transform.localPosition = new Vector3(
                (i + 1) * area.transform.GetChild(0).GetComponent<Map>().sizeX + 1,
                0,
                0
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
