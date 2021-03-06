﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float fadeTime;
    [SerializeField] bool fadeIn;
    public bool changeSceneAfterDone;

    private float timeBlack = 0;
    private float timer;

    private void Start()
    {
        Color color = GetComponent<Image>().color;
        int var = fadeIn == true ? 1 : 0;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, var);
    }

    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<Image>().color;
        float fade = timer / fadeTime;
        //float slow = fade < 0.5f ? 0.5f : 1;
        float alpha = fadeIn == true ? 1 -  fade: fade;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
        timer += Time.deltaTime;
        if(timer > fadeTime)
        {
            if (changeSceneAfterDone)
            {
                GameManager gameManager = GameManager.GetInstance();
                gameManager.LoadNextAct();
            }
            else
            {
                if(timer-fadeTime > timeBlack)
                {
                    GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
                    GameObject.Find("FadeIn").GetComponent<Animator>().Play("FadeIn", -1, 0f);
                    GameObject.Find("FadeIn").GetComponent<Animator>().speed = 4;
                    this.enabled = false;
                }
            }
            
        }

    }

    public void SetTimeBlack(float time)
    {
        timeBlack = time;
    }

    public void Reset()
    {
        timer = 0;
    }
}
