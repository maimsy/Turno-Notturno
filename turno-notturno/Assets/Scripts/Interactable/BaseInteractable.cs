using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteractable : MonoBehaviour
{
    protected Shader highLightShader;
    protected Shader originalShader;
    protected Renderer highlightRenderer;

    protected virtual void Awake()
    {
        GetRenderer();
        GetHighlightShader();
    }

    protected virtual void Update()
    {
        if (highlightRenderer) highlightRenderer.material.shader = originalShader;
    }

    protected virtual void GetRenderer()
    {
        if (GetComponent<Renderer>()) highlightRenderer = GetComponent<Renderer>();
        if (!highlightRenderer) highlightRenderer = GetComponentInChildren<Renderer>();
    }

    protected virtual void GetHighlightShader()
    {
        if (highlightRenderer) originalShader = highlightRenderer.material.shader;
        highLightShader = Resources.Load<Shader>("HighLight");
    }

    public abstract void Interact();

    public abstract String GetTooltip();

    public virtual void HighLight()
    {
        if (highlightRenderer) highlightRenderer.material.shader = highLightShader;
    }
}
