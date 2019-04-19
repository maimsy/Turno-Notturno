using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenNotebook : MonoBehaviour
{
    public GameObject Canvas;

    private void OnEnable()
    {
        //StartCoroutine("ShowText", 4);
    }

    private IEnumerator ShowNoteBookText(float delay)
    {
        yield return new WaitForSeconds(delay);
        Canvas.SetActive(true);
    }

    private void OnDisable()
    {
        Canvas.SetActive(false);
    }

}
