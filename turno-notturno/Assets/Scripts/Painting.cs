using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : Inspectable
{
    public bool enableClues = false;

    private PaintingUI ui;
    private List<Clue> clues;
    private List<Clue> foundClues;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        ui = FindObjectOfType<PaintingUI>();

        clues = new List<Clue>(GetComponentsInChildren<Clue>());
        foundClues = new List<Clue>();
        foreach (Transform child in transform)
        {
            //child.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (!enableClues) return;
        if (ui) ui.SetCluesRemainingText("Clues remaining: " + (clues.Count - foundClues.Count));
        if (ui) ui.EnableHintCursor(false);
        if (ui) ui.SetTooltip("");
        

        // Check for clues with raycast
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject objectHit = hit.transform.gameObject;
            Clue clue = objectHit.GetComponent<Clue>();
            if(clue != null)
            {
                if (clues.Contains(clue))
                {
                    if (ui) ui.EnableHintCursor(true);
                    if (ui) ui.SetTooltip("Press E");
                    if (Input.GetButtonDown("Interact"))
                    {
                        foundClues.Add(clue);
                        clue.AnimateDiscovery();
                        if (clues.Count - foundClues.Count == 0)
                        {
                            GameManager manager = FindObjectOfType<GameManager>();
                            if (manager) manager.IncrementDiscoveredClues();
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Clicked on clue that does not belong to this painting!");
                }
            }
        }
    }

    protected override void OnStartInspect()
    {
        if (!enableClues) return;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    protected override void OnStopInspect()
    {
        if (!enableClues) return;
        //if (ui) ui.gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        if (ui) ui.HideCursor();
    }
}
