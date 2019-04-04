using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool rotationEnabled;
    public bool useArtwork1Sound;
    public float maxSpeed;

    public float speedChangeRate = 50f;

    private float speed = 0;
    private StudioEventEmitter soundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        soundEmitter = GetComponent<StudioEventEmitter>();
        if (rotationEnabled) StartRotation();  // Ensure sound works correctly
        else StopRotation();
    }

    public void StartRotation()
    {
        rotationEnabled = true;
        if (useArtwork1Sound && soundEmitter)
        {
            soundEmitter.SetParameter("artRotateStartStop", 1);
        }
    }

    public void StopRotation()
    {
        rotationEnabled = false;
        if (useArtwork1Sound && soundEmitter)
        {
            soundEmitter.SetParameter("artRotateStartStop", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationEnabled)
        {
            if (speed < maxSpeed) speed += speedChangeRate * Time.deltaTime;
            speed = Mathf.Min(speed, maxSpeed);
        }
        else
        {
            if (speed > 0) speed -= speedChangeRate * Time.deltaTime;
            speed = Mathf.Max(speed, 0);
        }

        transform.Rotate(Vector3.up * speed* Time.deltaTime);
    }
}
