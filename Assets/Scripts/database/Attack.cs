using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public int Id;
    public string Name;
    public int Category;
    public int Power;
    public int APDrain;
    public int Element;
    public int RangeCapacity;
    public string Description;
    public float EffectPower;

    public Attack(int id, string name, int category, int power, int apdrain, int element, int rangecapacity, string description, float effectpower = 0)
    {
        Id = id;
        Name = name;
        Category = category;
        Power = power;
        APDrain = apdrain;
        Element = element;
        RangeCapacity = rangecapacity;
        Description = description;
        EffectPower = effectpower;
    }

    public bool isStatusAttack()
    {
        if (Category == 1 || Category == 2) return false;
        else return true;
    }
}
