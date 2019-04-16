using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ThunderManager : MonoBehaviour
{

    private float soundPauseMin = 0.5f;
    private float soundPauseMax = 3f;
    private float soundPause = 0;

    private float thunderPauseMin = 3.5f;
    private float thunderPauseMax = 7f;
    private float thunderPause = 0;

    private float soundTimer = 0;
    private float thunderTimer = 0;
    private int previousX = 0;

    private bool stormStarted = true;
    private bool thunder = false;
    // Start is called before the first frame update
    void Start()
    {
        soundPause = Random.Range(soundPauseMin, soundPauseMax);
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainL1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainR1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainL2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainR2");
    }

    private void ThunderSound()
    {
        
        int x = Random.Range(1, 7);
        while (x == previousX)
        {
            x = Random.Range(1, 7);
        }
        previousX = x;

        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "L1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "R1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "L2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "R2");
    }
}
