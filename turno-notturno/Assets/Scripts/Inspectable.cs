using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inspectable : MonoBehaviour
{
    [SerializeField] float inspectDistance = 3f;
    [SerializeField] float inspectSpeed = 0.05f;
    [SerializeField] float maxZoom = 0.4f;
    [SerializeField] InspectDirection inspectDirection = InspectDirection.xy;
    [SerializeField] bool flipDirection = false;

    public enum InspectDirection
    {
        xy,
        xz,
        zy
    }

    private Vector3 center;

    private bool inspecting;
    private float hor;
    private float vert;
    private float maxHor = 4;
    private float maxVert = 4;

    private float zoom = 1;

    private Vector3 prevCameraPosition;
    private Quaternion prevCameraRotation;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.GetInstance();
        if (!manager)
        {
            Debug.LogError("GameManager is missing from the scene!");
        }
        OnStart();
    }

    void CalculateBounds()
    {
        // Find the bounds to determine how far the camera can move
        Bounds bounds = GetComponent<MeshRenderer>().bounds;
        center = bounds.center;
        Vector3 centeredBounds = bounds.max - center;
        switch (inspectDirection)
        {
            case InspectDirection.xy:
                maxHor = centeredBounds.x;
                maxVert = centeredBounds.y;
                break;
            case InspectDirection.xz:
                maxHor = centeredBounds.x;
                maxVert = centeredBounds.z;
                break;
            case InspectDirection.zy:
                maxHor = centeredBounds.z;
                maxVert = centeredBounds.y;
                break;
        }

        // Add a little more room
        maxHor *= 1.2f;
        maxVert *= 1.2f;
    }

    void Update()
    {
        if (manager.IsPaused()) return;
        if (inspecting)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                StopInspect();
                return;
            }

            HandleInspectMovement();

            // Set zoom
            if (Input.GetKey(KeyCode.Mouse0)) zoom = maxZoom;
            else zoom = 1;

            OnUpdate();
        }
    }

    public void StartInspect()
    {
        CalculateBounds();
        inspecting = true;
        manager.DisableControls();
        manager.HideCursor();
        prevCameraPosition = Camera.main.transform.position;
        prevCameraRotation = Camera.main.transform.rotation;
        Camera.main.transform.position = GetStartingPosition();
        Camera.main.transform.LookAt(center);
        hor = 0;
        vert = 0;
        OnStartInspect();
    }

    public void StopInspect()
    {
        inspecting = false;
        Camera.main.transform.position = prevCameraPosition;
        Camera.main.transform.rotation = prevCameraRotation;
        manager.EnableControls();
        OnStopInspect();
    }

    protected virtual void OnStartInspect()
    {
        // Reserved for subclasses
    }

    protected virtual void OnStopInspect()
    {
        // Reserved for subclasses
    }

    protected virtual void OnUpdate()
    {
        // Reserved for subclasses
    }

    protected virtual void OnStart()
    {
        // Reserved for subclasses
    }

    void HandleInspectMovement()
    {
        float vSpeed = 1;
        float hSpeed = 1;

        float mouseX = -Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        if (mouseX > 0)
        {
            hSpeed = Mathf.Clamp(maxHor - hor, 0, 1);
        }
        if (mouseX < 0)
        {
            hSpeed = Mathf.Clamp(maxHor + hor, 0, 1);
        }
        if (mouseY > 0)
        {
            vSpeed = Mathf.Clamp(maxVert - vert, 0, 1);
        }
        if (mouseY < 0)
        {
            vSpeed = Mathf.Clamp(maxVert + vert, 0, 1);
        }

        float h = inspectSpeed * hSpeed * mouseX * zoom * maxHor;
        float v = inspectSpeed * vSpeed * mouseY * zoom * maxVert;

        hor = Mathf.Clamp(hor + h, -maxHor, maxHor);
        vert = Mathf.Clamp(vert + v, -maxVert, maxVert);

        Camera.main.transform.position = GetStartingPosition();
        Camera.main.transform.position -= Camera.main.transform.right * hor;
        Camera.main.transform.position -= Camera.main.transform.up * vert;
    }

    Vector3 GetStartingPosition()
    {
        Vector3 direction = Vector3.zero;
        switch (inspectDirection)
        {
            case InspectDirection.xy:
                direction = transform.forward;
                break;
            case InspectDirection.xz:
                direction = transform.up;
                break;
            case InspectDirection.zy:
                direction = transform.right;
                break;
        }
        if (flipDirection) direction = -direction;
        return center + direction * inspectDistance * zoom;
    }

    [ContextMenu("Visualize")]
    public void Visualize()
    {
        CalculateBounds();
        Debug.DrawRay(center, GetStartingPosition() - center, Color.red, 5f);
        Debug.Log(center);
    }

    /*
    void Reset()
    {
        // Automatically add interaction script when first created
        Interactable interactable = GetComponent<Interactable>();
        if (!interactable)
        {
            UnityEvent inspectEvent = new UnityEvent();
            inspectEvent.AddListener(StartInspect);
            interactable = gameObject.AddComponent<Interactable>();
            interactable.AddEvent(inspectEvent);
        }
    }
    */
}
