using System.Collections;
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
            if (Input.GetKeyUp(KeyCode.Mouse0))
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
        //player won the minigame

        FadeIn fadeIn = GameObject.Find("FadeOut").GetComponent<FadeIn>();
        fadeIn.enabled = true;
        if (win)
        {
            //infoText.GetComponent<Text>().text = "You win!! Left click to continue";
            //infoText.SetActive(true);
            //Invoke("LoadNextScene", 4f);
            //enabled = false; // Avoid ending game multiple times
            fadeIn.changeSceneAfterDone = true;
        }
        //he lost
        else
        {
            //infoText.GetComponent<Text>().text = "You lost.. Left click to restart";
            //infoText.SetActive(true);
            Invoke("Restart", 4f);
            fadeIn.changeSceneAfterDone = false;
        }
        //winning = win;
        //ending = true;
    }
}
