using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Dream_effect : MonoBehaviour
{
    public PostProcessProfile Dream;
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
        if (!Dream)
        {
            Debug.LogError("Dream_effect is missing PostProcessProfile");
            enabled = false;  // Disable this script
            return;
        }
        Dream.TryGetSettings(out chromeaticthing);
        Dream.TryGetSettings(out grainthing);
        Dream.TryGetSettings(out AmbientOcclusionthing);
        if (!chromeaticthing)
        {
            Debug.LogError("Dream_effect is missing ChromaticAberration");
            enabled = false;  // Disable this script
            return;
        }

        if (!grainthing)
        {
            Debug.LogError("Dream_effect is missing Grain");
            enabled = false;  // Disable this script
            return;
        }

        if (!AmbientOcclusionthing)
        {
            Debug.LogError("Dream_effect is missing AmbientOcclusion");
            enabled = false;  // Disable this script
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // judge the state
            if (chromeaticthing.intensity.value >= EffectStrength)
                isincreasing = false;
            if (chromeaticthing.intensity.value <= 0)
                isincreasing = true;

            if (isincreasing)
            {
                chromeaticthing.intensity.value += speed;
                grainthing.intensity.value += speed;
                AmbientOcclusionthing.intensity.value += speed * 4;
            }
            else
            {
                chromeaticthing.intensity.value -= speed;
                grainthing.intensity.value -= speed;
                AmbientOcclusionthing.intensity.value += speed;
            }

    }
    public void StartMigrain()
    {
        ismigrain = true;
    }
    public void EndMigrain()
    {
        ismigrain = false;
        ResetValue();
    }
    public void ResetValue()
    {
        isincreasing = false;
        chromeaticthing.intensity.value = 0;
    }
}
