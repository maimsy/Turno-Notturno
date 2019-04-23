using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Movable : BaseInteractable
{
    [SerializeField] float throwVelocity = 20f;
    public string displayName;

    protected bool playerIsHolding = false;
    private Transform target;
    protected float targetDistance;
    private Rigidbody rbody;
    private bool gravityWasEnabled;
    private int originalLayer;
    private bool wasHoldingThisFrame;

    protected override void Awake()
    {
        base.Awake();
        originalLayer = gameObject.layer;
        rbody = GetComponent<Rigidbody>();
        rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        wasHoldingThisFrame = playerIsHolding;
        if (playerIsHolding)
        {
            Vector3 targetPos = target.position + target.forward * targetDistance;
            Vector3 offset = targetPos - GetCenterOfMass();
            rbody.velocity = offset * 10;
            rbody.angularVelocity = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Throw();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Drop();
            }
        }
    }

    protected Vector3 GetCenterOfMass()
    {
        return transform.TransformPoint(rbody.centerOfMass);
    }

    public virtual void Grab()
    {
        target = Camera.main.transform;
        targetDistance = (target.position - GetCenterOfMass()).magnitude;
        playerIsHolding = true;
        gravityWasEnabled = rbody.useGravity;
        rbody.useGravity = false;
        IgnorePlayerCollision(true);
        
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.SetTooltip2("Left click to throw \nRight click to drop");
        }
    }

    public void Drop()
    {
        playerIsHolding = false;
        rbody.useGravity = gravityWasEnabled;
        IgnorePlayerCollision(false);
        wasHoldingThisFrame = true;
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.SetTooltip2("");
        }
    }

    void IgnorePlayerCollision(bool value)
    {
        // Ignore collision between movable object and player to avoid flying while holding the object
        //if (value) gameObject.layer = LayerMask.NameToLayer("Player");
        //else gameObject.layer = originalLayer;
    }

    public void Throw()
    {
        Drop();
        rbody.velocity = target.forward * throwVelocity;
    }

    public override void Interact()
    {
        if (wasHoldingThisFrame)
        {
            // Avoid picking the object up immediately after dropping it
            return;
        }
        if (!playerIsHolding) Grab();
        else Drop();
    }

    public override string GetTooltip()
    {
        if (!playerIsHolding) return "Pick up " + displayName;
        return "";
    }

    public override bool IsInteractable()
    {
        return true;
    }

    public override void HighLight()
    {
        if (!playerIsHolding) base.HighLight();
    }
}
