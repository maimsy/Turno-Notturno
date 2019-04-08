using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkRotateControl : BaseInteractable
{
    [SerializeField] Rotate artworkRotate;
    [SerializeField] string stopTooltip = "Stop rotation";
    [SerializeField] string startTooltip = "Start rotation";

    public override void Interact()
    {
        if (artworkRotate)
        {
            if (artworkRotate.rotationEnabled) artworkRotate.StopRotation();
            else artworkRotate.StartRotation();
        }
    }

    public override string GetTooltip()
    {
        if (artworkRotate && artworkRotate.rotationEnabled)
        {
            return stopTooltip;
        }
        else if (artworkRotate && !artworkRotate.rotationEnabled)
        {
            return startTooltip;
        }
        else
        {
            return "Broken plz fix";
        }
    }

    public override bool IsInteractable()
    {
        return true;
    }
}
