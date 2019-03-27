using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;

    public float speedChangeTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSpeed(float value)
    {
        StartCoroutine(SmoothSpeedChange(value));
    }

    // Update is called once per frame
    void Update()
    {
       
            // Rotate the object around its local X axis at 1 degree per second
            transform.Rotate(Vector3.up * speed* Time.deltaTime);

            // ...also rotate around the World's Y axis
           // transform.Rotate(Vector3.up * speed* Time.deltaTime, Space.World);
    }

    IEnumerator SmoothSpeedChange(float target)
    {
        float direction = target - speed;
        float stepsize = direction / speedChangeTime;
        while (speed != target)
        {
            speed += stepsize * Time.deltaTime;
            if (direction > 0 && speed > target) speed = target;
            if (direction < 0 && speed < target) speed = target;
            yield return null;
        }
    }
}
