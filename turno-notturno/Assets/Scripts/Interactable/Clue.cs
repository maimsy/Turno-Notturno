using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : BaseInteractable
{
    [SerializeField] bool inspectOnly = true; // Clue can only be found when inspecting
    [SerializeField] string tooltip = "Collect clue";
    public ObjectiveManager.ClueObjective objective = ObjectiveManager.ClueObjective.NotImplemented;

    private bool clueCollected = false;

    private ObjectiveManager objectiveManager;

    void Start()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        if (inspectOnly)
        {
            //gameObject.layer = LayerMask.NameToLayer("InspectOnly");
        }
        else
        {
            //gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void Interact()
    {
        if (IsInteractable())
        {
            objectiveManager.InspectCluesGlobal(objective);
            clueCollected = true;
        }
    }

    public override string GetTooltip()
    {
        if (IsInteractable()) return tooltip;
        else return "";
    }

    public override bool IsInteractable()
    {
        if (inspectOnly && !Inspectable.playerIsInspecting) return false;
        return !clueCollected && GetObjectiveActive();
    }

    private bool GetObjectiveActive()
    {
        if (objectiveManager) return objectiveManager.IsObjectiveActive(objective);
        else
        {
            Debug.LogError(name + " cannot find ObjectiveManager!");
            return false;
        }
    }

    public override void HighLight()
    {
        if (IsInteractable()) base.HighLight();
    }
}
