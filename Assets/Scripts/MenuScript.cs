using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MenuScript
{
    [MenuItem("Tools/Assign Tile Material")]
    public static void AssingTitleMaterial() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");
        foreach (GameObject t in tiles)
        {
            t.GetComponent<Renderer>().material = material;
        }

    }

    [MenuItem("Tools/Assign Tile Script")]
    public static void AssingTitleScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in tiles)
        {
            t.AddComponent<Tile>();
        }

    }
}
