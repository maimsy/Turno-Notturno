using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowseNotebook : MonoBehaviour
{
    public Animator animator;
    public GameObject clueCanvas;
    public GameObject[] pages;

    [SerializeField]
    private int _currentPageNumber = 6;

    private int _oldCurrentPageNumber = 6;
    private bool _isNewState = false;

    private float timeToWait = 2;

    private void OnEnable()
    {
        EnablePageText("Text08", "haha");
    }

    void Start()
    {
        StartCoroutine(ChangePageText());
    }

    IEnumerator ChangePageText()
    {
        while (true)
        {
            if (_isNewState)
            {
                yield return new WaitForSeconds(timeToWait);

                switch (_currentPageNumber)
                {
                    case 1:
                        clueCanvas.SetActive(false);
                        break;
                    case 2: //1
                        EnablePageText("Text01", "haha");
                        clueCanvas.SetActive(false);
                        break;
                    case 3: //2-3
                        EnablePageText("Text02", "Text03");
                        clueCanvas.SetActive(false);
                        break;
                    case 4: //4-5
                        EnablePageText("Text04", "Text05");
                        clueCanvas.SetActive(false);
                        break;
                    case 5: //6-7
                        EnablePageText("Text06", "Text07");
                        clueCanvas.SetActive(false);
                        break;
                    case 6: //8-9
                        EnablePageText("Text08", "haha");
                        clueCanvas.SetActive(true);
                        break;
                    default:
                        break;
                }

                _isNewState = false;
            }

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            timeToWait = 2;

            FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");
            if (_currentPageNumber == 6)
            {
                animator.SetTrigger("TurnToPage5From6");
                // EnablePageText("Text1-2");
                _currentPageNumber--;
            }
            else if (_currentPageNumber == 5)
            {
                animator.SetTrigger("TurnToPage4From5");
                _currentPageNumber--;
            }
            else if (_currentPageNumber == 4)
            {
                animator.SetTrigger("TurnToPage3fromPage4");
                _currentPageNumber--;
            }
            else if (_currentPageNumber == 3)
            {
                animator.SetTrigger("TurnToPage2From3");
                _currentPageNumber--;
            }

        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            timeToWait = 4;

            FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");
            if (_currentPageNumber == 5)
            {
                animator.SetTrigger("TurnToPage6From5");
                //clueCanvas.SetActive(true);
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 4)
            {
                animator.SetTrigger("TurnToPage5From4");
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 3)
            {
                animator.SetTrigger("TurnToPage4From3");
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 2)
            {
                animator.SetTrigger("TurnToPage3From2");
                _currentPageNumber++;

            }


        }

        if (_oldCurrentPageNumber != _currentPageNumber)
        {
            DisableAllPages();
            clueCanvas.SetActive(false);
            _isNewState = true;
            _oldCurrentPageNumber = _currentPageNumber;
        }


    }



    void EnablePageText(string page1, string page2)
    {

        foreach (GameObject go in pages)
        {
            if (go.name.Contains(page1) || go.name.Contains(page2))
            {
                go.SetActive(true);
            }
            else
                go.SetActive(false);
        }
    }

    void DisableAllPages()
    {
        foreach (GameObject p in pages)
        {
            p.SetActive(false);
        }
    }




    void FadeTextOut()
    {
        //fadeTimer += Time.deltaTime;
        //float fade = fadeTimer / fadeTime;
        //Color color = new Color();
        //Color Color1 = StartText.color;
        //Color Color2 = ContinueText.color;
        //Color Color3 = ExitText.color;
        //StartText.color = new Color(Color1.r, Color1.g, Color1.b, 1 - fade);
        //ContinueText.color = new Color(Color2.r, Color2.g, Color2.b, 1 - fade);
        //ExitText.color = new Color(Color3.r, Color3.g, Color3.b, 1 - fade);

        ////Image[] images = new Image[3];
        ////images[0] = ArrowImage1.GetComponent<Image>();
        ////images[1] = ArrowImage2.GetComponent<Image>();
        ////images[2] = ArrowImage3.GetComponent<Image>();

        ////foreach (Image image in images)
        ////{
        ////    color = image.color;
        ////    image.color = new Color(color.r, color.g, color.b, 1 - fade);
        ////}

        //if (fadeTimer > fadeTime)
        //{
        //    isFade = false;
        //    Destroy(StartText);
        //    Destroy(ContinueText);
        //    Destroy(ExitText);

        //    isBookOpen = true;
        //}
    }
}
