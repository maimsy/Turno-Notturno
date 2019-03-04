using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    private string description;
    private bool isDone;
    private Text text;
    private int progressAmount = 0;
    private int targetAmount = 0;

    private Toggle toggle;
    private float timer = 0;
    private float animSpeed = 0.05f;
    private int textIndex = 0;
    private int pos = 0;
    private float fadeTime = 2.0f;
    private float fadeTimer = 0;
    
    private bool animate = false;
    private bool hasStarted = false;
    private bool completed = false;
   
    // Start is called before the first frame update
    void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        toggle = GetComponentInChildren<Toggle>();
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(animate)
        {
            AnimateText();
        }
        if (completed)
        {
            Fade();
        }
    }
    
    // Make letters appear one by one
    public void AnimateText()
    {
        timer += Time.deltaTime;
        if (timer > animSpeed)
        {
            timer = 0;
            if (textIndex <= description.Length)
            {
                text.text = description.Substring(0, textIndex);
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
    public void SetUp(string descr, int position, int target)
    {
        pos = position;
        description = descr;
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
        if(targetAmount > 1)
        {
            text.text = description +" " + progressAmount.ToString() + "/" + targetAmount.ToString();
        }
        else
        {
            text.text = description;
        }
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
        //headLine.color = new UnityEngine.Color(p,p,p,p);
        //progress.color = new UnityEngine.Color(p, p, p, p);
        completed = true;
        //any other animations for completion

    }

    //Fade the UI away after completing the objective
    private void Fade()
    {
        fadeTimer += Time.deltaTime;
        float fade = fadeTimer/fadeTime;
        Debug.Log("Fade " + fade);
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, 1 - fade);
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            color = image.color;
            image.color = new Color(color.r, color.g, color.b, 1 - fade);
        }
        if(fadeTimer > fadeTime)
        {
            Destroy(gameObject);
        }
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }
}
