using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float letterPause = 0.1f;
    public float initialPause = 0f;
    public float endPause = 1f;

    string message;
    Text textComp;

    // Use this for initialization
    void Start()
    {
        //textComp = GetComponent<Text>();
        //message = textComp.text;
        //textComp.text = "";
        //StartCoroutine(TypeText());
    }

    public void DisplayText(String text)
    {
        StopCoroutine(TypeText()); // Stop any previous texts
        textComp = GetComponent<Text>();
        message = text;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSecondsRealtime(initialPause);
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return new WaitForSecondsRealtime(letterPause);
        }
        yield return new WaitForSecondsRealtime(endPause);
        textComp.text = "";
    }
}
