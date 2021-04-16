using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDatabase : MonoBehaviour
{
    List<Element> Elements;
    void Awake()
    {
        BuildElementDatabase();
    }

    public Element GetElement(int id)
    {
        return Elements.Find(Element => Element.Id == id);
    }

    private void BuildElementDatabase()
    {
        Elements = new List<Element>() 
        {
            new Element(1, "Fire", new List<int>() { 3, 9, 11 },       new List<int>() { 2, 4 }),
            new Element(2, "Water", new List<int>() { 1, 4 },          new List<int>() { 3, 6 }),
            new Element(3, "Plant", new List<int>() { 2, 5 },          new List<int>() { 1, 9, 11 }),
            new Element(4, "Ground", new List<int>() { 6, 9, 10 },     new List<int>() { 2, 8, 11 }),
            new Element(5, "Machine", new List<int>() { 7, 8, 10 },    new List<int>() { 3, 6, 11 }),
            new Element(6, "Electric", new List<int>() { 1, 5 },       new List<int>() { 4, 7 }),
            new Element(7, "Spellcaster", new List<int>() { 4, 8, 11}, new List<int>() { 5, 9 }),
            new Element(8, "Beast", new List<int>() { 2, 6 },          new List<int>() { 5, 7, 11 }),
            new Element(9, "Air", new List<int>() { 3, 7 },            new List<int>() { 1, 8 }),
            new Element(10, "Light", new List<int>() { 6, 7, 9, 11 },  new List<int>() { 3 }),
            new Element(11, "Dark", new List<int>() { },               new List<int>() { 1, 5, 6, 10})
        };
    }
}
