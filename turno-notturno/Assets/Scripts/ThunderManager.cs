using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ThunderManager : MonoBehaviour
{

    public float soundPauseMin = 0.5f;
    public float soundPauseMax = 1.5f;
    private float soundPause = 0;

    public float thunderPauseMin = 3.5f;
    public float thunderPauseMax = 7f;
    private float thunderPause = 0;

    public int maxFlashes = 5;
    public float minFlashDuration = 0.05f;
    public float maxFlashDuration = 0.2f;

    private float soundTimer = 0;
    private float thunderTimer = 0;
    private int previousX = 0;

    public bool stormStarted = true;
    private bool thunder = false;

    private StudioEventEmitter rainL1;
    private StudioEventEmitter rainL2;
    private StudioEventEmitter rainR1;
    private StudioEventEmitter rainR2;

    private StudioEventEmitter thunderL1;
    private StudioEventEmitter thunderL2;
    private StudioEventEmitter thunderR1;
    private StudioEventEmitter thunderR2;
    // Start is called before the first frame update
    void Start()
    {
        soundPause = Random.Range(soundPauseMin, soundPauseMax);
        rainL1 = GameObject.Find("rainL1").GetComponent<StudioEventEmitter>();
        rainL2 = GameObject.Find("rainL2").GetComponent<StudioEventEmitter>();
        rainR1 = GameObject.Find("rainR1").GetComponent<StudioEventEmitter>();
        rainR2 = GameObject.Find("rainR2").GetComponent<StudioEventEmitter>();
        thunderL1 = GameObject.Find("thunderL1").GetComponent<StudioEventEmitter>();
        thunderL2 = GameObject.Find("thunderL2").GetComponent<StudioEventEmitter>();
        thunderR1 = GameObject.Find("thunderR1").GetComponent<StudioEventEmitter>();
        thunderR2 = GameObject.Find("thunderR2").GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stormStarted)
        {
            thunderTimer += Time.deltaTime;
            if (thunderTimer > thunderPause)
            {
                thunder = true;
                thunderPause = Random.Range(thunderPauseMin, thunderPauseMax);
                thunderTimer = 0;
                StartCoroutine("ThunderLight");
            }
        }
        if(thunder)
        {
            soundTimer += Time.deltaTime;
            if (soundTimer > soundPause)
            {
                ThunderSound();
                soundPause = Random.Range(soundPauseMin, soundPauseMax);
                soundTimer = 0;
                thunder = false;
            }
        }
    }

    public void StartStorm()
    {
        stormStarted = true;
        rainL1.Play();
        rainL2.Play();
        rainR1.Play();
        rainR2.Play();
    }

    IEnumerator ThunderLight()
    {
        int flashTimes = Random.Range(1, maxFlashes);
        for (int i = 0; i < flashTimes; i++)
        {
            GetComponent<Light>().enabled = true;
            yield return new WaitForSeconds(Random.Range(minFlashDuration, maxFlashDuration));
            GetComponent<Light>().enabled = false;
            yield return new WaitForSeconds(Random.Range(minFlashDuration, maxFlashDuration));
        }
        
    }

    private void ThunderLightOff()
    {
        
    }

    private void ThunderSound()
    {
        
        int x = Random.Range(1, 7);
        while (x == previousX)
        {
            x = Random.Range(1, 7);
        }
        previousX = x;
        thunderL1.Event = "event:/thunder/thunder" + x + "L1";
        thunderL1.Play();
        thunderL2.Event = "event:/thunder/thunder" + x + "L2";
        thunderL2.Play();
        thunderR1.Event = "event:/thunder/thunder" + x + "R1";
        thunderR1.Play();
        thunderR2.Event = "event:/thunder/thunder" + x + "R2";
        thunderR2.Play();
    }
}
