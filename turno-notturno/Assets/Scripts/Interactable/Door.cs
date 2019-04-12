using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Door : BaseInteractable
{
    [SerializeField] String openTooltip = "close door";
    [SerializeField] String closedTooltip = "open door";
    [SerializeField] String lockedTooltip = "open door (locked)";
    public float slamTime = 0.1f;
    public float openingTime = 1f;
    public bool locked = false;
    public bool closed = false;
    public float closedAngle = 0;
    public float openAngle = 100;

    public StudioEventEmitter closeDoorSound;
    public StudioEventEmitter openDoorSound;
    public StudioEventEmitter lockedDoorSound;

    private float curTime;
    private bool moving = false;
    private Rigidbody rbody;
    private Vector3 originalPosition;
    private Animator animator;
    

    protected override void Awake()
    {
        base.Awake();
        curTime = openingTime;
        animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("Door does not have animation!");
        rbody = GetComponent<Rigidbody>();
        rbody.isKinematic = true;
        rbody.useGravity = false;
        moving = false;
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (moving)
        {
            FixPosition();
            float degreesPerSecond =  (openAngle - closedAngle) / curTime;
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
            if (closeDoorSound) closeDoorSound.Play();
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
        curTime = openingTime;
        closed = true;
        moving = true;
    }

    public void SlamClose()
    {
        curTime = slamTime;
        closed = true;
        moving = true;
    }

    public void Open()
    {
        curTime = openingTime;
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
                if (lockedDoorSound) lockedDoorSound.Play();
            }
            else
            {
                if (moving == false)
                {
                    // Only play animation/sound if the door was completely closed
                    if (animator) animator.Play("DoorHandleOpen");
                    if (openDoorSound) openDoorSound.Play();
                }
                Open();
            }
        }
        else Close();
    }

    public override void Interact()
    {
        ToggleDoor();
    }

    public override string GetTooltip()
    {
        String tooltip;
        if (closed && locked) tooltip = lockedTooltip;
        else if (closed) tooltip = closedTooltip;
        else tooltip = openTooltip;
        return tooltip;
    }

    public override bool IsInteractable()
    {
        return true;
    }
}
