using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : Inspectable
{

    private Player player;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
    }

    protected override void OnUpdate()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        int mask = (1 << LayerMask.NameToLayer("InspectOnly") | 1 << LayerMask.NameToLayer("InspectAndNormal")); // Ignore everything else except "InspectOnly" 
        BaseInteractable.StopPreviousHighlight();
        if (Physics.Raycast(ray, out var hit, 5f, mask))
        {
            GameObject objectHit = hit.transform.gameObject;
            Clue interactable = GetInteractable(objectHit);
            if (interactable != null)
            {

                if (player) player.SetTooltip(interactable.GetTooltip());
                interactable.HighLight();
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
                {
                    interactable.Interact();
                }
            }

        }
    }

    private Clue GetInteractable(GameObject obj)
    {
        Clue[] interactables = obj.GetComponentsInParent<Clue>();
        foreach (Clue interactable in interactables)
        {
            // Prefer interactables that are enabled
            if (interactable.IsInteractable()) return interactable;
        }
        // Otherwise return the first interactable found
        if (interactables.Length > 0) return interactables[0];
        return null;
    }
}
