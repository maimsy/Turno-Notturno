using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_blink : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 1f;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > blinkSpeed)
        {
            //Debug.Log(GetComponent<Light>().enabled);
            timer = 0;
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
        }
    }
}
