using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{

    public GameObject camera;
    public GameObject hintIcon;
    public GameObject cursorIcon;
    public Text cluesRemaining;
    public float inspectDistance;
    public float inspectSpeed;
    public float maxX;
    public float maxY;
    public float maxZoom;

    private bool inspecting = false;
    private float x;
    private float y;

    private float zoom = 1;

    private List<Clue> clues;
    private List<Clue> foundClues;
    
    // Start is called before the first frame update
    void Start()
    {
        clues = new List<Clue>(GetComponentsInChildren<Clue>());
        foundClues = new List<Clue>();
        StartInspect();
    }

    // Update is called once per frame
    void Update()
    {

        if (inspecting)
        {
            HandleInspectMovement();
            
            // Set zoom
            if (Input.GetKey(KeyCode.Mouse0)) zoom = maxZoom;
            else zoom = 1;
            
            // Set UI elements
            if (cluesRemaining)
            {
                cluesRemaining.text = (clues.Count - foundClues.Count).ToString();    
            }
            
            hintIcon.SetActive(false);

            // Check for clues with raycast
            RaycastHit hit;
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (Physics.Raycast(ray, out hit)) 
            {
                GameObject objectHit = hit.transform.gameObject;
                Clue clue = objectHit.GetComponent<Clue>();
                if(clue != null)
                {
                    if (clues.Contains(clue))
                    {
                        hintIcon.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            foundClues.Add(clue);
                            clue.AnimateDiscovery();
                            Debug.Log("Clue found!");
                        }
                        else
                        {
                            //Debug.Log("Yes clue!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Clicked on clue that does not belong to this painting!");
                    }
                }
                else
                {
                    //Debug.Log("No clue");
                }
            }
            else
            {
                //Debug.Log("No hit");
            }
            cursorIcon.SetActive(!hintIcon.active);
        
            
        }
    }

    void StartInspect()
    {
        inspecting = true;
        camera.transform.position = transform.position - transform.forward * inspectDistance;
        x = 0;
        y = 0;
    }

    void StopInspect()
    {
        inspecting = false;
    }

    void HandleInspectMovement()
    {
        float vSpeed = 1;
        float hSpeed = 1;

        float mouseX = -Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
            
        if (mouseX > 0)
        {
            hSpeed = Mathf.Clamp(maxX-x, 0, 1);
        }
        if (mouseX < 0)
        {
            hSpeed = Mathf.Clamp(maxX+x, 0, 1);
        }   
        if (mouseY > 0)
        {
            vSpeed = Mathf.Clamp(maxY-y, 0, 1);
        }
        if (mouseY < 0)
        {
            vSpeed = Mathf.Clamp(maxY+y, 0, 1);
        }
            
        float h = inspectSpeed * hSpeed * mouseX * zoom;
        float v = inspectSpeed * vSpeed * mouseY * zoom;
            
        x = Mathf.Clamp(x+h, -maxX, maxX);
        y = Mathf.Clamp(y+v, -maxY, maxY);

        camera.transform.position = transform.position - transform.forward * inspectDistance * zoom;
        camera.transform.position += transform.right * x;
        camera.transform.position += transform.up * y;
    }
    
    
}
