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
                    GameManager gameManager = GameManager.GetInstance();
                    gameManager.LoadNextAct();
                    //SceneManager.LoadScene(sceneToLoadAfter);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager gameManager = GameManager.GetInstance();
            gameManager.LoadNextAct();
        }
    }

    public void EndGame(bool win)
    {
        //player won the minigame
        if (win)
        {
            infoText.GetComponent<Text>().text = "You win!! Left click to continue";
            infoText.SetActive(true);
        }
        //he lost
        else
        {
            infoText.GetComponent<Text>().text = "You lost.. Left click to restart";
            infoText.SetActive(true);
        }
        GameObject.Find("FadeOut").GetComponent<FadeIn>().enabled = true;
        winning = win;
        ending = true;
    }
}
