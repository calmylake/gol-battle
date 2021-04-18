using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogBox : MonoBehaviour
{
    GameObject LogBoxText;
    void Start()
    {
        LogBoxText = GameObject.Find("LogBoxText");
        LogBoxText.GetComponent<Text>().text = "";
    }

    void Update()
    {
        
    }

    public void SetLogBoxText(string txt)
    {
        if (!LogBoxText)
        {
            return;
        }
        LogBoxText.GetComponent<Text>().text = txt + "\n" + LogBoxText.GetComponent<Text>().text;
    }
}
