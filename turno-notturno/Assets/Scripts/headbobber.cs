using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headbobber : MonoBehaviour
{
    public AudioClip[] WalkNoises;
    public float stepsPerMinute = 100;
    public float runningStepsPerMinute = 170;
    public float bobbingAmount = 0.04f;
    public float runningBobbingAmount = 0.05f;
    public float midpoint = 2.0f;
    private float waveslice,horizontal, vertical, translateChange, totalAxes;
    
    private float timer = 0.0f;
    private Player player;

    private bool stepSoundPlayed;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        //InvokeRepeating("PlaySound", 0.0f, 0.5f);
    }
    void Update()
    {
        if (!player || !player.enabled) return; // Disable bobbing on pause menu and while inspecting paintings
        waveslice = 0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
            stepSoundPlayed = false;
        }
        else
        {
            // Advance bobbing timer
            float spm = stepsPerMinute;
            if (player.IsRunning()) spm = runningStepsPerMinute;
            waveslice = Mathf.Sin(timer);
            timer = timer + spm / 60 * Time.deltaTime * (Mathf.PI * 2);
            
            // Play sounds
            if (timer > 4.2f && !stepSoundPlayed)  // Sin(4.2) = ~-0.9
            {
                stepSoundPlayed = true;
                PlaySound();
            }
            
            // Reset timer
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
                stepSoundPlayed = false;
            }
        }

        

        // Move camera
        if (waveslice != 0)
        {
            if (player.IsRunning())
            {
                translateChange = waveslice * runningBobbingAmount;
            }
            else
            {
                translateChange = waveslice * bobbingAmount;
            }
            totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            transform.localPosition =  new Vector3(0, midpoint+translateChange, 0); 
        }
        else
        {
            transform.localPosition =new Vector3(0, midpoint, 0);
        }
    }
    void PlaySound()
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            SoundManager.Instance.RandomSoundEffect(WalkNoises);
        }
    }
}
