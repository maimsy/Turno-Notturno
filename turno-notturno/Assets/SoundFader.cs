using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundFader : MonoBehaviour
{
    private bool fading = false;
    private float fadeTime = 10;
    private float fadeCounter = 0f;
    private string paramName = "panicSoundVol";
    private StudioEventEmitter sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fading)
        {
            fadeCounter += Time.deltaTime;
            sound.SetParameter(paramName, 1f - fadeCounter / fadeTime); 
            if(fadeCounter / fadeTime > 1)
            {
                Destroy(this);
            }
        }
    }

    public void FadeAway(float time, string parameterName)
    {
        paramName = parameterName;
        fading = true;
        fadeTime = time;
    }
        
}
