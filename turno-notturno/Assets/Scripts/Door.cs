using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Door : Interactable
{
    [SerializeField] String closedTooltip = "open door";
    [SerializeField] String lockedTooltip = "open door (locked)";
    public float openingTime = 1f;
    public bool locked = false;
    public bool closed = false;
    public float closedAngle = 0;
    public float openAngle = 100;

    private bool moving = false;
    private Rigidbody rbody;
    private Vector3 originalPosition;
    private Animator animator;
    

    void Awake()
    {
        originalTooltip = tooltip;
        animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("Door does not have animation!");
        rbody = GetComponent<Rigidbody>();
        rbody.isKinematic = true;
        rbody.useGravity = false;
        moving = false;
        originalPosition = transform.position;
        UpdateTooltip();
        
    }

    void FixedUpdate()
    {
        if (moving)
        {
            FixPosition();
            float degreesPerSecond =  (openAngle - closedAngle) / openingTime;
            float radPerSecond = degreesPerSecond * Mathf.Deg2Rad;
            transform.position = originalPosition;

            float rotY = transform.rotation.eulerAngles.y;
            if (rotY > 180) rotY -= 360; // Wrap to 180, -180 
            if (closed && rotY < closedAngle)
            {
                moving = false;
                Snap();
            }
            else if (!closed && rotY > openAngle)
            {
                moving = false;
                Snap();
            }
            else if (closed)
            {
                rbody.angularVelocity = new Vector3(0, -radPerSecond, 0);
                rbody.isKinematic = false;
                rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            else
            {
                rbody.angularVelocity = new Vector3(0, radPerSecond, 0);
                rbody.isKinematic = false;
                rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
        
    }

    void Snap()
    {
        if (closed)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, closedAngle, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, openAngle, 0));  
        }
        transform.position = originalPosition;
        rbody.angularVelocity = Vector3.zero;
        rbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rbody.isKinematic = true;
    }

    void FixPosition()
    {
        transform.position = originalPosition;
        float y = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
    }

    public void Close()
    {
        closed = true;
        moving = true;
    }

    public void Open()
    {
        closed = false;
        moving = true;
    }

    public void ToggleDoor()
    {
        if (closed)
        {
            if (locked)
            {
                if (animator) animator.Play("DoorHandleLocked");
            }
            else
            {
                Open();
                if (animator) animator.Play("DoorHandleOpen");
            }
        }
        else Close();

        UpdateTooltip();
    }

    public override void OnInteract()
    {
        ToggleDoor();
        base.OnInteract();
    }

    public void UpdateTooltip()
    {
        if (closed && locked) tooltip = lockedTooltip;
        else if (closed) tooltip = closedTooltip;
        else tooltip = originalTooltip;
    }

    /*
    public override string GetTooltip()
    {
        if (closed && locked) return lockedTooltip;
        else if (closed) return closedTooltip;
        return tooltip;
    }
    */

    void Reset()
    {
        tooltip = "close door";
    }
}
