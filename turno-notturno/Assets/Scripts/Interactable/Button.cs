using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class Button : BaseInteractable
{
    [SerializeField] UnityEvent interactionEvent;
    [SerializeField] StudioEventEmitter soundEmitter;
    [SerializeField] bool triggersOnce = false;
    [SerializeField] bool isEnabled = true;
    [SerializeField] string enabledTooltip = "Press button";
    [SerializeField] string disabledTooltip = "Button";
    [SerializeField] bool requireObjective = false;
    [SerializeField] string[] objective;

    private ObjectiveManager objectiveManager;

    void Start()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    public override void Interact()
    {
        if (IsInteractable())
        {
            if (soundEmitter) soundEmitter.Play();
            interactionEvent.Invoke();
            if (triggersOnce) isEnabled = false;
        } 
    }

    public override string GetTooltip()
    {
        if (IsInteractable())
        {
            return enabledTooltip;
        }
        else
        {
            return disabledTooltip;
        }
    }

    public override bool IsInteractable()
    {
        return isEnabled && GetObjectiveActive();
    }

    private bool GetObjectiveActive()
    {
        if (objectiveManager)
        {
            foreach(string obj in objective)
            {
                if (objectiveManager.IsObjectiveActive(obj)) return true;
            }
            return false;
            
        }
        else
        {
            Debug.LogError("Button cannot find ObjectiveManager!");
            return false;
        }
    }
}
