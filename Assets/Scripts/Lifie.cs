using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifie : MonoBehaviour
{
    //stats
    public int Number = 0;
    public string Name = "";
    public int Type = 1;
    public float Level = 0;
    public int LP = 0;
    public int AP = 0;
    public int StatusCondition = 0;

    //movement
    int MovementCapacity = 2;
    Vector3 Velocity;
    float moveSpeed = 1;

    List<Tile> SelectableTiles;
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    List<string> TypesInString;
    List<string> StatusInString;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        Velocity = new Vector3();
        SelectableTiles = new List<Tile>();

        TypesInString = new List<string>();
        TypesInString.AddRange(new List<string>() { "Fire", "Water", "Plant", "Ground", "Machine", "Electric", "Spellcaster", "Beast", "Air", "Light", "Dark" });
        StatusInString = new List<string>();
        StatusInString.AddRange(new List<string>() { "", "Burned", "Frozen", "Paralyzed", "Poisoned", "Asleep", "Confused" });

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        //tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();
        
        Debug.Log("adj count: " + currentTile.adjacencyList.Count);

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            SelectableTiles.Add(t);
            t.selectable = true;
            
            if (t.distance < MovementCapacity)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    Debug.Log("t.adjacencyList.Count = " + t.adjacencyList.Count);

                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }

    }

    public int GetMovementCapacity()
    {
        return MovementCapacity;
    }

    public string TypeToString()
    {
        return TypesInString[Type-1];
    }

    public string LevelToString()
    {
        return "Lvl. " + Level;
    }
    public string APToString()
    {
        return "AP: " + AP;
    }
    public string LPToString()
    {
        return "LP: " + LP;
    }

    public string StatusConditionToString()
    {
        return StatusInString[StatusCondition];
    }

    public bool Move(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) > MovementCapacity)
        {
            return false;
        }
        else
        {
            transform.position = position;
            return true;
        }
    }
}
