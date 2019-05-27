using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMainMenuButtons : MonoBehaviour
{
    //public GameObject ArrowImage1;
    //public GameObject ArrowImage2;
    //public GameObject ArrowImage3;

    public GameObject StartButton;
    public GameObject ContinueButton;
    public GameObject OptionsButton;
    
    public Text StartText;
    public Text ContinueText;
    public Text ExitText;

    public GameObject Book;

    public Text IntroText; 
    public Text TurnPageText;

    private GameManager gameManager;
    private float fadeTime = 2.0f;
    private float fadeTimer = 0;
    private bool isFade = false;
    private bool isBookOpen = false;
    private bool isZoomIn = false;

    Animator m_Animator;

    private void Start()
    {
        //ArrowImage1.SetActive(true);
        //ArrowImage2.SetActive(false);
        //ArrowImage3.SetActive(false);

        StartText.text = "Start";
        ContinueText.text = "C̶o̶n̶t̶i̶n̶u̶e̶";
        ExitText.text = "E̶x̶i̶t̶";


        gameManager = GameManager.GetInstance();
        m_Animator = Book.GetComponent<Animator>();
        IntroText.gameObject.SetActive(false);

    }


    void Update()
    {
        if (this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("TurnToPage1"))
        {
            //ShowIntrotext();
            IntroText.gameObject.SetActive(true);
            //ShowIntrotext();
        }


        if (isFade) { FadeTextOut(); }
        if (isBookOpen) { OpenBook(); }
        if (isZoomIn) { InitiateMiniGameTransition(); }
        if (IntroText.GetComponent<TextAnimation>().isAnimating) { FadeTextIn(TurnPageText);  }

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == StartButton)
        {
            StartText.text = "Start";
            ContinueText.text = "C̶o̶n̶t̶i̶n̶u̶e̶";
            ExitText.text = "E̶x̶i̶t̶";
        }
        else if (selected == ContinueButton)
        {
            StartText.text = "S̶t̶a̶r̶t̶";
            ContinueText.text = "Continue";
            ExitText.text = "E̶x̶i̶t̶";
        }
        else if (selected == OptionsButton)
        {
            StartText.text = "S̶t̶a̶r̶t̶";
            ContinueText.text = "C̶o̶n̶t̶i̶n̶u̶e̶";
            ExitText.text = "Exit";
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) {
            
        FMODUnity.RuntimeManager.PlayOneShot("event:/menuClick");
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

        //Image[] images = new Image[3];
        //images[0] = ArrowImage1.GetComponent<Image>();
        //images[1] = ArrowImage2.GetComponent<Image>();
        //images[2] = ArrowImage3.GetComponent<Image>();

        //foreach (Image image in images)
        //{
        //    color = image.color;
        //    image.color = new Color(color.r, color.g, color.b, 1 - fade);
        //}

        if (fadeTimer > fadeTime)
        {
            isFade = false;
            Destroy(StartText);
            Destroy(ContinueText);
            Destroy(ExitText);
            
            isBookOpen = true;
        }
    }


    void FadeTextIn(Text text)
    {
        fadeTimer += Time.deltaTime;
        float fade = fadeTimer / 10f;                 
        Color Color1 = StartText.color; 
        text.color = new Color(Color1.r, Color1.g, Color1.b, 0.1f + fade);
        if (fadeTimer > fadeTime)
        {
            //isFade = false;
        } 
    }
    public Animation anim;

    void OpenBook() { 
        m_Animator.SetTrigger("isTurnPage1");

        FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");
        isBookOpen = false;


    }

    // Start is called before the first frame update
    public void StartGame()
    {
        isFade = true;
        PlayerPrefs.DeleteKey("ClueFoundAct11");
        PlayerPrefs.DeleteKey("ClueFoundAct12");
        PlayerPrefs.DeleteKey("ClueFoundAct13");
        PlayerPrefs.DeleteKey("ClueFoundAct14");

        PlayerPrefs.DeleteKey("ClueFoundAct21");
        PlayerPrefs.DeleteKey("ClueFoundAct22");
        PlayerPrefs.DeleteKey("ClueFoundAct23");
        PlayerPrefs.DeleteKey("ClueFoundAct24");

        PlayerPrefs.DeleteKey("ClueFoundAct31");
        PlayerPrefs.DeleteKey("ClueFoundAct32");
        PlayerPrefs.DeleteKey("ClueFoundAct33");
        PlayerPrefs.DeleteKey("ClueFoundAct34");

        PlayerPrefs.DeleteKey("ClueFoundAct41");
        PlayerPrefs.DeleteKey("ClueFoundAct42");
        PlayerPrefs.DeleteKey("ClueFoundAct43");
        PlayerPrefs.DeleteKey("ClueFoundAct44");
        PlayerPrefs.DeleteKey("ClueFoundAct51");
        PlayerPrefs.DeleteKey("ClueFoundAct52");
        PlayerPrefs.DeleteKey("ClueFoundAct53");
        PlayerPrefs.DeleteKey("ClueFoundAct54");


        PlayerPrefs.DeleteKey("ClueFoundAct61");
        PlayerPrefs.DeleteKey("ClueFoundAct62");
        PlayerPrefs.DeleteKey("ClueFoundAct63");
        PlayerPrefs.DeleteKey("ClueFoundAct64");

       
    }

    void ShowIntrotext() {

        //yield return new WaitForSeconds(1f);

            IntroText.gameObject.SetActive(true); 
        
    }

     
    // Update is called once per frame
    public void Continue()
    { 
        isFade = true;
        GameObject.FindObjectOfType<GameManager>().LoadGame();
    } 

    public void Quit() { 
        Application.Quit();
    }

    public void OnClickTurnPage() {
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");
        IntroText.gameObject.SetActive(false);
        TurnPageText.gameObject.SetActive(false);
        m_Animator.SetTrigger("isTurnPage2");
        fadeTimer = 0;
        isZoomIn = true;
        GameObject.Find("FadeOut").GetComponent<FadeIn>().enabled = true;
        PlayerPrefs.SetInt("GameState", -1);
    }

    public void InitiateMiniGameTransition() {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("TurnPage2"))
        {

            fadeTimer += Time.deltaTime;
            float fade = fadeTimer / 10.0f; 
            Color Color1 = TurnPageText.color; 
            TurnPageText.color = new Color(Color1.r, Color1.g, Color1.b, 1 - fadeTimer);
            Color Color2 = IntroText.color;
            IntroText.color = new Color(Color2.r, Color2.g, Color2.b, 1 - fadeTimer);



            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 0.1f;
        } 
    }

    
}
