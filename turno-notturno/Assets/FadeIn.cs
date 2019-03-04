using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool fadeIn;

    private int counter;

    private void Start()
    {
        Color color = GetComponent<Image>().color;
        int var = fadeIn == true ? 1 : 0;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, var);
    }

    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<Image>().color;
        float alpha = fadeIn == true ? 1 - counter * speed : counter * speed;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
        counter++;

    }
}
