using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : Inspectable
{
    private bool cluesEnabled = false;

    private PaintingUI ui;
    private List<Interactable> clues;

    private Player player;
    //private List<Interactable> foundClues;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        ui = FindObjectOfType<PaintingUI>();

        clues = new List<Interactable>(GetComponentsInChildren<Interactable>());
        //foundClues = new List<Interactable>();

        player = FindObjectOfType<Player>();
        EnableClues(false); // Enabled in objective manager
    }

    public void EnableClues(bool value)
    {
        cluesEnabled = value;
        /*
        foreach (Interactable clue in clues)
        {

            if(clue && clue.gameObject != this.gameObject) clue.gameObject.SetActive(value);

        }
        */
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (!cluesEnabled) return;
        //if (ui) ui.SetCluesRemainingText("Clues remaining: " + (clues.Count - foundClues.Count));
        if (ui) ui.EnableHintCursor(false);
        if (ui) ui.SetTooltip("");
        if (player) player.SetTooltip("");

        // Check for clues with raycast
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        int mask = (1 << LayerMask.NameToLayer("InspectOnly")); // Ignore everything else except "InspectOnly" 

        if (Physics.Raycast(ray, out hit, 2f, mask)) 
        {
            GameObject objectHit = hit.transform.gameObject;
            Interactable interactable = objectHit.GetComponent<Interactable>();
            if (!interactable) interactable = objectHit.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                
                if (player) player.SetTooltip(interactable.GetTooltip());
                if (interactable.isInteractable)
                {
                    interactable.HighLight();
                    
                    if (player) player.SetTooltip("Press E to " + interactable.GetTooltip());
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.OnInteract();
                    }
                }
            }

        }
    }

    protected override void OnStartInspect()
    {
        if (!cluesEnabled) return;
        /*foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }*/
    }

    protected override void OnStopInspect()
    {
        if (!cluesEnabled) return;
        //if (ui) ui.gameObject.SetActive(false);
        /*foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }*/
        if (ui) ui.HideCursor();
    }
}
