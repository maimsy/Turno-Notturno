using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headbobber : MonoBehaviour
{
    public AudioClip[] WalkNoises;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float midpoint = 2.0f;
    private float waveslice,horizontal, vertical, translateChange, totalAxes;
    
    private float timer = 0.0f;

    private void Start()
    {
        InvokeRepeating("PlaySound", 0.0f, 0.5f);
    }
    void Update()
    {
        waveslice = 0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            translateChange = waveslice * bobbingAmount;
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
