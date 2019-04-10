using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Character
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Text interactTooltip;
    [SerializeField] float maxInteractDistance = 4f;

    [SerializeField] float runSpeedMultiplier = 1.5f;
    [SerializeField] float maxMovementSpeed = 5f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float XSensitivity = 1f;
    [SerializeField] float YSensitivity = 1f;
    [SerializeField] bool clampVerticalRotation = true;
    [SerializeField] float MinimumX = -90F;
    [SerializeField] float MaximumX = 90F;
    [SerializeField] float cameraSmoothing = 5f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float shootingCooldown = 0.5f;

    Quaternion characterTargetRotation;
    Quaternion cameraTargetRotation;
    bool m_cursorIsLocked = true;
    bool cameraRotationEnabled = true;
    Rigidbody rb;

    AudioSource Sound;
    AudioSource Death;
    public AudioClip[] Aw;
    public AudioClip Player_Death;
    public bool dizzy = false;

    private string tipString = "Left click to ";
    private bool rotatingRight = false;
    private float rotator = 0;
    private float rotateLimit = 1;
    private float rotateSpeed = 0.01f;

    private bool running = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterTargetRotation = transform.localRotation;
        cameraTargetRotation = playerCamera.localRotation;
        Sound = gameObject.AddComponent<AudioSource>();
        Death = gameObject.AddComponent<AudioSource>();
        m_cursorIsLocked = lockCursor;
        InternalLockUpdate();
        InitCharacter();
    }

    void Update()
    {
        if(cameraRotationEnabled) LookRotation();

        Interact();
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Toggle cursor and camera rotation (makes editing the scene easier while playing)
            cameraRotationEnabled = !cameraRotationEnabled;
            HideCursor(cameraRotationEnabled);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
             
        }
    }

    void OnDisable()
    {
        // Quick fix for interaction text being stuck while inspecting pictures/final puzzle
        if (interactTooltip) interactTooltip.text = "";
    }

    void Interact()
    {
        if (interactTooltip) interactTooltip.text = "";
        BaseInteractable.StopPreviousHighlight();
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        LayerMask mask = ~(1 << LayerMask.NameToLayer("InspectOnly") | 1 << LayerMask.NameToLayer("Ignore Raycast")); // Ignore "InspectOnly" layer, which is handled by Inspectable objects
        if (Physics.Raycast(ray, out hit, maxInteractDistance, mask)) 
        {
            GameObject objectHit = hit.transform.gameObject;
            BaseInteractable interactable = GetInteractable(objectHit);
            if (interactable != null)
            {
                String tooltip = interactable.GetTooltip();
                if (tipString.Length > 0 && interactable.IsInteractable())
                {
                    // Add tooltip and force lowercase first letter 
                    if (tooltip.Length > 0) tooltip = char.ToLower(tooltip[0]) + tooltip.Substring(1);
                    tooltip = tipString + tooltip;
                }
                else
                {
                    // Force uppercase first letter 
                    if (tooltip.Length > 0) tooltip = char.ToUpper(tooltip[0]) + tooltip.Substring(1);
                }
                interactable.HighLight();
                if (interactTooltip) interactTooltip.text = tooltip;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactable.Interact();
                    if (interactable.IsInteractable()) tipString = "";
                }
            }

        }
    }

    private BaseInteractable GetInteractable(GameObject obj)
    {
        BaseInteractable[] interactables = obj.GetComponentsInParent<BaseInteractable>();

        // Prefer interactables that are currently active
        foreach (BaseInteractable interactable in interactables)
        {
            if (interactable.isActiveAndEnabled && interactable.IsInteractable()) return interactable;
        }


        // Otherwise return the first enabled interactable found
        if (interactables.Length > 0)
        {
            foreach (BaseInteractable interactable in interactables)
            {
                if (interactable.isActiveAndEnabled) return interactable;
            }
        }
        return null;
    }

    public void SetTooltip(String str)
    {
        if (interactTooltip) interactTooltip.text = str;
    }

    public override void TakeDamage(int damage, Transform attacker)
    {
        Sound.clip = Aw[Random.Range(0, Aw.Length)];
        Sound.Play();
        healthRemaining -= damage;
        Debug.Log("Hit character. Health remaining: " + healthRemaining);
        if (healthRemaining <= 0 && !dead)
        {
            Death.clip = Player_Death;
            Death.Play();
            Die(attacker);
        }
    } 

    void FixedUpdate()
    {
        Vector3 move = MovementVector();
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }

        float speed = movementSpeed;
        if (Input.GetButton("Run"))
        {
            running = true;
            speed *= runSpeedMultiplier;
        }
        else
        {
            running = false;
        }
        move.x *= speed;
        move.z *= speed;
        
        move = transform.TransformDirection(move);

        //rb.MovePosition(rb.position + move * Time.fixedDeltaTime);


        move *= Time.fixedDeltaTime;
        //move.y = rb.velocity.y;
        //rb.velocity = Vector3.zero;
        rb.AddForce(move, ForceMode.Acceleration);
        rb.angularVelocity = Vector3.zero;

        Vector3 velo = Vector3.zero;
        velo.x = rb.velocity.x;
        velo.z = rb.velocity.z;
        
        if (Input.GetButton("Run") && velo.magnitude > maxMovementSpeed * runSpeedMultiplier)
        {
            velo = velo.normalized * maxMovementSpeed * runSpeedMultiplier;
        }
        else if (velo.magnitude > maxMovementSpeed)
        {
            velo = velo.normalized * maxMovementSpeed;
        }
        
        velo.y = rb.velocity.y;
        if (velo.y > 0)
        {
            // Stop bouncing
            velo.y = 0;
        }
        rb.velocity = velo;
    }
    
    Vector3 MovementVector()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, 0, v);
    }

    void LookRotation()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        characterTargetRotation *= Quaternion.Euler(0f, yRot, 0f);
        cameraTargetRotation *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation);

        transform.rotation = Quaternion.Slerp(transform.localRotation, characterTargetRotation, cameraSmoothing);
        playerCamera.localRotation = Quaternion.Slerp(playerCamera.localRotation, cameraTargetRotation, cameraSmoothing);
        if(dizzy)
        {
            Dizzyness();
        }
    }

    private void Dizzyness()
    {
        if (rotatingRight)
        {
            gameObject.transform.Rotate(Vector3.forward, 0.01f * 10);
            rotator += rotateSpeed;
            if (rotator > rotateLimit) rotatingRight = false;
        }
        else
        {
            gameObject.transform.Rotate(Vector3.forward, -0.01f * 10);
            rotator -= rotateSpeed;
            if (rotator < 0) rotatingRight = true;
        }
    }

    public void RotateTo(Quaternion rotation)
    {
        characterTargetRotation = rotation;
    }

    public void HideCursor(bool value)
    {
        m_cursorIsLocked = value;
        InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    private void OnCollisionStay(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
        {
            //collision was from below
            if (collision.collider.tag == "Stairs")
            {
                FindObjectOfType<SoundManager>().footstepsound = "event:/footstepsStairs";
            }
            else
            {
                FindObjectOfType<SoundManager>().footstepsound = "event:/footsteps";
            }
        }


    }

    public bool IsRunning()
    {
        return running;
    }
}
