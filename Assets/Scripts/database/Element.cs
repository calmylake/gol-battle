using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{
    public int Id;
    public string Name;
    public List<int> Strengths;
    public List<int> Weaknesses;

    public Element(int id, string name, List<int> strengths, List<int> weaknesses)
    {
        Id = id;
        Name = name;
        Strengths = strengths;
        Weaknesses = weaknesses;
    }
}
