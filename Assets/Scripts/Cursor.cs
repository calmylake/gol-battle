using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    Lifie HoveredLifie;
    GameObject HoveringTile;
    Lifie SelectedLifie;

    Color ColorStandard;
    Color ColorHover;
    Color ColorSelected;

    MetaObject MetaObject;

    void Start()
    {
        MetaObject = GameObject.Find("MetaObject").GetComponent<MetaObject>();

        float R = 255 / 255.0f;
        float G = 196 / 255.0f;
        float B = 0 / 255.0f;
        ColorStandard = new Color(R, G, B);
        ColorHover = new Color(G, R, B);
        ColorSelected = new Color(B, R, G);

    }

    void Update()
    {
        if (SelectedLifie) SetColor(ColorSelected);
        else if (HoveredLifie) SetColor(ColorHover);
        else SetColor(ColorStandard);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { Move("Left"); return; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { Move("Up"); return; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { Move("Right"); return; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Move("Down"); return; }
        
        HoveredLifie = GetHoveringLifie();
        CheckForInfoUI();

        if (Input.GetKeyDown(KeyCode.Space)) { Click(); return; }

    }

    private void ShowHoveringInfo()
    {
        GameObject.Find("InfoUI").transform.position = new Vector3(9f, 2, 3.5f);
        GameObject.Find("HoveringInfoSprite").GetComponent<SpriteRenderer>().sprite = HoveredLifie.GetComponent<SpriteRenderer>().sprite;
        GameObject.Find("HoveringLifieName").GetComponent<Text>().text = HoveredLifie.Name;
        GameObject.Find("HoveringLifieType").GetComponent<Text>().text = HoveredLifie.TypeToString();
        GameObject.Find("HoveringLifieLevel").GetComponent<Text>().text = HoveredLifie.LevelToString();
        GameObject.Find("HoveringLifieLP").GetComponent<Text>().text = HoveredLifie.LPToString();
        GameObject.Find("HoveringLifieAP").GetComponent<Text>().text = HoveredLifie.APToString();
        GameObject.Find("HoveringLifieStatus").GetComponent<Text>().text = HoveredLifie.StatusConditionToString();

    }
    private void HideHoveringInfo()
    {
        GameObject.Find("InfoUI").transform.position = new Vector3(26.5f, 2, 3.5f);
        GameObject.Find("HoveringInfoSprite").GetComponent<SpriteRenderer>().sprite = null;
        GameObject.Find("HoveringInfoCanvas").GetComponentInChildren<Text>().text = "";

    }

    private Lifie GetHoveringLifie()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, new Vector3(0, -0.5f, 0));
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.tag == "Lifie")
            {
                return hit.collider.gameObject.GetComponent<Lifie>();
            }
        }
        return null;
    }

    private void Click()
    {
        if (!SelectedLifie) 
        {
            SelectLifie();
            if(SelectedLifie) SelectedLifie.FindSelectableTiles();
        }
        else
        {
            if (!HoveredLifie || HoveredLifie == SelectedLifie)
            {
                if (SelectedLifie.Move(new Vector3(transform.position.x, SelectedLifie.transform.position.y, transform.position.z)))
                {
                    SelectedLifie = null;
                    MetaObject.ResetTiles();
                }
            }
            
        }

        

    }

    private void SelectLifie()
    {
        if (!HoveredLifie)
        {
            return;
        }
        
        SelectedLifie = HoveredLifie;

        Collider[] hitColliders = Physics.OverlapSphere(SelectedLifie.transform.position, SelectedLifie.GetMovementCapacity()*0.78f);
        foreach(Collider hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.tag == "Tile")
            {
                hitCollider.gameObject.GetComponent<Tile>().selectable = true;
            }
        }

    }

    private void Move(string v)
    {
        Vector3 tempPosition = transform.position;

        switch (v)
        {
            case "Right":
                transform.position += new Vector3(1, 0, 0); break;
            case "Left":
                transform.position += new Vector3(-1, 0, 0); break;
            case "Up":
                transform.position += new Vector3(0, 0, 1); break;
            case "Down":
                transform.position += new Vector3(0, 0, -1); break;
        }

        if (IsCursorOut()) {
            transform.position = tempPosition;
        }

        CheckForInfoUI();

    }

    void CheckForInfoUI()
    {
        if (HoveredLifie) ShowHoveringInfo();
        if (!HoveredLifie) HideHoveringInfo();
    }

    private bool IsCursorOut()
    {
        Vector3 tempPosition = transform.position;

        if (tempPosition.x > 7 || tempPosition.x < 0 || tempPosition.z > 7 || tempPosition.z < 0) return true;
        else return false;

    }

    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
