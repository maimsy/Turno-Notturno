﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool triggersOnce = false;
    [SerializeField] String tooltip;
    [SerializeField] List<UnityEvent> interactionEvents;

    public bool isInteractable = true;

    protected Shader HighLightShader;

    private int nextEventIndex = 0;

    public virtual void OnInteract()
    {
        if (interactionEvents.Count == 0) return;
        if (nextEventIndex + 1 > interactionEvents.Count) nextEventIndex = 0;
        interactionEvents[nextEventIndex++].Invoke();
        if(triggersOnce) Destroy(this);
    }

    public virtual String GetTooltip()
    {
        return tooltip;
    }

    public void HighLight()
    {

    }
}
