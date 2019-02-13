using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{

    [SerializeField] Transform playerCamera;
    [SerializeField] Text interactTooltip;
    [SerializeField] float maxInteractDistance = 4f;

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
    Rigidbody rb;

    AudioSource Sound;
    AudioSource Death;
    public AudioClip[] Aw;
    public AudioClip Player_Death;

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
        LookRotation();

        Interact();

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
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, maxInteractDistance))
        {
            GameObject objectHit = hit.transform.gameObject;
            Interactable interactable = objectHit.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactTooltip) interactTooltip.text = "Press E to " + interactable.GetTooltip();
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.OnInteract();
                }
            }

        }
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
        move.x *= movementSpeed;
        move.z *= movementSpeed;
        move = transform.TransformDirection(move);

        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
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
    }

    public void HideCursor(bool value)
    {
        m_cursorIsLocked = value;
        InternalLockUpdate();
        Debug.Log(Cursor.visible);
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
}
