using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainDoor : Door
{
    [SerializeField] UnityEvent interactionEvent;
    public string forcedTooltip = "";

    public override string GetTooltip()
    {
        if (forcedTooltip.Length > 0) return forcedTooltip;
        return base.GetTooltip();
    }

    public override void Interact()
    {
        interactionEvent.Invoke();
        base.Interact();
    }
}
