﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameEnding : MonoBehaviour
{
    public GameObject infoText;

    private bool ending = false;
    private bool winning = false;

    void Start()
    {
        GameManager gameManager = GameManager.GetInstance(); // Make sure game manager is loaded so pause menu works
    }

    // Update is called once per frame
    void Update()
    {
        if(ending)
        {
            ending = false;
            if (winning)
            {
                LoadNextScene();
            }
            else
            {
                Restart();
            }
        }
    }

    private void LoadNextScene()
    {
        GameManager gameManager = GameManager.GetInstance();
        gameManager.LoadNextAct();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame(bool win)
    {
        EnvironmentMove env = FindObjectOfType<EnvironmentMove>();
        //player won the minigame
        if (win)
        {
            if(env == null || (env && FindObjectOfType<EnvironmentMove>().GetState() != EnvironmentMove.MinigameState.behind))
            {
                FadeIn fadeIn = GameObject.Find("FadeOut").GetComponent<FadeIn>();
                fadeIn.enabled = true;
                fadeIn.changeSceneAfterDone = true;
            }
            //infoText.GetComponent<Text>().text = "You win!! Left click to continue";
            //infoText.SetActive(true);
            //Invoke("LoadNextScene", 4f);
            //enabled = false; // Avoid ending game multiple times
            
        }
        //he lost
        //winning = win;
        //ending = true;
    }
}
