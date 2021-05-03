using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    Vector2 screenSize;
    bool isMoving;
    bool isFading;
    bool fadingIn;
    bool pressStart;
    float pressStartTiming;
    bool isPressStartTimingIncreasing;
    float fadeDelay;
    bool play;

    void Start()
    {
        screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        isMoving = true;
        pressStart = false;
        pressStartTiming = 0;
        isPressStartTimingIncreasing = true;
        play = false;
        isFading = false;
        Debug.Log(screenSize.x);
        //GetComponent<RectTransform>().anchoredPosition += new Vector2(screenSize.x, 0);

        GameObject.Find("WhiteFade").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (pressStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Play();
            }
        }
    }

    void FixedUpdate()
    {
        if (pressStart)
        {
            if (play)
            {
                GameObject.Find("PressStart").GetComponent<Image>().enabled = false;
            }
            else
            {
                if (pressStartTiming >= 100) isPressStartTimingIncreasing = false;
                else if (pressStartTiming <= 0) isPressStartTimingIncreasing = true;
                if (isPressStartTimingIncreasing) pressStartTiming += 100 * Time.deltaTime;
                else pressStartTiming -= 100 * Time.deltaTime;

                Debug.Log(pressStartTiming);
                if (pressStartTiming > 50) GameObject.Find("PressStart").GetComponent<Image>().enabled = true;
                else GameObject.Find("PressStart").GetComponent<Image>().enabled = false;
            }
        }
        if (isMoving)
        {
            Move();
        }
        if (isFading)
        {
            Fade();
        }
    }

    void Play()
    {
        StartFade(1);
        play = true;
    }

    private void Move()
    {
        float speed = 50;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y + speed*Time.deltaTime);
        if (GetComponent<RectTransform>().anchoredPosition.y > 0)
        {
            isMoving = false;
            StartFade(2);
        }
    }

    public void StartFade(float delay)
    {
        fadeDelay = delay;
        isFading = true;
        fadingIn = true;
        GameObject.Find("WhiteFade").GetComponent<Image>().enabled = true;
        Debug.Log("StartFade()");
    }
    public void EndFade()
    {
        isFading = false;
        Debug.Log("EndFade()");
        pressStart = true;
    }

    private void Fade()
    {
        Image imageComponent = GameObject.Find("WhiteFade").GetComponent<Image>();
        
        float valueToChange = Time.deltaTime/fadeDelay;
        if (!fadingIn) valueToChange = -valueToChange;
        else if (!play) valueToChange = 1;

        if (isFading)
        {
            imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, imageComponent.color.a + valueToChange);
        }
        
        if (imageComponent.color.a >= 1)
        {
            fadingIn = false;
            if (play)
            {
                SceneManager.LoadScene("Battle");
                SceneManager.UnloadSceneAsync("Menu");
            }
        }

        if (!fadingIn && imageComponent.color.a <= 0) EndFade();

        Debug.Log("Fade transparency at "+imageComponent.color.a);
    }
}
