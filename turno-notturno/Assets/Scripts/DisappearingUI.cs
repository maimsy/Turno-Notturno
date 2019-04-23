using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DisappearingUI : MonoBehaviour
{

    public float lifeTime;

    private Text text;

    private float startTime = 0;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        ResetTimer();
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        gameObject.SetActive(true);
        if (text)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = FadingText(text);
            StartCoroutine(coroutine);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if(startTime + lifeTime < Time.time)
        {
            gameObject.SetActive(false);
        }
    }

    
    IEnumerator FadingText(Text text)
    {
        Color originalColor = text.color;
        originalColor.a = 1;
        Color targetColor = originalColor;
        targetColor.a = 0;
        for (float t = 0.01f; t < lifeTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, targetColor, Mathf.Min(1, t/lifeTime));
            yield return null;
        }
    }
    

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
