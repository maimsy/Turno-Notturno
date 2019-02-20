using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    private string description;
    private bool isDone;
    private Text text;
    private Toggle toggle;
    private int counter = 0;
    private int animSpeed = 3;
    private int textIndex = 0;
    private int pos = 0;
   
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();
        toggle = GetComponentInChildren<Toggle>();
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        AnimateText();
    }
    
    // Make letters appear one by one
    public void AnimateText()
    {
        counter++;
        if (counter > animSpeed)
        {
            counter = 0;
            if (textIndex <= description.Length)
            {
                text.text = description.Substring(0, textIndex);
                textIndex++;
            }
        }
    }

    // Set the UI
    public void SetUp(string descr, int position)
    {
        pos = position;
        description = descr;
        Vector2 topRight = GetComponent<RectTransform>().offsetMax;
        Vector2 botLeft = GetComponent<RectTransform>().offsetMin;
        float height = GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().offsetMax = new Vector2(topRight.x, topRight.y - position*height);
        GetComponent<RectTransform>().offsetMin = new Vector2(botLeft.x, botLeft.y - position*height);
    }
    
    // Complete the objective
    public void Complete()
    {
        GetComponentInChildren<Toggle>().isOn = true;
        float p = 0.5f;
        GetComponentInChildren<Text>().color = new UnityEngine.Color(p,p,p,p);
        //any other animations for completion
        //do we want the completed to move to topmost or is it even possible to have multiple objectives

    }
}
