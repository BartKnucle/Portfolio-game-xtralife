using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Level generation seed
    public int seed = 0;
    [Range(10, 50)]
    public int sizeX = 20;

    [Range(10, 50)]
    public int sizeZ = 20;

    public GameObject brickPrefab;

    public void generateMaze() {
        seed = new System.Random().Next(1, 5000);

        Random.InitState(seed);
        _generateLines();
        _setStructure();
    }

    void Update() {
        if (transform.childCount == 0) {
            generateMaze();
        }
    }

    public void reset() {
        //Destroy old maze
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    private void _generateLines() {
        for (int x = 0; x < sizeX; x++)
        {
            GameObject line = new GameObject();
            line.name = "Line " + x;
            line.transform.parent = transform;
            line.transform.localPosition = Vector3.right * x;
            _generateColumns(x);
        }
    }

    private void _generateColumns(int x) {
        for (int z = 0; z < sizeZ; z++)
        {
            _addItem(x, z);
        }
    }

    private void _addItem(int x, int z) {
        GameObject item = Instantiate(brickPrefab, transform.GetChild(x));
        item.name = x + ":" + z;
        item.GetComponent<Brick>().x = x;
        item.GetComponent<Brick>().z = z;
        item.transform.localPosition = Vector3.forward * z;
    }

    private void _deleteItem(int x, int z) {
        GameObject item = transform.GetChild(x).transform.GetChild(z).gameObject;
        GameObject.Destroy(item);
    }

    public GameObject getItem(int x, int z) {       
        GameObject item = transform.GetChild(x).GetChild(z).gameObject;      
        return item;
    }

    public int getOwner(int x, int z) {
        Brick brick = getItem(x, z).GetComponent<Brick>();
        return brick.owner.index;
    }

    private void _setStructure() {
        _setStarts();
        _generatePaths();
    }

    
    private void _setStarts() {
        // Set new starts
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 1; j <= 2; j++)
            {
                _setFloor(i, j);
                _setFloor(i, sizeZ - 1 - j);
                _setFloor(sizeX - 1 - i, j);
                _setFloor(sizeX - 1 - i, sizeZ - 1 - j);   
            }
        }
    }

    private void _generatePaths() {
        for (int i = 2; i < (sizeX - 1); i++)
        {
            for (int j = 2; j < (sizeZ - 1); j++)
            {
                bool right = (Random.value > 0.5f);
                if (right) {
                    _setFloor(i - 1, j);
                } else {
                    _setFloor(i, j - 1);
                }
            }
        }
    }

    private void _setFloor(int x, int z) {
        transform.GetChild(x).transform.GetChild(z).GetComponent<Brick>().setFloor();
    }

    private void _setWall(int x, int z) {
        transform.GetChild(x).transform.GetChild(z).GetComponent<Brick>().setWall();
    }
}