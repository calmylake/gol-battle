using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifie : MonoBehaviour
{
    //stats
    public int Id = 0;
    public string Name = "";
    public int Element = 1;
    public float Level = 0;
    public float LP = 0;
    public float AP = 0;
    public int StatusCondition = 0;
    public int MovementCapacity = 2;

    public int Strength;
    public int Defense;
    public int Magic;
    public int MagicDefense;

    public int StrengthModifier;
    public int DefenseModifier;
    public int MagicModifier;
    public int MagicDefenseModifier;

    public int StrengthTileModifier;
    public int DefenseTileModifier;
    public int MagicTileModifier;
    public int MagicDefenseTileModifier;


    public int TotalLP = 0;
    public int TotalAP = 0;
    public bool CanWalk;

    //attacks
    List<int> AttackList;

    //movement
    Vector3 Velocity;
    float moveSpeed = 1;

    List<Tile> SelectableTiles;
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    List<string> StatusInString;

    protected void Init()
    {
        switch (name)
        {
            case "LifiePlayer0":
                Id = 1;
                break;
            case "LifiePlayer1":
                Id = 3;
                break;
            case "LifiePlayer2":
                Id = 4;
                break;
            case "LifieCPU0":
                Id = 2;
                break;
            case "LifieCPU1":
                Id = 5;
                break;
            case "LifieCPU2":
                Id = 6;
                break;
        }

        LifieDatabase LifieDB = GameObject.Find("LifieDatabase").GetComponent<LifieDatabase>();
        Name = LifieDB.GetLifie(Id).Name;
        Element = LifieDB.GetLifie(Id).Element;
        Level = 50;
        LP = TotalLP = LifieDB.GetLifie(Id).TotalLP;
        AP = TotalAP = LifieDB.GetLifie(Id).TotalAP;
        Strength = LifieDB.GetLifie(Id).Strength;
        Defense = LifieDB.GetLifie(Id).Defense;
        Magic = LifieDB.GetLifie(Id).Magic;
        MagicDefense = LifieDB.GetLifie(Id).MagicDefense;
        MovementCapacity = LifieDB.GetLifie(Id).MovementCapacity;
        AttackList = LifieDB.GetLifie(Id).Attacks;

        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(""+Id);

        //tiles = GameObject.FindGameObjectsWithTag("Tile");
        Velocity = new Vector3();
        SelectableTiles = new List<Tile>();

        StatusInString = new List<string>();
        StatusInString.AddRange(new List<string>() { "", "Burned", "Frozen", "Paralyzed", "Poisoned", "Asleep", "Confused" });

        //set to false when there is turn manager
        CanWalk = true;
        UpdateTileModifiers();
        StrengthModifier = 0;
        DefenseModifier = 0;
        MagicModifier = 0;
        MagicDefenseModifier = 0;
}
    protected void Loop()
    {
        if (LP <= 0)
        {
            Die();
        }
    }
    
    public void UpdateTileModifiers()
    {
        StrengthTileModifier = 0;
        DefenseTileModifier = 0;
        MagicTileModifier = 0;
        MagicDefenseTileModifier = 0;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            switch (tile.type)
            {
                //water
                case 1:
                    if(!(Element == 1 || Element == 5))
                    {
                        StrengthTileModifier = 1;
                        MagicTileModifier = 1;
                    }
                    break;
                //sand
                case 2:
                    if (!(Element == 2 || Element == 3))
                    {
                        DefenseTileModifier = 1;
                        MagicDefenseTileModifier = 1;
                    }
                    break;
                //grass
                case 3:
                    if (!(Element == 4 || Element == 6))
                    {
                        StrengthTileModifier = 1;
                        DefenseTileModifier = 1;
                    }
                    break;
                //mountain
                case 4:
                    if (!(Element == 7 || Element == 8))
                    {
                        StrengthTileModifier = 1;
                        DefenseTileModifier = 1;
                    }
                    break;
                //storm
                case 5:
                    if (!(Element == 9 || Element == 10))
                    {
                        MagicTileModifier = 1;
                        MagicDefenseTileModifier = 1;
                    }
                    break;
                default:
                    break;
            }
        }
        else return;
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

    public void ComputeAdjacencyLists(int type)
    {
        tiles = GameObject.Find("MetaObject").GetComponent<MetaObject>().getTileList();
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Tile>().FindNeighbors(type);
        }
    }

    public void FindSelectableTiles(int type)
    {
        int Capacity = 0;
        if (type == 0) Capacity = MovementCapacity;
        else if (type == 1) Capacity = 1;
        if (GameObject.Find("MetaObject").GetComponent<MetaObject>().isFirstTime())
        {
            ComputeAdjacencyLists(type);
        }
        ComputeAdjacencyLists(type);
        GetCurrentTile();
        
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            SelectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < Capacity)
            {
                foreach (Tile tile in t.adjacencyList)
                {

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

    public void FindSelectableAttack(int rangeCapacity, bool isStatsAttack)
    {
        if (GameObject.Find("MetaObject").GetComponent<MetaObject>().isFirstTime())
        {
            ComputeAdjacencyLists(1);
        }
        ComputeAdjacencyLists(1);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            SelectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < rangeCapacity)
            {
                foreach (Tile tile in t.adjacencyList)
                {

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

    public string ElementToString()
    {
        return GameObject.Find("ElementDatabase").GetComponent<ElementDatabase>().GetElement(Element).Name;
    }

    public string LevelToString()
    {
        return "Lvl. " + Level;
    }
    public string APToString()
    {
        if (AP <= 1) return "AP: " + (int) Math.Ceiling(AP);
        else return "AP: " + (int) Math.Round(AP, MidpointRounding.AwayFromZero);
    }
    
    public string TotalAPToString()
    {
        return "" + TotalAP;
    }

    public string LPToString()
    {
        if(LP <= 1) return "LP: " + (int) Math.Ceiling(LP);
        else return "LP: " + (int) Math.Round(LP, MidpointRounding.AwayFromZero);
    }

    public string TotalLPToString()
    {
        return ""+TotalLP;
    }

    public List<int> GetAttackList()
    {
        return AttackList;
    }

    public int GetAttack(int pos)
    {
        return AttackList[pos];
    }

    public string StatusConditionToString()
    {
        return StatusInString[StatusCondition];
    }

    public string MovementCapacityToString()
    {
        return "Movement: " + MovementCapacity;
    }
    
    public string StrengthToString()
    {
        return "Strength: " + (int) Math.Round(GetStrength(), MidpointRounding.AwayFromZero);
    }
    public string DefenseToString()
    {
        return "Defense: " + (int)Math.Round(GetDefense(), MidpointRounding.AwayFromZero);
    }
    public string MagicToString()
    {
        return "Magic: " + (int)Math.Round(GetMagic(), MidpointRounding.AwayFromZero);
    }
    public string MagicDefenseToString()
    {
        return "Magic Defense: " + (int)Math.Round(GetMagicDefense(), MidpointRounding.AwayFromZero);
    }

    public float GetStrength()
    {
        return Strength + GetStrengthBonus();
    }
    public float GetDefense()
    {
        return Defense + GetDefenseBonus();
    }
    public float GetMagic()
    {
        return Magic + GetMagicBonus();
    }
    public float GetMagicDefense()
    {
        return MagicDefense + GetMagicDefenseBonus();
    }

    public float GetStrengthBonus()
    {
        float r = 1;
        if(StrengthModifier >= 0) r *= (StrengthModifier+2)/2.0f;
        else r *= 2.0f/(Math.Abs(StrengthModifier)+2);
        if (StrengthTileModifier >= 0) r *= (StrengthTileModifier+2)/2.0f;
        else r *= 2.0f/(Math.Abs(StrengthTileModifier)+2);
        r = Strength * r - Strength;
        return r;
    }
    public float GetDefenseBonus()
    {
        float r = 1;
        if (DefenseModifier >= 0) r *= (DefenseModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(DefenseModifier) + 2);
        if (DefenseTileModifier >= 0) r *= (DefenseTileModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(DefenseTileModifier) + 2);
        r = Defense * r - Defense;
        return r;
    }
    public float GetMagicBonus()
    {
        float r = 1;
        if (MagicModifier >= 0) r *= (MagicModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(MagicModifier) + 2);
        if (MagicTileModifier >= 0) r *= (MagicTileModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(MagicTileModifier) + 2);
        r = Magic * r - Magic;
        return r;
    }
    public float GetMagicDefenseBonus()
    {
        float r = 1;
        if (MagicDefenseModifier >= 0) r *= (MagicDefenseModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(MagicDefenseModifier) + 2);
        if (MagicDefenseTileModifier >= 0) r *= (MagicDefenseTileModifier + 2) / 2.0f;
        else r *= 2.0f / (Math.Abs(MagicDefenseTileModifier) + 2);
        r = MagicDefense * r - MagicDefense;
        return r;
    }

    public bool MoveTo(Tile target)
    {
        if (SelectableTiles.Contains(target))
        {
            transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            SelectableTiles.Clear();
            UpdateTileModifiers();
            return true;
        }
        else return false;
    }

    public void Enable()
    {
        CanWalk = true;
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    public void Disable()
    {
        CanWalk = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        GameObject.Find("MetaObject").GetComponent<MetaObject>().CheckForNextTurn();
    }

    public bool Attack(Lifie targetLifie, Attack attack)
    {
        if (
            (
                attack.isStatusAttack() &&
                (
                    (gameObject.GetComponent<LifiePlayer>() && targetLifie.gameObject.GetComponent<LifieCPU>()) ||
                    (gameObject.GetComponent<LifieCPU>() && targetLifie.gameObject.GetComponent<LifiePlayer>())
                )
            ) ||
            (
                !attack.isStatusAttack() &&
                (
                    (gameObject.GetComponent<LifiePlayer>() && targetLifie.gameObject.GetComponent<LifiePlayer>()) ||
                    (gameObject.GetComponent<LifieCPU>() && targetLifie.gameObject.GetComponent<LifieCPU>())
                )
            )
            )
        {
            return false;
        }
        if (AP - attack.APDrain < 0)
        {
            GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText("This lifie doesn't have enough AP for this attack!");
            return false;
        }

        RaycastHit[] hits;
        hits = Physics.RaycastAll(targetLifie.transform.position, new Vector3(0, -0.5f, 0));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                if (hit.collider.gameObject.GetComponent<Tile>().selectable)
                {
                    if(!attack.isStatusAttack())
                    {
                        float multiplier = ElementMultiplier(attack.Element, targetLifie.Element);
                        float damage = CalculateDamage(attack.Power, attack.Category, targetLifie) * multiplier;
                        targetLifie.LP -= damage;
                        AP -= attack.APDrain;
                        string logtext = targetLifie.Name + " took " + (int)Math.Round(damage, MidpointRounding.AwayFromZero) + " damage from " + Name;
                        if (multiplier > 1) logtext = "Super effective!\n" + logtext + "!";
                        else if (multiplier < 1) logtext = "Not very effective...\n" + logtext + "...";
                        else logtext += ".";

                        GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText(logtext);
                    } else
                    {
                        string temps = "";
                        //stats change
                        switch (attack.Category)
                        {
                            case 3:
                                //Strength
                                targetLifie.StrengthModifier += attack.EffectPower;
                                temps = "strength";
                                break;
                            case 4:
                                //Defense
                                targetLifie.DefenseModifier += attack.EffectPower;
                                temps = "defense";
                                break;
                            default:
                                Debug.Log("Need to setup Lifie.Attack //stats change case > 5");
                                break;
                        }
                        GameObject.Find("LogBox").GetComponent<LogBox>().SetLogBoxText (targetLifie.Name + "'s "+temps+" was increased!");
                    }
                    return true;
                }
            }
        }
        
        return false;
    }

    public float CalculateDamage(int power, int attackCategory, Lifie defenderLifie)
    {
        float Damage;
        float s;
        float d;
        float modifier = 1;

        if (attackCategory == 1)
        {
            s = GetMagic();
            d = defenderLifie.GetMagicDefense();
        }
        else if (attackCategory == 2)
        {
            s = GetStrength();
            d = defenderLifie.GetDefense();
        }
        else
        {
            s = 1;
            d = 1;
        }

        //Damage = ((2 * Level / 5 + 2) * power * (s / d) / 50 + 2) * modifier;
        Damage = (7 + Level / 200 * power * s / d) * modifier;

        return Damage;
    }
    public float ElementMultiplier(int elementAttacker, int elementDefender)
    {
        float multiplier = 1;
        Element defender = GameObject.Find("ElementDatabase").GetComponent<ElementDatabase>().GetElement(elementDefender);

        if (defender.Strengths.Contains(elementAttacker))
            multiplier = 0.5f;
        else if (defender.Weaknesses.Contains(elementAttacker))
            multiplier = 2;

        return multiplier;
    }

    public void Die()
    {
        GameObject.Find("MetaObject").GetComponent<MetaObject>().KillLifie(gameObject);
    }
}
