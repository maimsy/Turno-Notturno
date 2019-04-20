using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FlashLight : BaseInteractable
{
    private bool playerIsHolding = false;
    private Transform target;
    [SerializeField] float targetDistance;
    [SerializeField] float offSetRight;
    [SerializeField] float offSetDown;
    private Rigidbody rbody;
    private bool gravityWasEnabled;
    private bool wasHoldingThisFrame;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        rbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        base.Update();
        wasHoldingThisFrame = playerIsHolding;
        if (playerIsHolding)
        {
            

            Vector3 targetPos = target.position + target.forward * targetDistance + target.right * offSetRight + -target.up * offSetDown;
            Vector3 offset = targetPos - GetCenterOfMass();
            rbody.velocity = offset * 12;
            rbody.angularVelocity = Vector3.zero;
            transform.rotation = target.rotation;
            RaycastHit hit;
            if(Physics.Raycast(target.transform.position, target.forward, out hit))
            {
                Debug.Log(hit.point);
                Vector3 diff = Vector3.RotateTowards(transform.forward, (hit.point - transform.position), 1f, 0);
                transform.rotation = Quaternion.LookRotation(diff);
            }
            transform.rotation *= Quaternion.Euler(0, -90, 0);

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Drop();
            }
        }
    }

    Vector3 GetCenterOfMass()
    {
        return transform.TransformPoint(rbody.centerOfMass);
    }

    public void Grab()
    {
        playerIsHolding = true;
        GetComponent<MeshCollider>().enabled = false;
        target = Camera.main.transform;
        gravityWasEnabled = rbody.useGravity;
        rbody.useGravity = false;
        FMODUnity.RuntimeManager.PlayOneShot("event:/fx/flashlightPickup");
    }

    public void Drop()
    {
        playerIsHolding = false;
        rbody.useGravity = gravityWasEnabled;
        wasHoldingThisFrame = true;
        GetComponent<MeshCollider>().enabled = true;
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
        if (!playerIsHolding) return "Pick up ";
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
