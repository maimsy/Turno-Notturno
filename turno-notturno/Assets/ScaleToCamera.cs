using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    public Camera targetCamera;
    public float distance = 100f;
    void Awake()
    {
        Vector3 screen2World = targetCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));
        screen2World = targetCamera.transform.InverseTransformPoint(screen2World);
        Vector3 newScale = transform.localScale;
        newScale.x *= screen2World.x * 2;
        newScale.z *= screen2World.y * 2;
        transform.localScale = newScale;
        print(screen2World);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
