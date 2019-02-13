using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    
    [SerializeField] float inspectDistance;
    [SerializeField] float inspectSpeed;
    [SerializeField] float maxX;
    [SerializeField] float maxY;
    [SerializeField] float maxZoom;

    private bool inspecting;
    private float x;
    private float y;

    private float zoom = 1;

    private Vector3 prevCameraPosition;
    private Quaternion prevCameraRotation;
    private List<Clue> clues;
    private List<Clue> foundClues;
    private GameObject camera;

    private PaintingUI ui;

    private GameManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<PaintingUI>();
        manager = FindObjectOfType<GameManager>();
        if (!manager)
        {
            Debug.LogError("GameManager is missing from the scene!");
        }
        clues = new List<Clue>(GetComponentsInChildren<Clue>());
        foundClues = new List<Clue>();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        //StartInspect();
    }

    // Update is called once per frame
    void Update()
    {

        if (inspecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopInspect();
                return;
            }

            HandleInspectMovement();
            
            // Set zoom
            if (Input.GetKey(KeyCode.Mouse0)) zoom = maxZoom;
            else zoom = 1;
            
            if (ui) ui.SetCluesRemainingText("Clues remaining: " + (clues.Count - foundClues.Count).ToString());
            if (ui) ui.EnableHintCursor(false);
            

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
                        if (ui) ui.EnableHintCursor(true);
                        if (Input.GetButtonDown("Interact"))
                        {
                            foundClues.Add(clue);
                            clue.AnimateDiscovery();
                            if (clues.Count - foundClues.Count == 0)
                            {
                                GameManager manager = FindObjectOfType<GameManager>();
                                if (manager) manager.IncrementDiscoveredClues();
                            }
                            //Debug.Log("Clue found!");
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
 
        }
    }

    

    public void StartInspect()
    {
        //if (ui) ui.gameObject.SetActive(true);
        
        // Enable clue colliders and info text
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        // Take control of camera
        camera = Camera.main.gameObject;
        inspecting = true;
        manager.DisableControls();
        prevCameraPosition = camera.transform.position;
        prevCameraRotation = camera.transform.rotation;
        camera.transform.position = transform.position - transform.forward * inspectDistance;
        camera.transform.LookAt(transform.position);
        x = 0;
        y = 0;
    }

    void StopInspect()
    {
        //if (ui) ui.gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        inspecting = false;
        camera.transform.position = prevCameraPosition;
        camera.transform.rotation = prevCameraRotation;
        if (ui) ui.HideCursor();
        manager.EnableControls();
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
