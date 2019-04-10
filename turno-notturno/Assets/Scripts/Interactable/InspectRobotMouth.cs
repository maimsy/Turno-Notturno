using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectRobotMouth : Painting
{
    public SkinnedMeshRenderer mouth;

    private MouthArtWork robotScript;
    protected override void Awake()
    {
        base.Awake();
        //meshRenderer = mouth;
        robotScript = FindObjectOfType<MouthArtWork>();
    }

    protected override void OnStartInspect()
    {
        base.OnStartInspect();
        robotScript.disabled = true;
    }

    protected override void OnStopInspect()
    {
        base.OnStopInspect();
        robotScript.disabled = false;
    }
}
