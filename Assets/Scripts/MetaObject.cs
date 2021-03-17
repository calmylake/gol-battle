using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaObject : MonoBehaviour
{
    List<Tile> TileList;

    void Start()
    {
        TileList = new List<Tile>();
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Tile"))
        {
            TileList.Add(o.GetComponent<Tile>());
        }
    }

    void Update()
    {

        
    }

    public void ResetTiles()
    {
        foreach(Tile t in TileList)
        {
            t.Reset();
        }
    }

}
