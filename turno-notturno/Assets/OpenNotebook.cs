using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenNotebook : MonoBehaviour
{
    public Camera MainCamera;
    public Camera NotesCamera;
    public GameObject ClueText;

    public void OpenNotebok()
    {
        MainCamera.enabled =(false);
        NotesCamera.enabled =(true);

        StartCoroutine("ShowText", 4);
    }

    private IEnumerator ShowNoteBookText(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClueText.SetActive(true); 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainCamera.enabled =(true);
            NotesCamera.enabled =(false);
            ClueText.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
