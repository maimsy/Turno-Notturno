using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTrigger : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;
    private bool played = false;
    [SerializeField] string whisper;
    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/whispers/" + whisper);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !played)
        {
            Debug.Log("HOWOO");
            instance.start();
            played = true;
        }
    }
}
