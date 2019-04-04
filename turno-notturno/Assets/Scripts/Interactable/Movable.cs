using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Movable : Interactable
{
    [SerializeField] float throwVelocity = 20f;
    public string name;
    private bool playerIsHolding = false;
    
    private Transform target;
    private float targetDistance;
    private Rigidbody rbody;
    private bool gravityWasEnabled;
    private int originalLayer;
    private bool wasHoldingThisFrame;

    void Awake()
    {
        originalLayer = gameObject.layer;
        rbody = GetComponent<Rigidbody>();
        rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    // Update is called once per frame
    void Update()
    {
        wasHoldingThisFrame = playerIsHolding;
        if (playerIsHolding)
        {
            Vector3 targetPos = target.position + target.forward * targetDistance;
            Vector3 offset = targetPos - GetCenterOfMass();
            //rbody.AddForce(offset*10, ForceMode.Acceleration);
            rbody.velocity = offset * 10;
            rbody.angularVelocity = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Throw();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Drop();
            }
        }
        renderer.material.shader = originalShader;
    }

    Vector3 GetCenterOfMass()
    {
        return transform.TransformPoint(rbody.centerOfMass);
    }

    public void Grab()
    {
        target = Camera.main.transform;
        targetDistance = (target.position - GetCenterOfMass()).magnitude;
        playerIsHolding = true;
        gravityWasEnabled = rbody.useGravity;
        rbody.useGravity = false;
        IgnorePlayerCollision(true);
    }

    public void Drop()
    {
        playerIsHolding = false;
        rbody.useGravity = gravityWasEnabled;
        IgnorePlayerCollision(false);
        wasHoldingThisFrame = true;
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

    public override void OnInteract()
    {
        if (wasHoldingThisFrame)
        {
            // Avoid picking the object up immediately after dropping it
            return;
        }
        base.OnInteract();
        if (!playerIsHolding) Grab();
        else Drop();
    }

    public override string GetTooltip()
    {
        if (!playerIsHolding) return "pick up " + name;
        else return "Left click to Drop \nE to throw";
    }
}
