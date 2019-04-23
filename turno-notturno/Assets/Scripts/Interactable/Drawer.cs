using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Drawer : BaseInteractable
{
    [SerializeField] String openTooltip = "close drawer";
    [SerializeField] String closedTooltip = "open drawer";
    [SerializeField] String lockedTooltip = "open drawer (locked)";
    public float speed = 1f;
    public bool locked = false;
    public float openX = -0.5f;

    public StudioEventEmitter openSound;
    public StudioEventEmitter closeSound;

    private float curTime;
    private bool opening = false;
    private Rigidbody rbody;
    private Vector3 originalPosition;
    private Animator animator;

    private float targetX;
    private float closedX;

    private List<GameObject> containedObjects;
    

    protected override void Awake()
    {
        base.Awake();
        containedObjects = new List<GameObject>();
        originalPosition = transform.position;
        targetX = transform.localPosition.x;
        closedX = targetX;
        openSound = gameObject.AddComponent<StudioEventEmitter>();
        openSound.Event = "event:/fx/drawerOpen";
        
        closeSound = gameObject.AddComponent<StudioEventEmitter>();
        closeSound.Event = "event:/fx/drawerClose";
    }

    void FixedUpdate()
    {
        float curX = transform.localPosition.x;
        if (curX != targetX)
        {
            // Move drawer towards target position
            float dir = targetX - curX;
            dir = Mathf.Clamp(dir, -speed * Time.fixedDeltaTime, speed * Time.fixedDeltaTime);
            curX += dir;
            Vector3 newpos = transform.localPosition;
            newpos.x = curX;
            Vector3 dirVec = newpos - transform.localPosition;
            transform.localPosition = newpos;
            Vector3 worldDir = transform.TransformDirection(dirVec);
            
            // Ensure that every object inside the drawer stays there
            foreach (GameObject containedObject in containedObjects)
            {
                Vector3 pos = containedObject.transform.position;
                pos += worldDir;
                containedObject.transform.position = pos;
            }

            if (!opening && transform.localPosition.x == targetX)
            {
                closeSound.Play();
            }
        }
    }

    
    public void ToggleDoor()
    {
        opening = !opening;
        if (opening)
        {
            targetX = openX;
            openSound.Play();
        }
        else
        {
            targetX = closedX;

        }
    }

    public override void Interact()
    {
        ToggleDoor();
    }

    public override string GetTooltip()
    {
        String tooltip;
        if (locked) tooltip = lockedTooltip;
        else if (!opening) tooltip = closedTooltip;
        else tooltip = openTooltip;
        return tooltip;
    }

    public override bool IsInteractable()
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rbody = other.GetComponentInChildren<Rigidbody>();
        if (!rbody ) rbody  = other.GetComponentInParent<Rigidbody>();
        if (rbody && !containedObjects.Contains(rbody.gameObject))
        {
            containedObjects.Add(rbody.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rbody = other.GetComponentInChildren<Rigidbody>();
        if (!rbody ) rbody  = other.GetComponentInParent<Rigidbody>();
        if (rbody && containedObjects.Contains(rbody.gameObject))
        {
            containedObjects.Remove(rbody.gameObject);
        }
    }
}
