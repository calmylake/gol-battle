﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int type = 0;
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacencyList;

    //needed for BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    void Start()
    {
        adjacencyList = new List<Tile>();
        
        if(type > 0 && type <= 5) GetComponent<Renderer>().material = Resources.Load<Material>("tilestextures/"+type);
    }

    private void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        } else if (target) 
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;

        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;

        }
    }

    public void Reset()
    {
        adjacencyList.Clear();

        walkable = true;
        current = false;
        target = false;
        selectable = false;
        
        visited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbors(int type)
    {
        Reset();

        CheckTile(Vector3.forward, type);
        CheckTile(-Vector3.forward, type);
        CheckTile(Vector3.right, type);
        CheckTile(-Vector3.right, type);

    }

    public void CheckTile(Vector3 dir, int type)
    {
        

        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, halfExtents);

        foreach (Collider collider in colliders)
        {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null)
            {
                if (tile.walkable)
                {
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(tile.transform.position, new Vector3(0, 0.5f, 0));
                    if (type != 0) adjacencyList.Add(tile);
                    else if (hits.Length == 0) adjacencyList.Add(tile);
                    else foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject.tag != "Lifie")
                        {
                            adjacencyList.Add(tile);
                            break;
                        }
                    }
                }
            }
        }
    }
}
