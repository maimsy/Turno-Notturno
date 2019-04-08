using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public string description;
    private bool isDone;
    private Text text;
    private int progressAmount = 0;
    private int targetAmount = 0;

    private Toggle toggle;
    private float timer = 0;
    private float animSpeed = 0.05f;
    private int textIndex = 0;
    private float pos = 0;
    private float fadeTime = 2.0f;
    private float fadeTimer = 0;
    private float moveSpeed = 0.03f;
    private float moveDelay = 2f;
    private float moveTimer = 0;
    private int targetPos = 0;

    private bool animate = false;
    private bool hasStarted = false;
    private bool completed = false;
    private bool moving = false;

    private Vector2 topRight;
    private Vector2 botLeft;

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
        if(moving)
        {
            Move();
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
        targetPos = position;
        moveTimer = moveDelay;
        topRight = GetComponent<RectTransform>().offsetMax;
        botLeft = GetComponent<RectTransform>().offsetMin;
        description = descr;
        targetAmount = target;
        Move();
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

    //Move up if there is space
    public void StartMoving(int amount)
    {
        if(pos > amount)
        {
            targetPos = amount;
            moving = true;
        }
    }
    private void Move()
    {
        moveTimer += Time.deltaTime;
        if(moveTimer > moveDelay)
        {
            pos = Mathf.Max(targetPos, pos - moveSpeed);
            if (pos <= targetPos)
            {
                moving = false;
                moveTimer = 0;
            }
            float height = GetComponent<RectTransform>().rect.height;
            GetComponent<RectTransform>().offsetMax = new Vector2(topRight.x, topRight.y - pos * height);
            GetComponent<RectTransform>().offsetMin = new Vector2(botLeft.x, botLeft.y - pos * height);
        }
    }
    public float GetPos()
    {
        return pos;
    }
}
