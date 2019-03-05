using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource EffectSource;
    public AudioSource MusicScore;

    public float lowPictchRange;
    public float highPitchRage;

    public static SoundManager Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void Play(AudioClip clip)
    {
        EffectSource.clip = clip;
        EffectSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicScore.clip = clip;
        MusicScore.Play();
    }


    int iter = 0;

    public void RandomSoundEffect(params AudioClip[] clips)
    {
        //float randomPitch = Random.Range(lowPictchRange, highPitchRage);

        if (iter > (clips.Length-1)) { iter = 0; } 
        int randomIndx = iter;
        iter++;

        //EffectSource.pitch = randomPitch;*/
        EffectSource.clip = clips[randomIndx];
        EffectSource.PlayOneShot(EffectSource.clip);

    }
}





