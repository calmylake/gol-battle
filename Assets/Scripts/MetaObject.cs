using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaObject : MonoBehaviour
{
    public bool playerTurn = true;

    bool firstTime;
    List<GameObject> Tiles;
    List<GameObject> Lifies;
    List<GameObject> PlayableLifies;
    List<GameObject> OpponentLifies;

    bool isFading;

    void Start()
    {

        isFading = true;
        GameObject.Find("WhiteImage").GetComponent<Image>().color = new Color(1,1,1,1);

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
        foreach (GameObject tile in Tiles)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(tile.transform.position, new Vector3(0, 0.5f, 0));
            Lifie templifie;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag != "Lifie")
                {
                    templifie = hit.collider.gameObject.GetComponent<Lifie>();
                    break;
                }
            }
            switch (tile.GetComponent<Tile>().type)
            {
                case 1:

                default:
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        Fade();
    }

    void Fade()
    {
        if (isFading)
        {
            float valueToChange = Time.deltaTime / 3;

            Image tempImage = GameObject.Find("WhiteImage").GetComponent<Image>();
            tempImage.color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, tempImage.color.a - valueToChange);
            if (tempImage.color.a <= 0) isFading = false;
        }
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
            Lifie templifie = Lifie.GetComponent<Lifie>();
            templifie.Enable();
            templifie.AP += 20;
            if(templifie.AP > templifie.TotalAP) templifie.AP = templifie.TotalAP;
        }
        
        foreach(GameObject Lifie in OpponentLifies.ToArray())
        {
            Lifie.GetComponent<Lifie>().Disable();
        }

        GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText("Next Turn! Select a Lifie.");

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
