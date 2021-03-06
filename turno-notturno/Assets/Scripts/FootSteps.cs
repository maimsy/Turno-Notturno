﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FootSteps : MonoBehaviour
{
    StudioEventEmitter sound;
    public bool soundPlaying;
    [SerializeField] float pauseLength = 1.2f;
    [SerializeField] float pauseDecrement = 0.07f;
    private float counter = 0;
    private Transform target;
    [SerializeField] float moveSpeed = 0.04f;
    [SerializeField] float stopDistance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("FootStepsTarget").transform;
        sound = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(soundPlaying)
        {
            counter += Time.deltaTime;
            if(counter >= pauseLength)
            {
                counter = 0;
                sound.Play();
                pauseLength -= pauseDecrement;
                if(pauseLength < 0.4f)
                {
                    StopSound();
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
            if(Vector3.Distance(transform.position,target.position) < stopDistance)
            {
                StopSound();
            }
        }
        else
        {
            counter = 0;
        }
    }
    public void StartSound()
    {
        soundPlaying = true;
    }

    public void StopSound()
    {
        soundPlaying = false;
    }
}
