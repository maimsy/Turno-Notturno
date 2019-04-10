using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using FMODUnity;

public class DizzyEffect1 : MonoBehaviour
{
    public PostProcessProfile Dizzy;
    public float speed;
    public float rotateSpeed;
    public bool isDizzy;
    public bool isincreasing;
    public float fadeSpeed;
    private float maximumSound = 0.55f;
    public float EffectStrength;
    private float effectLimit = 1;
    private float soundVolume = 0;
    private bool rotatingRight = true;
    private float rotator = 0;
    private float rotateLimit = 0;
    private PostProcessVolume ppVolume;
    ChromaticAberration chromeaticthing;
    LensDistortion lensDistortion;
    // Start is called before the first frame update
    void Start()
    {
        ppVolume = GetComponent<PostProcessVolume>();
        isDizzy = false;
        isincreasing = false;
        Dizzy.TryGetSettings(out chromeaticthing);
        Dizzy.TryGetSettings(out lensDistortion);
        EndDizzy();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDizzy)
        {
            // judge the state
            if (chromeaticthing.intensity.value >= effectLimit)
                isincreasing = false;
            if (chromeaticthing.intensity.value <= 0)
                isincreasing = true;
            if (isincreasing)
            {
                chromeaticthing.intensity.value += speed;
                lensDistortion.intensity.value += speed* 30;
                gameObject.transform.Rotate(Vector3.forward, speed * 1);
            }
            else
            {
                chromeaticthing.intensity.value -= speed;
                lensDistortion.intensity.value -= speed*30;
                gameObject.transform.Rotate(Vector3.forward, -speed * 1);
            }
            ppVolume.weight = Mathf.Min(ppVolume.weight + fadeSpeed, 0.9f);
            effectLimit = Mathf.Min(effectLimit + fadeSpeed * EffectStrength, EffectStrength);
            soundVolume = Mathf.Min(soundVolume + fadeSpeed * maximumSound, maximumSound);
            Camera.main.GetComponent<StudioEventEmitter>().EventInstance.setParameterValue("migraineVolume", soundVolume);
        } 
        else
        {
            ppVolume.weight = Mathf.Max(ppVolume.weight - fadeSpeed, 0);
            effectLimit = Mathf.Max(effectLimit - fadeSpeed * EffectStrength, 0);
            soundVolume = Mathf.Max(soundVolume - fadeSpeed * maximumSound, 0);
            Camera.main.GetComponent<StudioEventEmitter>().EventInstance.setParameterValue("migraineVolume", soundVolume);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
             //StartDizzy();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
             //EndDizzy();
        }
    }

    private void LateUpdate()
    {
        if(isDizzy)
        {
            if (rotatingRight)
            {
                gameObject.transform.Rotate(Vector3.forward, rotator * 1);
                rotator += rotateSpeed;
                if (rotator > rotateLimit) rotatingRight = false;
            }
            else
            {
                
                gameObject.transform.Rotate(Vector3.forward, rotator * 1);
                rotator -= rotateSpeed;
                if (rotator < -rotateLimit) rotatingRight = true;
            }
            Debug.Log("rotateLimit " + rotateLimit);
            rotateLimit = Mathf.Min(rotateLimit + rotateSpeed * 0.3f, 4);
        }

    }
    public void StartDizzy() {
        // Camera.main.GetComponent<StudioEventEmitter>().enabled = true;
        isDizzy = true;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        GetComponent<PostProcessVolume>().profile = Dizzy;
        GetComponent<MigrainEffect>().enabled = false;
    }
    public void EndDizzy()
    {
        //Camera.main.GetComponent<StudioEventEmitter>().enabled = false;
        isDizzy = false;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        GetComponent<MigrainEffect>().enabled = true;
        ResetValues();
    }

    public void ResetValues()
    {
        chromeaticthing.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        isincreasing = true;
    }

    public void StartMigrainDelayed(float delay)
    {
        Invoke("StartDizzy", delay);
    }

    public void EndMigrainDelayed(float delay)
    {
        // End effect after delay seconds
        Invoke("DEndDizzy", delay);
    }
}
