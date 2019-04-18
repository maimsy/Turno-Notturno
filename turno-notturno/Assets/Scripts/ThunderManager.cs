using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ThunderManager : MonoBehaviour
{

    public float soundPauseMin = 0.5f;
    public float soundPauseMax = 3f;
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

    public GameObject light;
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainL1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainR1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainL2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/rainR2");
    }

    IEnumerator ThunderLight()
    {
        int flashTimes = Random.Range(1, maxFlashes);
        for (int i = 0; i < flashTimes; i++)
        {
            light.SetActive(true);
            yield return new WaitForSeconds(Random.Range(minFlashDuration, maxFlashDuration));
            light.SetActive(false);
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

        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "L1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "R1");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "L2");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder/thunder" + x + "R2");
    }
}
