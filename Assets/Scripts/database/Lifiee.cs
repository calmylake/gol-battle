using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifiee
{
    public int Id;
    public string Name;
    public int Element;
    public int TotalLP;
    public int TotalAP;
    public int Strength;
    public int Defense;
    public int Magic;
    public int MagicDefense;
    public int MovementCapacity;
    public List<int> Attacks;

    public Lifiee(int id, string name, int element, int totalLP, int totalAP, int strength, int defense, int magic, int magicDefense, int movementCapacity, List<int> attacks)
    {
        Id = id;
        Name = name;
        Element = element;
        TotalLP = totalLP;
        TotalAP = totalAP;
        Strength = strength;
        Defense = defense;
        Magic = magic;
        MagicDefense = magicDefense;
        MovementCapacity = movementCapacity;
        Attacks = attacks;
    }
}
