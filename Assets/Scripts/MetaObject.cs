using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaObject : MonoBehaviour
{
    public bool playerTurn = true;

    bool firstTime;
    List<GameObject> Tiles;
    List<GameObject> Lifies;
    List<GameObject> PlayableLifies;
    List<GameObject> OpponentLifies;

    void Start()
    {

        Tiles = new List<GameObject>();
        ReadCurrentTiles();

        Lifies = new List<GameObject>();
        ReadCurrentLifies();

        PlayableLifies = new List<GameObject>();
        OpponentLifies = new List<GameObject>();
        ReadLifieLists();

        firstTime = true;

        playerTurn = !playerTurn;
        nextTurn();
    }

    void Update()
    {
        
    }

    private void ReadLifieLists()
    {
        bool tempBool;
        PlayableLifies.Clear();
        OpponentLifies.Clear();
        ReadCurrentLifies();
        foreach (GameObject Lifie in Lifies)
        {
            if (playerTurn) tempBool = Lifie.GetComponent<LifiePlayer>(); 
            else tempBool = Lifie.GetComponent<LifieCPU>();
            
            if (tempBool)
            {
                PlayableLifies.Add(Lifie);
            } else
            {
                OpponentLifies.Add(Lifie);
            }
        }
    }

    private void ReadCurrentTiles()
    {
        Tiles.Clear();
        foreach (GameObject Tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Tiles.Add(Tile);
        }
    }

    private void ReadCurrentLifies()
    {
        Lifies.Clear();
        foreach (GameObject Lifie in GameObject.FindGameObjectsWithTag("Lifie"))
        {
            Lifies.Add(Lifie);
        }
    }

    public void ResetTiles()
    {
        foreach(GameObject t in Tiles)
        {
            t.GetComponent<Tile>().Reset();
        }
    }

    public GameObject[] getTileList()
    {
        return Tiles.ToArray();
    }

    public bool isFirstTime()
    {
        if (firstTime == true)
        {
            firstTime = false;
            return true;
        } else return false;
    }

    public void nextTurn()
    {
        playerTurn = !playerTurn;
        ReadLifieLists();

        foreach (GameObject Lifie in PlayableLifies.ToArray())
        {
            Lifie.GetComponent<Lifie>().Enable();
        }
        
        foreach(GameObject Lifie in OpponentLifies.ToArray())
        {
            Lifie.GetComponent<Lifie>().Disable();
        }


    }

    public void CheckForNextTurn()
    {
        ReadLifieLists();
        foreach (GameObject Lifie in PlayableLifies)
        {
            if (Lifie.GetComponent<Lifie>().CanWalk)
            {
                return;
            }
        }

        nextTurn();
    }

}
