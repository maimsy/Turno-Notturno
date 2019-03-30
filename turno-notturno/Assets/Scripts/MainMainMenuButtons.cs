using System;
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

    public Text StartText;
    public Text ContinueText;
    public Text ExitText;

    public GameObject Book;

    private GameManager gameManager;
    private float fadeTime = 2.0f;
    private float fadeTimer = 0;
    private bool isFade = false;
    private bool isBookOpen = false;

    Animator m_Animator;

    private void Start()
    {
        ArrowImage1.SetActive(true);
        ArrowImage2.SetActive(false);
        ArrowImage3.SetActive(false);
        gameManager = GameManager.GetInstance();
        m_Animator = Book.GetComponent<Animator>();

    }


    void Update()
    {
        if (isFade) { FadeTextOut(); }
        if (isBookOpen) { OpenBook(); }

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



    void FadeTextOut()
    {
        fadeTimer += Time.deltaTime;
        float fade = fadeTimer / fadeTime;
        Color color = new Color();
        Color Color1 = StartText.color;
        Color Color2 = ContinueText.color;
        Color Color3 = ExitText.color;
        StartText.color = new Color(Color1.r, Color1.g, Color1.b, 1 - fade);
        ContinueText.color = new Color(Color2.r, Color2.g, Color2.b, 1 - fade);
        ExitText.color = new Color(Color3.r, Color3.g, Color3.b, 1 - fade);

        Image[] images = new Image[3];
        images[0] = ArrowImage1.GetComponent<Image>();
        images[1] = ArrowImage2.GetComponent<Image>();
        images[2] = ArrowImage3.GetComponent<Image>();

        foreach (Image image in images)
        {
            color = image.color;
            image.color = new Color(color.r, color.g, color.b, 1 - fade);
        }

        if (fadeTimer > fadeTime)
        {
            isFade = false;
            Destroy(StartText);
            Destroy(ContinueText);
            Destroy(ExitText);
            isBookOpen = true;
        }
    }


    void OpenBook() {
        Animator anim = Book.GetComponent<Animator>();
        anim.Play("Open");
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        isFade = true;
    }

    // Update is called once per frame
    public void Continue()
    {
        isFade = true;
    }

    public void Options()
    {
        isFade = true;
    }
}
