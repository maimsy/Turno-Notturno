using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MigrainEffect : MonoBehaviour
{
    public PostProcessProfile Migraint;
    public float speed;
    public bool ismigrain;
    private bool isincreasing;
    public float EffectStrength;
    ChromaticAberration chromeaticthing;
    AmbientOcclusion AmbientOcclusionthing;
    Grain grainthing;
    // Start is called before the first frame update
    void Start()
    {
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
       
        // judge the state
        if (ismigrain)
        {
            if (chromeaticthing.intensity.value >= EffectStrength)
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
                //AmbientOcclusionthing.intensity.value += speed;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //StartMigrain();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //EndMigrain();
        }
    }
    public void StartMigrain() {
        ismigrain = true;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        if (ppVolume) ppVolume.enabled = true;
    }
    public void EndMigrain()
    {
        ismigrain = false;
        PostProcessVolume ppVolume = GetComponent<PostProcessVolume>();
        if (ppVolume) ppVolume.enabled = false;
        ResetValue();
    }
    public void ResetValue()
    { 
        isincreasing = false;
        chromeaticthing.intensity.value = 0;
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
