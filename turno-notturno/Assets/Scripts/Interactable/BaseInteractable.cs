using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteractable : MonoBehaviour
{
    public static BaseInteractable previouslyHighlighted;
    public bool highlightChildren = true;
    public bool highlightParents = false;
    public bool overrideDefaultHighlight = false;
    public Material overrideHighlightMaterial;

    protected List<HighlightRenderer> highlightRenderers;
    protected static Material outlineMaterial;


    

    protected virtual void Awake()
    {
        GetRenderer();
        if (!outlineMaterial)
        {
            //highLightShader = Resources.Load<Shader>("HighLight");
            //outlineMaterial = new Material(highLightShader);
            //outlineMaterial.SetFloat("_Outline", 0.002f);
        }
        
    }

    protected virtual void Update()
    {

    }


    protected virtual void GetRenderer()
    {
        List<Renderer> renderers = new List<Renderer>();
        if (highlightChildren)
        {
            renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        if (highlightParents)
        {
            renderers.AddRange(GetComponentsInParent<Renderer>());
        }
        if (!highlightChildren && !highlightParents)
        {
            // Children and parents also includes this gameobject (If both flags are used, this gameobject might be included twice!)
            renderers.Add(GetComponent<Renderer>());
        }

        highlightRenderers = new List<HighlightRenderer>();
        foreach (Renderer rend in renderers)
        {
            if (overrideDefaultHighlight)
            {
                // Use custom highlight for very special cases
                highlightRenderers.Add(new HighlightRenderer(rend, overrideHighlightMaterial));
            }
            else
            {
                // Use hardcoded default highlight (These are expected to be used for almost all of the Interactables)
                highlightRenderers.Add(new HighlightRenderer(rend, null));
            }
        }
    }

    


    public abstract void Interact();

    public abstract String GetTooltip();

    public abstract bool IsInteractable();

    

    public virtual void HighLight()
    {
        StopPreviousHighlight();
        previouslyHighlighted = this;
        foreach (HighlightRenderer highlightRenderer in highlightRenderers)
        {
            highlightRenderer.ToggleHighlight(true);
        }
    }

    public virtual void StopHighlight()
    {
        foreach (HighlightRenderer highlightRenderer in highlightRenderers)
        {
            highlightRenderer.ToggleHighlight(false);
        }
    }

    public static void StopPreviousHighlight()
    {
        if (previouslyHighlighted) previouslyHighlighted.StopHighlight();
    }

    public class HighlightRenderer
    {
        public HighlightRenderer(Renderer renderer, Material overrideMaterial)
        {
            this.renderer = renderer;
            originalMaterials = new List<Material>();
            highlightMaterials = new List<Material>();
            foreach (Material material in renderer.materials)
            {
                originalMaterials.Add(new Material(material));
                if (overrideMaterial) highlightMaterials.Add(overrideMaterial);
                else highlightMaterials.Add(GenerateHighlightMaterial(material));
            }

            if (outlineMaterial) highlightMaterials.Add(outlineMaterial);
        }

        public void ToggleHighlight(bool highlightEnabled)
        {
            if (highlightEnabled)
            {
                renderer.materials = highlightMaterials.ToArray();
            }
            else
            {
                renderer.materials = originalMaterials.ToArray();
            }
        }

        public Material GenerateHighlightMaterial(Material originalMaterial)
        {
            Material newMaterial = new Material(originalMaterial);
            Texture mainTexture = newMaterial.mainTexture;

            newMaterial.SetTexture("_EmissionMap", mainTexture);
            float emission = 0.01f;
            Color color = Color.white * Mathf.LinearToGammaSpace(emission);
            newMaterial.SetColor("_EmissionColor", color);
            newMaterial.EnableKeyword("_EMISSION");

            return newMaterial;
        }

        public Renderer renderer;
        public List<Material> originalMaterials;
        public List<Material> highlightMaterials;
    }
}
