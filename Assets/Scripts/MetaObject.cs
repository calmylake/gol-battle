using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MetaObject : MonoBehaviour
{
    public bool playerTurn = true;

    bool firstTime;
    List<GameObject> Tiles;
    List<GameObject> Lifies;
    List<GameObject> Player1Lifies;
    List<GameObject> Player2Lifies;

    int isFading;
    string VictoryText;

    void Start()
    {
        isFading = 1;
        GameObject.Find("WhiteImage").GetComponent<Image>().color = new Color(1,1,1,1);
        GameObject.Find("WhiteImage").GetComponent<Image>().enabled = true;

        GameObject.Find("VictoryText").GetComponent<TextMeshProUGUI>().enabled = false;

        Tiles = new List<GameObject>();
        ReadCurrentTiles();

        Lifies = new List<GameObject>();
        ReadCurrentLifies();

        Player1Lifies = new List<GameObject>();
        Player2Lifies = new List<GameObject>();
        ReadLifieLists();

        firstTime = true;

        playerTurn = !playerTurn;
        nextTurn();

        Destroy(Player1Lifies[1]);
        Destroy(Player1Lifies[2]);
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
        Debug.Log(isFading);
        if (isFading == 2)
        {
            float valueToChange = Time.deltaTime * 3;

            Image tempImage = GameObject.Find("WhiteImage").GetComponent<Image>();
            GameObject.Find("WhiteImage").GetComponent<Image>().color = new Color(0, 0, 0, tempImage.color.a + valueToChange);
            if (GameObject.Find("WhiteImage").GetComponent<Image>().color.a >= 1) {
                GameObject.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = VictoryText;
                GameObject.Find("VictoryText").GetComponent<TextMeshProUGUI>().enabled = true;
                isFading = 0;
            }
            Debug.Log("white image color.a: " + GameObject.Find("WhiteImage").GetComponent<Image>().color.a);
            return;
        }
        if (isFading == 1)
        {
            float valueToChange = Time.deltaTime / 3;

            Image tempImage = GameObject.Find("WhiteImage").GetComponent<Image>();
            GameObject.Find("WhiteImage").GetComponent<Image>().color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, tempImage.color.a - valueToChange);
            if (GameObject.Find("WhiteImage").GetComponent<Image>().color.a <= 0) isFading = 0;
            Debug.Log(GameObject.Find("WhiteImage").GetComponent<Image>().color.a);
        }
    }

    public void ReadLifieLists()
    {
        Player1Lifies.Clear();
        Player2Lifies.Clear();
        ReadCurrentLifies();
        foreach (GameObject Lifie in Lifies)
        {
            if (Lifie.GetComponent<LifiePlayer>() != null)
            {
                Player1Lifies.Add(Lifie);
            }
            else if (Lifie.GetComponent<LifieCPU>() != null)
            {
                Player2Lifies.Add(Lifie);
            }
        }
        Debug.Log("Lifie Lists: playable: " + Player1Lifies.Count + "\n opponent lifies: " + Player2Lifies.Count);
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
        Lifies = new List<GameObject>();
        foreach (GameObject Lifie in GameObject.FindGameObjectsWithTag("Lifie"))
        {
            Lifies.Add(Lifie);
            Debug.Log("added: "+Lifie.GetComponent<Lifie>().Name);
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

        List<GameObject> tempList1;
        List<GameObject> tempList2;

        if (playerTurn)
        {
            tempList1 = Player1Lifies;
            tempList2 = Player2Lifies;
        }
        else
        {
            tempList1 = Player2Lifies;
            tempList2 = Player1Lifies;
        }

        foreach (GameObject Lifie in tempList1.ToArray())
        {
            Lifie templifie = Lifie.GetComponent<Lifie>();
            templifie.Enable();
            templifie.AP += 20;
            if (templifie.AP > templifie.TotalAP) templifie.AP = templifie.TotalAP;
        }

        foreach (GameObject Lifie in tempList2.ToArray())
        {
            Lifie.GetComponent<Lifie>().Disable();
        }


        GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText("Next Turn! Select a Lifie.");

    }

    public void CheckForNextTurn()
    {
        ReadLifieLists();
        List<GameObject> tempList;
        if (playerTurn)
        {
            tempList = Player1Lifies;
        } else
        {
            tempList = Player2Lifies;
        }
        foreach (GameObject Lifie in tempList)
        {
            if (Lifie.GetComponent<Lifie>().CanWalk)
            {
                return;
            }
        }

        nextTurn();
    }

    public void KillLifie(GameObject obj)
    {
        Destroy(obj);
        GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText(obj.GetComponent<Lifie>().Name + " fainted!");
        StartCoroutine(CheckEnd());
    }

    public IEnumerator CheckEnd()
    {
        yield return new WaitForSeconds(1);

        ReadLifieLists();
        Debug.Log("checkend opponent lifies count: "+Player2Lifies.Count);
        if(Player2Lifies.Count == 0)
        {
            DeclareEnd(1);
        }
        if (Player1Lifies.Count == 0)
        {
            DeclareEnd(2);
        }

    }

    public void DeclareEnd(int teamThatWon)
    {
        GameObject.Find("Cursor").GetComponent<Cursor>().disabled = true;
        string tempstring;
        if (teamThatWon == 1)
        {
            tempstring = ""+1;
        } else
        {
            tempstring = ""+2;
        }
        VictoryText = "Player "+tempstring + " won!";
        isFading = 2;

    }

    public IEnumerator End()
    {
        Debug.Log("before 10s");
        yield return new WaitForSeconds(10);
        Debug.Log("load scene menu unload battle");
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync("Battle");
    }

}
