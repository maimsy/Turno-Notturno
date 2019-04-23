﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Newspaper : Movable
{
    public bool flipOnEveryGrab = false;  // Used for letting the player see the back page
    public float forcedInspectionDistance = 1f;
    public Vector3 forcedInspectionRotation = new Vector3(0, 0, 0);
    
    private bool flipped = false;
    
    protected override void Update()
    {
        if (playerIsHolding)
        {
            targetDistance = forcedInspectionDistance;
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(forcedInspectionRotation);
        }

        base.Update();
    }

    public override void Grab()
    {
        base.Grab();

        if (flipOnEveryGrab)
        {
            forcedInspectionRotation = -forcedInspectionRotation;
        }
        
        // Force newspaper to its desired position
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * forcedInspectionDistance;
        Vector3 offset = targetPos - GetCenterOfMass();
        transform.position = targetPos;
        Debug.Log(targetPos);
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(forcedInspectionRotation);
        
        // Ignore collision with player - this should be done with layers instead
        Player player = FindObjectOfType<Player>();
        foreach (Collider col_1 in GetComponentsInChildren<Collider>())
        {
            foreach (Collider col_2 in player.gameObject.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col_1, col_2, true);
            }
        }


    }
}
