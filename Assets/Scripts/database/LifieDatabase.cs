using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifieDatabase : MonoBehaviour
{
    List<Lifiee> Lifies;
    void Awake()
    {
        BuildElementDatabase();
    }

    public Lifiee GetLifie(int id)
    {
        return Lifies.Find(Lifiee => Lifiee.Id == id);
    }

    private void BuildElementDatabase()
    {
        Lifies = new List<Lifiee>()
        {
            new Lifiee(1, "Dana", 11, 80, 65, 45, 80, 100, 110, 2, new List<int>(){ 1, 2, 3, 4 }),
            new Lifiee(2, "Frealla", 10, 70, 75, 110, 60, 70, 100, 2, new List<int>(){ 5, 6, 7, 8 }),
            new Lifiee(3, "Laaka", 2, 110, 55, 110, 70, 40, 80, 2, new List<int>(){ 9, 10, 7, 11 }),
            new Lifiee(4, "Asjim", 1, 70, 65, 35, 80, 150, 80, 2, new List<int>(){ 12, 13, 7, 14 }),
            new Lifiee(5, "Nyexi-33", 5, 90, 80, 90, 120, 40, 60, 2, new List<int>(){ 15, 16, 17, 18 }),
            new Lifiee(6, "Haiss", 8, 80, 60, 130, 70, 30, 100, 2, new List<int>(){ 7, 19, 20, 21 })
        };
    }
}
