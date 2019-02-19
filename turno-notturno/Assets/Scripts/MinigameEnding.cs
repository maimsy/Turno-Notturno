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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ending)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                ending = false;
                if (winning)
                {
                    GameManager gameManager = GameManager.GetInstance();
                    gameManager.LoadNextAct();
                    //SceneManager.LoadScene("standard-test");
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void EndGame(bool win)
    {
        //player won the minigame
        if (win)
        {
            infoText.GetComponent<Text>().text = "You win!! Press E to continue";
            infoText.SetActive(true);
        }
        //he lost
        else
        {
            infoText.GetComponent<Text>().text = "You lost.. Press E to restart";
            infoText.SetActive(true);
        }
        winning = win;
        ending = true;
    }
}
