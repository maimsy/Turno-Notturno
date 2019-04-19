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

    private StudioEventEmitter[] thunderEmittersL1;
    private StudioEventEmitter[] thunderEmittersL2;
    private StudioEventEmitter[] thunderEmittersR1;
    private StudioEventEmitter[] thunderEmittersR2;
    // Start is called before the first frame update
    void Start()
    {
        soundPause = Random.Range(soundPauseMin, soundPauseMax);
        rainL1 = GameObject.Find("rainL1").GetComponent<StudioEventEmitter>();
        rainL2 = GameObject.Find("rainL2").GetComponent<StudioEventEmitter>();
        rainR1 = GameObject.Find("rainR1").GetComponent<StudioEventEmitter>();
        rainR2 = GameObject.Find("rainR2").GetComponent<StudioEventEmitter>();
        thunderEmittersL1 = GameObject.Find("thunderL1").GetComponents<StudioEventEmitter>();
        thunderEmittersL2 = GameObject.Find("thunderL2").GetComponents<StudioEventEmitter>();
        thunderEmittersR1 = GameObject.Find("thunderR1").GetComponents<StudioEventEmitter>();
        thunderEmittersR2 = GameObject.Find("thunderR2").GetComponents<StudioEventEmitter>();
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
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(minFlashDuration, maxFlashDuration));
            transform.GetChild(0).gameObject.SetActive(false);
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
        foreach(StudioEventEmitter emitter in thunderEmittersL1)
        {
            if(emitter.Event == "event:/thunder/thunder" + x + "L1")
            {
                emitter.Play();
                break;
            }
        }
        foreach (StudioEventEmitter emitter in thunderEmittersL2)
        {
            if (emitter.Event == "event:/thunder/thunder" + x + "R1")
            {
                emitter.Play();
                break;
            }
        }
        foreach (StudioEventEmitter emitter in thunderEmittersR1)
        {
            if (emitter.Event == "event:/thunder/thunder" + x + "L2")
            {
                emitter.Play();
                break;
            }
        }
        foreach (StudioEventEmitter emitter in thunderEmittersR2)
        {
            if (emitter.Event == "event:/thunder/thunder" + x + "R2")
            {
                emitter.Play();
                break;
            }
        }
    }
}
