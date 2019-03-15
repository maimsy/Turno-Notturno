using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool triggersOnce = false;
    public String tooltip;
    [SerializeField] List<UnityEvent> interactionEvents;

    public bool isInteractable = true;

    protected Shader highLightShader;
    protected Shader originalShader;
    protected Renderer renderer;

    private int nextEventIndex = 0;
    protected String originalTooltip;

    private void Awake()
    {
        originalTooltip = tooltip;
    }

    private void Start()
    {
        if (GetComponent<Renderer>()) renderer = GetComponent<Renderer>();
        if (!renderer) renderer = transform.GetChild(0).GetComponent<Renderer>();
        originalShader = renderer.material.shader;
        highLightShader = Resources.Load<Shader>("HighLight");
    }

    private void Update()
    {
        renderer.material.shader = originalShader;
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
            renderer.material.shader = originalShader;
            Destroy(this);
        }
    }

    public virtual String GetTooltip()
    {
        return tooltip;
    }

    public void HighLight()
    {
        renderer.material.shader = highLightShader;
    }
}
