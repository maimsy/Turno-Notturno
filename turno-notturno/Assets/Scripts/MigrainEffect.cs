using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using FMODUnity;

public class MigrainEffect : MonoBehaviour
{
    public PostProcessProfile Migraint;
    public float speed;
    public bool ismigrain;
    private bool isincreasing;
    public float fadeSpeed;
    private float maximumSound = 0.55f;
    public float EffectStrength;
    private float effectLimit = 0;
    private float soundVolume = 0;
    private PostProcessVolume ppVolume;
    ChromaticAberration chromeaticthing;
    AmbientOcclusion AmbientOcclusionthing;
    Grain grainthing;
    // Start is called before the first frame update
    void Start()
    {
        ppVolume = GetComponent<PostProcessVolume>();
        ismigrain = false;
        isincreasing = false;
        Migraint.TryGetSettings(out chromeaticthing);
        Migraint.TryGetSettings(out grainthing);
        Migraint.TryGetSettings(out AmbientOcclusionthing);
        EndMigrain();
        
    }

    // Update is called once per frame
    void Update()
    {

        

        if (ismigrain)
        {
            // judge the state
            if (chromeaticthing.intensity.value >= effectLimit)
                isincreasing = false;
            if (chromeaticthing.intensity.value <= 0)
                isincreasing = true;
            if (isincreasing)
            {
                chromeaticthing.intensity.value += speed;
                grainthing.intensity.value += speed;
                //AmbientOcclusionthing.intensity.value += speed*4;
            }
            else
            {
                chromeaticthing.intensity.value -= speed;
                grainthing.intensity.value -= speed;
                //AmbientOcclusionthing.intensity.value -= speed*4;
            }



            ppVolume.weight = Mathf.Min(ppVolume.weight + fadeSpeed, 1);
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
           // StartMigrain();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
           // EndMigrain();
        }
    }
    public void StartMigrain() {
        // Camera.main.GetComponent<StudioEventEmitter>().enabled = true;
        Debug.Log("QHAA");
        ismigrain = true;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        GetComponent<PostProcessVolume>().profile = Migraint;
        GetComponent<DizzyEffect1>().enabled = false;
    }
    public void EndMigrain()
    {
        //Camera.main.GetComponent<StudioEventEmitter>().enabled = false;
        ismigrain = false;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        GetComponent<DizzyEffect1>().enabled = true;
        ResetValues();
    }

    public void ResetValues()
    {
        chromeaticthing.intensity.value = 0;
        grainthing.intensity.value = 0;
        AmbientOcclusionthing.intensity.value = 4;
        isincreasing = true;
    }

    public void StartMigrainDelayed(float delay)
    {
        Invoke("StartMigrain", delay);
    }

    public void EndMigrainDelayed(float delay)
    {
        // End effect after delay seconds
        Invoke("EndMigrain", delay);
    }
}
