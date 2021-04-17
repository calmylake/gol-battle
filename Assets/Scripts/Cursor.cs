using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public bool SelectingTarget;
    Lifie SelectedLifie;
    Attack SelectedAttack;
    Vector3 CancelActionBackup;

    Color ColorStandard;
    Color ColorHover;
    Color ColorSelected;

    //UI
    bool isActionUIOpen;
    bool isAttackUIOpen;
    Lifie HoveringLifie;
    Tile HoveringTile;

    float UITransitionDelay;
    Vector3 InfoUIShownPosition; Vector3 InfoUIHiddenPosition;
    Vector3 ActionUIShownPosition; Vector3 ActionUIHiddenPosition;
    Vector3 AttackUIShownPosition; Vector3 AttackUIHiddenPosition;

    void Start()
    {
        float R = 255 / 255.0f;
        float G = 196 / 255.0f;
        float B = 0 / 255.0f;
        ColorStandard = new Color(R, G, B);
        ColorHover = new Color(G, R, B);
        ColorSelected = new Color(B, R, G);
        UITransitionDelay = 6;

        InfoUIShownPosition = new Vector3(9f, 2, 3.5f);
        InfoUIHiddenPosition = GameObject.Find("InfoUI").transform.position;

        ActionUIShownPosition = new Vector3(13.75f, 2, 5.5f);
        ActionUIHiddenPosition = GameObject.Find("ActionUI").transform.position;

        AttackUIShownPosition = new Vector3(13.75f, 2, 5.5f);
        AttackUIHiddenPosition = GameObject.Find("AttackUI").transform.position;

        Reset();
        HideActionUI();
        HideAttackUI();
    }

    void Update()
    {
        if (SelectedLifie) SetColor(ColorSelected);
        else if (HoveringLifie) SetColor(ColorHover);
        else SetColor(ColorStandard);

        if (!isActionUIOpen && !isAttackUIOpen)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { Move("Left"); return; }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { Move("Up"); return; }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { Move("Right"); return; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { Move("Down"); return; }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { MoveOtherCursor("Up"); return; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { MoveOtherCursor("Down"); return; }
        }

        if (Input.GetKeyDown(KeyCode.Space)) { Click(); return; }

        if (Input.GetKeyDown(KeyCode.Escape)) { if (SelectedLifie) SelectedLifie.transform.position = CancelActionBackup; Reset(); return; }

        HoveringLifie = GetHoveringLifie();
        HoveringTile = GetHoveringTile();
        CheckForInfoUI();

    }

    void CheckForInfoUI()
    {
        if (HoveringLifie) ShowHoveringInfo();
        if (!HoveringLifie || isActionUIOpen || isAttackUIOpen) HideHoveringInfo();

    }

    private void ShowHoveringInfo()
    {
        //GameObject.Find("InfoUI").transform.position = new Vector3(9f, 2, 3.5f);
        GameObject.Find("InfoUI").GetComponent<UI>().AskMovement(InfoUIShownPosition, UITransitionDelay);
        GameObject.Find("HoveringInfoSprite").GetComponent<SpriteRenderer>().sprite = HoveringLifie.GetComponent<SpriteRenderer>().sprite;
        GameObject.Find("HoveringLifieName").GetComponent<Text>().text = HoveringLifie.Name;
        GameObject.Find("HoveringLifieElement").GetComponent<Text>().text = HoveringLifie.TypeToString();
        GameObject.Find("HoveringLifieLevel").GetComponent<Text>().text = HoveringLifie.LevelToString();
        GameObject.Find("HoveringLifieLP").GetComponent<Text>().text = HoveringLifie.LPToString() + " / " + HoveringLifie.TotalLPToString();
        GameObject.Find("HoveringLifieAP").GetComponent<Text>().text = HoveringLifie.APToString() + " / " + HoveringLifie.TotalAPToString();
        GameObject.Find("HoveringLifieStatus").GetComponent<Text>().text = HoveringLifie.StatusConditionToString();
        GameObject.Find("HoveringLifieMovementCapacity").GetComponent<Text>().text = HoveringLifie.MovementCapacityToString();
        GameObject.Find("HoveringLifieStrength").GetComponent<Text>().text = HoveringLifie.StrengthToString();
        GameObject.Find("HoveringLifieDefense").GetComponent<Text>().text = HoveringLifie.DefenseToString();
        GameObject.Find("HoveringLifieMagic").GetComponent<Text>().text = HoveringLifie.MagicToString();
        GameObject.Find("HoveringLifieMagicDefense").GetComponent<Text>().text = HoveringLifie.MagicDefenseToString();

    }
    private void HideHoveringInfo()
    {
        //GameObject.Find("InfoUI").transform.position = new Vector3(26.5f, 2, 3.5f);
        GameObject.Find("InfoUI").GetComponent<UI>().AskMovement(InfoUIHiddenPosition, UITransitionDelay);
        GameObject.Find("HoveringInfoSprite").GetComponent<SpriteRenderer>().sprite = null;
        GameObject.Find("HoveringInfoCanvas").GetComponentInChildren<Text>().text = "";

    }
    
    private void ShowActionUI()
    {
        isActionUIOpen = true;
        //GameObject.Find("ActionUI").transform.position = new Vector3(13.75f, 2, 5.5f);
        GameObject.Find("ActionUI").GetComponent<UI>().AskMovement(ActionUIShownPosition, UITransitionDelay);

        GameObject.Find("ActionCursor").transform.localPosition = new Vector3(0, 1.9f, 0.05f);

    }
    private void HideActionUI()
    {
        isActionUIOpen = false;
        //GameObject.Find("ActionUI").transform.position = new Vector3(13.75f, 2, -10);
        GameObject.Find("ActionUI").GetComponent<UI>().AskMovement(ActionUIHiddenPosition, UITransitionDelay);
        GameObject.Find("ActionCursor").transform.localPosition = new Vector3(0, 1.9f, 0.05f);

    }

    private void ShowAttackUI()
    {
        isAttackUIOpen = true;
        //GameObject.Find("AttackUI").transform.position = new Vector3(13.75f, 2, 5.5f);
        GameObject.Find("AttackUI").GetComponent<UI>().AskMovement(AttackUIShownPosition, UITransitionDelay);
        GameObject.Find("AttackCursor").transform.localPosition = new Vector3(0, 1.9f, 0.05f);
    }

    private void HideAttackUI()
    {
        isAttackUIOpen = false;
        //GameObject.Find("AttackUI").transform.position = new Vector3(9.75f, 2, -10);
        GameObject.Find("AttackUI").GetComponent<UI>().AskMovement(AttackUIHiddenPosition, UITransitionDelay);
        GameObject.Find("AttackCursor").transform.localPosition = new Vector3(0, 1.9f, 0.05f);
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

    private Tile GetHoveringTile()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, new Vector3(0, -0.5f, 0));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                return hit.collider.gameObject.GetComponent<Tile>();
            }
        }
        return null;
    }

    private void Click()
    {
        if (!isActionUIOpen && !isAttackUIOpen)
        {
            if (!SelectedLifie)
            {
                SelectLifie();
                if (SelectedLifie)
                {
                    SelectedLifie.FindSelectableTiles(0);
                    int i = 0;
                    foreach (Transform child in GameObject.Find("AttackButtons").transform)
                    {
                        child.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = GameObject.Find("AttackDatabase").GetComponent<AttackDatabase>().GetAttack(SelectedLifie.GetAttack(i)).Name;
                        i++;
                    }
                }
            } else if (SelectingTarget)
            {
                if (HoveringLifie && HoveringLifie != SelectedLifie)
                {
                    if (SelectedLifie.Attack(HoveringLifie, SelectedAttack))
                    {
                        SelectedLifie.Disable();
                        Reset();
                    }
                }
            }
            else
            {
                if (SelectedLifie.MoveTo(HoveringTile))
                {
                    //SelectedLifie = null;
                    GameObject.Find("MetaObject").GetComponent<MetaObject>().ResetTiles();
                    ShowActionUI();
                }
            }
            
        } else
        {
            if (isActionUIOpen)
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(GameObject.Find("ActionCursor").transform.position, new Vector3(0, -1, 0));
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.name == "AttackButton")
                    {
                        AttackAction();
                        break;
                    }
                    if (hit.collider.gameObject.name == "WaitButton")
                    {
                        WaitAction();
                        break;
                    }
                    if (hit.collider.gameObject.name == "CancelButton")
                    {
                        SelectedLifie.transform.position = CancelActionBackup;
                        Reset();
                        break;
                    }

                }
            } else if (isAttackUIOpen) {
                //attack
                int tempAttack = 1;
                RaycastHit[] hits;
                hits = Physics.RaycastAll(GameObject.Find("AttackCursor").transform.position, new Vector3(0, -1, 0));
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.name == "Attack0")
                    {
                        tempAttack = 0;
                        break;
                    }
                    if (hit.collider.gameObject.name == "Attack1")
                    {
                        tempAttack = 1;
                        break;
                    }
                    if (hit.collider.gameObject.name == "Attack2")
                    {
                        tempAttack = 2;
                        break;
                    }
                    if (hit.collider.gameObject.name == "Attack3")
                    {
                        tempAttack = 3;
                        break;
                    }

                }
                SelectedAttack = GameObject.Find("AttackDatabase").GetComponent<AttackDatabase>().GetAttack(SelectedLifie.GetAttack(tempAttack));
                Debug.Log("Selected attack: "+SelectedAttack.Name);
                HideAttackUI();
                SelectedLifie.FindSelectableAttack(SelectedAttack.RangeCapacity);
                SelectingTarget = true;
            }
            
        }

    }

    private void AttackAction()
    {
        HideActionUI();
        ShowAttackUI();
    }

    private void WaitAction()
    {
        SelectedLifie.Disable();
        Reset();
    }

    private void CancelAction()
    {
        //SelectedLifie.transform.position = CancelActionBackup;
        //SelectedLifie.FindSelectableTiles(0);
        Reset();
        HideActionUI();
    }

    private void SelectLifie()
    {
        if (!HoveringLifie) return;
        if (!HoveringLifie.CanWalk) return;
        
        SelectedLifie = HoveringLifie;
        CancelActionBackup = new Vector3(SelectedLifie.transform.position.x, SelectedLifie.transform.position.y, SelectedLifie.transform.position.z);
    }

    private void Move(string dir)
    {
        Vector3 tempPosition = transform.position;

        switch (dir)
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


    }
    private void MoveOtherCursor(string dir)
    {
        int numberOfButtons;
        Vector3 tempPosition = new Vector3();
        if (isActionUIOpen)
        {
            numberOfButtons = GameObject.Find("ActionButtons").transform.childCount;
            if (numberOfButtons < 2) return;
            tempPosition = GameObject.Find("ActionCursor").GetComponent<Transform>().localPosition;
        }
        else if (isAttackUIOpen)
        {
            numberOfButtons = GameObject.Find("AttackButtons").transform.childCount;
            if (numberOfButtons < 2) return;
            tempPosition = GameObject.Find("AttackCursor").GetComponent<Transform>().localPosition;
        }
        else return;

        switch (dir)
        {
            case "Up":
                tempPosition += new Vector3(0, 1.01f, 0); break;
            case "Down":
                tempPosition += new Vector3(0, -1.01f, 0); break;
        }

        if (isActionUIOpen)
        {
            if (!isActionCursorOut(tempPosition, numberOfButtons))
            {
                GameObject.Find("ActionCursor").GetComponent<Transform>().localPosition = tempPosition;
            }
        }
        else if (isAttackUIOpen)
        {
            if (!isActionCursorOut(tempPosition, numberOfButtons))
            {
                GameObject.Find("AttackCursor").GetComponent<Transform>().localPosition = tempPosition;
            }
        }

    }

    private bool IsCursorOut()
    {
        Vector3 tempPosition = transform.position;

        if (tempPosition.x > 7 || tempPosition.x < 0 || tempPosition.z > 7 || tempPosition.z < 0) return true;
        else return false;

    }

    private bool isActionCursorOut(Vector3 v, int n)
    {
        if (v.y > 2 || v.y < 1.2f-n*1.01f+1) return true;
        else return false;
    }

    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Reset()
    {
        SelectingTarget = false;
        SelectedLifie = null;
        SelectedAttack = null;
        CancelActionBackup = new Vector3();
        GameObject.Find("MetaObject").GetComponent<MetaObject>().ResetTiles();
        HideActionUI();
        HideAttackUI();
    }
}
