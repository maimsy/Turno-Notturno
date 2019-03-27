using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inspectable : Interactable
{
    [SerializeField] float inspectDistance = 3f;
    [SerializeField] float inspectSpeed = 0.05f;
    [SerializeField] float maxZoom = 0.4f;
    [SerializeField] InspectDirection inspectDirection = InspectDirection.xy;
    [SerializeField] bool flipDirection = false;
    [SerializeField] string name;

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
    private float distanceOffset = 0f; 

    private float zoom = 1;


    private Vector3 prevCameraPosition;
    private Quaternion prevCameraRotation;

    private GameManager manager;

    protected override void Start()
    {
        base.Start();
        manager = GameManager.GetInstance();
        if (!manager)
        {
            Debug.LogError("GameManager is missing from the scene!");
        }
        OnStart();
    }

    void CalculateBounds(Transform cameraTransform)
    {
        // Find the bounds to determine how far the camera can move
        //Bounds bounds = GetComponent<MeshRenderer>().bounds;
        transform.rotation = Quaternion.identity;
        Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;
        Debug.Log(bounds);
        center = bounds.center;
        Vector3 centeredBounds = bounds.extents;// - center;
        //bounds.
        Debug.Log(bounds.extents);
        //centeredBounds = cameraTransform.InverseTransformDirection(centeredBounds);
        centeredBounds = transform.InverseTransformDirection(centeredBounds);
        Vector3 centeredBounds2 = cameraTransform.InverseTransformDirection(-bounds.extents);
        //centeredBounds = bounds.extents;


        //centeredBounds.x = Mathf.Abs(centeredBounds.x);
        //centeredBounds.y = Mathf.Abs(centeredBounds.y);
        //centeredBounds.z = Mathf.Abs(centeredBounds.z);
        Debug.Log(centeredBounds);
        Debug.Log(centeredBounds2);

        maxHor = centeredBounds.x;
        maxVert = centeredBounds.y;
        distanceOffset = centeredBounds.z;

        //Vector3 v3Center = transform.InverseTransformPoint(bounds.center);
        //Vector3 v3Extents = transform.InverseTransformDirection(bounds.extents);
        Vector3 v3Center = (bounds.center);
        Vector3 v3Extents = (bounds.extents);

        Vector3 v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        Vector3 v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        Vector3 v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        Vector3 v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        Vector3 v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        Vector3 v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        Vector3 v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        Vector3 v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);

        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, Color.red, 0.1f);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, Color.red, 0.1f);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, Color.red, 0.1f);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, Color.red, 0.1f);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, Color.red, 0.1f);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, Color.red, 0.1f);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, Color.red, 0.1f);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, Color.red, 0.1f);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, Color.red, 0.1f);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, Color.red, 0.1f);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, Color.red, 0.1f);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, Color.red, 0.1f);
    }

    void DrawBox()
    {
        //if (Input.GetKey (KeyCode.S)) {
        

        /*switch (inspectDirection)
        {
            case InspectDirection.xy:
                maxHor = centeredBounds.x;
                maxVert = centeredBounds.y;
                distanceOffset = centeredBounds.z;
                break;
            case InspectDirection.xz:
                maxHor = centeredBounds.x;
                maxVert = centeredBounds.z;
                distanceOffset = centeredBounds.y;
                break;
            case InspectDirection.zy:
                maxHor = centeredBounds.z;
                maxVert = centeredBounds.y;
                distanceOffset = centeredBounds.x;
                break;
        }
        Debug.DrawRay(transform.position, new Vector3(centeredBounds.x, 0, 0), Color.red, 5f);
        Debug.DrawRay(transform.position, new Vector3(0, centeredBounds.y, 0), Color.red, 5f);
        Debug.DrawRay(transform.position, new Vector3(0, 0, centeredBounds.z), Color.red, 5f);
        Debug.DrawRay(transform.position, new Vector3(-centeredBounds.x, 0, 0), Color.red, 5f);
        Debug.DrawRay(transform.position, new Vector3(0, -centeredBounds.y, 0), Color.red, 5f);
        Debug.DrawRay(transform.position, new Vector3(0, 0, -centeredBounds.z), Color.red, 5f);
        */
        // Add a little more room
        maxHor *= 1.2f;
        maxVert *= 1.2f;
    }

    protected override void Update()
    {
        base.Update();
        if (manager.IsPaused()) return;
        if (inspecting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
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

        CalculateBounds(Camera.main.transform);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        StartInspect();
    }

    public override string GetTooltip()
    {
        return "Inspect " + name;
    }

    public void StartInspect()
    {
        center = transform.position;
        prevCameraPosition = Camera.main.transform.position;
        prevCameraRotation = Camera.main.transform.rotation;
        Camera.main.transform.position = GetStartingPosition();
        Camera.main.transform.LookAt(center);
        CalculateBounds(Camera.main.transform);
        Camera.main.transform.position = GetStartingPosition(); // Calculate bounds might change the zoom distance
        inspecting = true;
        manager.DisableControls();
        manager.HideCursor();
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
        return center + direction * distanceOffset + direction * inspectDistance * zoom;
    }

    [ContextMenu("Visualize")]
    public void Visualize()
    {
        //CalculateBounds();
        Debug.DrawRay(transform.position, GetStartingPosition() - transform.position, Color.red, 5f);
        //Debug.Log(center);
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
