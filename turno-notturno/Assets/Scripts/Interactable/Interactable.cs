using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool triggersOnce = false;
    public String tooltip;
    public String interactionDisabledTooltip;
    [SerializeField] List<UnityEvent> interactionEvents;

    public bool isInteractable = true;
    public StudioEventEmitter soundEmitter;

    protected Shader highLightShader;
    protected Shader originalShader;
    protected Renderer renderer;

    private int nextEventIndex = 0;
    protected String originalTooltip;

    private void Awake()
    {
        originalTooltip = tooltip;
    }

    protected virtual void Start()
    {
        if (GetComponent<Renderer>()) renderer = GetComponent<Renderer>();
        if (!renderer) renderer = GetComponentInChildren<Renderer>();
        if (renderer) originalShader = renderer.material.shader;
        highLightShader = Resources.Load<Shader>("HighLight");
    }

    protected virtual void Update()
    {
        if (renderer) renderer.material.shader = originalShader;
    }

    public void SetTooltip(String newTooltip)
    {
        tooltip = newTooltip;
    }

    public void ResetTooltip()
    {
        tooltip = originalTooltip;
    }

    public virtual void OnInteract()
    {
        if (interactionEvents.Count == 0) return;
        if (nextEventIndex + 1 > interactionEvents.Count) nextEventIndex = 0;
        interactionEvents[nextEventIndex++].Invoke();
        if (triggersOnce)
        {
            isInteractable = false;
            //renderer.material.shader = originalShader;
            //Destroy(this);
        }
        if (soundEmitter) soundEmitter.Play();
    }

    public virtual String GetTooltip()
    {
        if (isInteractable) return tooltip;
        else return interactionDisabledTooltip;
    }

    public void HighLight()
    {
        if (renderer) renderer.material.shader = highLightShader;
    }
}
