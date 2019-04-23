using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLights : MonoBehaviour
{
    public string lightTag = "Act1";

    private GameObject[] prevObjects;

    private bool areEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            areEnabled = !areEnabled;
            SetLights(lightTag, areEnabled);
            Debug.Log(areEnabled);
        }   
    }

    void SetLights(string tag, bool value)
    {
        if (!value)
        {
            prevObjects = GameObject.FindGameObjectsWithTag(tag);  // This can only find enabled objects
        }
        foreach (GameObject obj in prevObjects)
        {
            obj.SetActive(value);
        }
    }
}
