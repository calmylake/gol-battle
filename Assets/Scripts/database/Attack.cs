using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public int Id;
    public string Name;
    public int Category;
    public int Power;
    public int APdrain;
    public int Element;
    public int RangeCapacity;
    public string Description;

    public Attack(int id, string name, int category, int power, int apdrain, int element, int rangecapacity, string description)
    {
        Id = id;
        Name = name;
        Category = category;
        Power = power;
        APdrain = apdrain;
        Element = element;
        RangeCapacity = rangecapacity;
        Description = description;
    }
}
