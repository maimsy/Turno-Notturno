using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMainMenuButtons : MonoBehaviour
{
    public GameObject ArrowImage1;
    public GameObject ArrowImage2;
    public GameObject ArrowImage3;

    public Button StartButton;
    public Button ContinueButton;
    public Button OptionsButton;


    private void Start()
    {
        ArrowImage1.SetActive(true);
        ArrowImage2.SetActive(false);
        ArrowImage3.SetActive(false); 
    }




    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == StartButton.gameObject)
        {
            ArrowImage1.SetActive(true);
            ArrowImage2.SetActive(false);
            ArrowImage3.SetActive(false);
        }
        else if (selected == ContinueButton.gameObject)
        {
            ArrowImage1.SetActive(false);
            ArrowImage2.SetActive(true);
            ArrowImage3.SetActive(false);
        }
        else if (selected == OptionsButton.gameObject)
        {
            ArrowImage1.SetActive(false);
            ArrowImage2.SetActive(false);
            ArrowImage3.SetActive(true);
        }
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("teoBridgeGame");
    }

    // Update is called once per frame
    public void Continue()
    {

    }

    public void Options()
    {
    }
}
