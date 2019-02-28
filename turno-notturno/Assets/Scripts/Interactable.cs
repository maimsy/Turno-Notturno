using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool triggersOnce = false;
    [SerializeField] String tooltip;
    [SerializeField] List<UnityEvent> interactionEvents;

    private int nextEventIndex = 0;

    public void OnInteract()
    {
        if (interactionEvents.Count == 0) return;
        if (nextEventIndex + 1 > interactionEvents.Count) nextEventIndex = 0;
        interactionEvents[nextEventIndex++].Invoke();
        if(triggersOnce) Destroy(this);
    }

    public String GetTooltip()
    {
        return tooltip;
    }
}
