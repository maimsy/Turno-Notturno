﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    [SerializeField] GameObject hintIcon;
    [SerializeField] GameObject cursorIcon;
    [SerializeField] Text cluesRemaining;
    [SerializeField] float inspectDistance;
    [SerializeField] float inspectSpeed;
    [SerializeField] float maxX;
    [SerializeField] float maxY;
    [SerializeField] float maxZoom;

    private bool inspecting = false;
    private float x;
    private float y;

    private float zoom = 1;

    private Player player;
    private Vector3 prevCameraPosition = new Vector3();
    private Quaternion prevCameraRotation = new Quaternion();
    private List<Clue> clues;
    private List<Clue> foundClues;
    private GameObject camera;
    
    // Start is called before the first frame update
    void Start()
    {
        clues = new List<Clue>(GetComponentsInChildren<Clue>());
        foundClues = new List<Clue>();
        player = FindObjectOfType<Player>();
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
            
            // Set UI elements
            if (cluesRemaining)
            {
                cluesRemaining.text = (clues.Count - foundClues.Count).ToString();    
            }

            EnableHintCursor(false);

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
                        EnableHintCursor(true);
                        if (Input.GetButtonDown("Interact"))
                        {
                            foundClues.Add(clue);
                            clue.AnimateDiscovery();
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

    void EnableHintCursor(bool value)
    {
        if (hintIcon) hintIcon.SetActive(value);
        if (cursorIcon) cursorIcon.SetActive(!value);
    }

    public void StartInspect()
    {
        // Enable clue colliders and info text
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        // Take control of camera
        camera = Camera.main.gameObject;
        inspecting = true;
        if (player) player.enabled = false;
        prevCameraPosition = camera.transform.position;
        prevCameraRotation = camera.transform.rotation;
        camera.transform.position = transform.position - transform.forward * inspectDistance;
        camera.transform.LookAt(transform.position);
        x = 0;
        y = 0;
    }

    void StopInspect()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        inspecting = false;
        if (player) player.enabled = true;
        camera.transform.position = prevCameraPosition;
        camera.transform.rotation = prevCameraRotation;
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
