using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Movable : Interactable
{
    [SerializeField] float throwVelocity = 20f;
    private bool playerIsHolding = false;
    
    private Transform target;
    private float targetDistance;
    private Rigidbody rbody;
    private bool gravityWasEnabled;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        rbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsHolding)
        {
            Vector3 targetPos = target.position + target.forward * targetDistance;
            Vector3 offset = targetPos - transform.position;
            //rbody.AddForce(offset*10, ForceMode.Acceleration);
            rbody.velocity = offset * 10;
            rbody.angularVelocity = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Throw();
            }
        }
    }

    public void Grab()
    {
        target = Camera.main.transform;
        targetDistance = (target.position - transform.position).magnitude;
        playerIsHolding = true;
        gravityWasEnabled = rbody.useGravity;
        rbody.useGravity = false;
    }

    public void Drop()
    {
        playerIsHolding = false;
        rbody.useGravity = gravityWasEnabled;
    }

    public void Throw()
    {
        Drop();
        rbody.velocity = target.forward * throwVelocity;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if (!playerIsHolding) Grab();
        else Drop();
    }

    public override string GetTooltip()
    {
        if (!playerIsHolding) return "pick up";
        else return "drop. Throw with left click";
    }
}
