using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcity_rotate : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
            // Rotate the object around its local X axis at 1 degree per second
            transform.Rotate(Vector3.up * speed* Time.deltaTime);

            // ...also rotate around the World's Y axis
           // transform.Rotate(Vector3.up * speed* Time.deltaTime, Space.World);
    }
}
