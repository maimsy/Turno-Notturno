using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseUI : MonoBehaviour
{
    [SerializeField] float pulseSpeed;
    [SerializeField] float maxScale;

    private Vector2 originalScale;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = GetComponent<RectTransform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float increment = counter * pulseSpeed;
        GetComponent<RectTransform>().localScale = new Vector3(originalScale.x + increment, originalScale.y + increment, 1);
        if (increment > maxScale-1)
        {
            counter = 0;
        }
        counter++;
    }
}
