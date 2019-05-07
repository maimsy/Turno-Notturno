using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowseNotebook : MonoBehaviour
{
    public Animator animator;
    public Animation animation;
    public GameObject clueCanvas;
    private int _currentPageNumber = 6;
   

    // Update is called once per frame
    void Update() {
       
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            
            if (_currentPageNumber == 6) {
                //animator.speed = -1;
                animator.SetTrigger("TurnToPage5From6");
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
            StartCoroutine(WaitForSomeTime());
            clueCanvas.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        { 
            if (_currentPageNumber == 5)
            { 
                animator.SetTrigger("TurnToPage4From5");
                StartCoroutine(WaitForSomeTime());
                clueCanvas.SetActive(true);
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 4)
            {
                animator.SetTrigger("TurnToPage3fromPage4");
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 3)
            {
                animator.SetTrigger("TurnToPage2From3");
                _currentPageNumber++;
            }
            else if (_currentPageNumber == 2)
            {
                animator.SetTrigger("TurnToPage2From3");
                _currentPageNumber++;
            }

        }
    }

    IEnumerator WaitForSomeTime()
    {
        yield return new WaitForSeconds(1);
    }
}
