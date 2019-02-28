using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    private string description;
    private bool isDone;
    private Text headLine;
    private Text progress;
    private int progressAmount = 0;
    private int targetAmount = 0;
    private string progressText;

    private Toggle toggle;
    private int counter = 0;
    private int animSpeed = 3;
    private int textIndex = 0;
    private int pos = 0;
    
    private bool animate = false;
    private bool hasStarted = false;
   
    // Start is called before the first frame update
    void Awake()
    {
        headLine = transform.GetChild(0).GetComponent<Text>();
        progress = transform.GetChild(1).GetComponent<Text>();
        toggle = GetComponentInChildren<Toggle>();
        headLine.text = "";
        progress.text = "";


    }

    // Update is called once per frame
    void Update()
    {
        if(animate)
        {
            AnimateText();
        }
        
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
                headLine.text = description.Substring(0, textIndex);
                textIndex++;
            }
            else
            {
                UpdateProgress(0);
                animate = false;
            }
        }
    }

    // Set the UI
    public void SetUp(string descr, string progress, int position, int target)
    {
        pos = position;
        description = descr;
        progressText = progress;
        targetAmount = target;
        Vector2 topRight = GetComponent<RectTransform>().offsetMax;
        Vector2 botLeft = GetComponent<RectTransform>().offsetMin;
        float height = GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().offsetMax = new Vector2(topRight.x, topRight.y - position*height);
        GetComponent<RectTransform>().offsetMin = new Vector2(botLeft.x, botLeft.y - position*height);
        animate = true;
        hasStarted = true;
    }

    public int GetProgress()
    {
        return progressAmount;
    }
    //UpdateProgress
    public bool UpdateProgress(int amount)
    {
        progressAmount += amount;
        progress.text = progressAmount.ToString() + "/" + targetAmount.ToString() + " " + progressText;
        if(progressAmount == targetAmount)
        {
            return true;
        }
        return false;
    }

    // Complete the objective
    public void Complete()
    {
        GetComponentInChildren<Toggle>().isOn = true;
        float p = 0.5f;
        headLine.color = new UnityEngine.Color(p,p,p,p);
        progress.color = new UnityEngine.Color(p, p, p, p);
        //any other animations for completion
        //do we want the completed to move to topmost or is it even possible to have multiple objectives

    }
}
