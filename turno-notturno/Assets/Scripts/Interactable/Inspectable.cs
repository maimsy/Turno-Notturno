using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inspectable : BaseInteractable
{
    [SerializeField] float inspectDistance = 3f;
    [SerializeField] float inspectSpeed = 0.05f;
    [SerializeField] float maxZoom = 0.4f;
    [SerializeField] InspectDirection inspectDirection = InspectDirection.xy;
    [SerializeField] bool flipDirection = false;
    [SerializeField] string displayName = "artwork";
    [SerializeField] UnityEvent objectiveEvent;
    [SerializeField] string objective = "";
    static public bool playerIsInspecting = false;

    public enum InspectDirection
    {
        xy,
        xz,
        zy
    }

    private Vector3 localCenter;

    private bool inspecting;
    private float hor;
    private float vert;
    private float maxHor = 4;
    private float maxVert = 4;
    private float distanceOffset = 0f;

    private float zoom = 1;


    private Vector3 prevCameraPosition;
    private Quaternion prevCameraRotation;

    private GameManager gameManager;
    private Bounds bounds;
    private Vector3[] originalCorners;
    private Vector3[] rotatedCorners;
    protected MeshRenderer meshRenderer;

    private ObjectiveManager objectiveManager;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        
    }

    protected virtual void Start()
    {
        InitialBounds();
        playerIsInspecting = false;
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        gameManager = GameManager.GetInstance();
        if (!gameManager)
        {
            Debug.LogError("GameManager is missing from the scene!");
        }

        OnStart();
    }

    void InitialBounds()
    {
        if (!meshRenderer)
        {
            Debug.LogError("Inspectable object missing MeshRenderer!");
            return;
        }

        // Calculate bounding box in local space
        Quaternion originalRotation = transform.rotation;
        transform.rotation =
            Quaternion.identity; // Rotate temporarily to (0,0,0), because MeshRenderer bounds work weirdly
        bounds = meshRenderer.bounds;
        foreach (var childRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            // Add bounds from all child mesh renderers
            Bounds b = childRenderer.bounds;
            bounds.Encapsulate(b);
        }

        //bounds.extents = transform.InverseTransformDirection(bounds.extents);
        Vector3 center = (bounds.center);
        localCenter = transform.InverseTransformPoint(center);
        Vector3 v3Extents = (bounds.extents);

        Vector3 v3FrontTopLeft = new Vector3(center.x - v3Extents.x, center.y + v3Extents.y, center.z - v3Extents.z);
        Vector3 v3FrontTopRight = new Vector3(center.x + v3Extents.x, center.y + v3Extents.y, center.z - v3Extents.z);
        Vector3 v3FrontBottomLeft = new Vector3(center.x - v3Extents.x, center.y - v3Extents.y, center.z - v3Extents.z);
        Vector3 v3FrontBottomRight =
            new Vector3(center.x + v3Extents.x, center.y - v3Extents.y, center.z - v3Extents.z);
        Vector3 v3BackTopLeft = new Vector3(center.x - v3Extents.x, center.y + v3Extents.y, center.z + v3Extents.z);
        Vector3 v3BackTopRight = new Vector3(center.x + v3Extents.x, center.y + v3Extents.y, center.z + v3Extents.z);
        Vector3 v3BackBottomLeft = new Vector3(center.x - v3Extents.x, center.y - v3Extents.y, center.z + v3Extents.z);
        Vector3 v3BackBottomRight = new Vector3(center.x + v3Extents.x, center.y - v3Extents.y, center.z + v3Extents.z);

        originalCorners = new[]
        {
            v3FrontTopLeft, v3FrontTopRight, v3FrontBottomLeft, v3FrontBottomRight, v3BackTopLeft, v3BackTopRight,
            v3BackBottomLeft, v3BackBottomRight
        };

        rotatedCorners = new[]
        {
            v3FrontTopLeft, v3FrontTopRight, v3FrontBottomLeft, v3FrontBottomRight, v3BackTopLeft, v3BackTopRight,
            v3BackBottomLeft, v3BackBottomRight
        };

        for (int i = 0; i < originalCorners.Length; i++)
        {
            originalCorners[i] = transform.InverseTransformPoint(originalCorners[i]); // Transform to local space
        }

        transform.rotation = originalRotation;
    }

    void CalculateBounds(Transform cameraTransform)
    {
        if (!meshRenderer)
        {
            Debug.LogError("Inspectable object missing MeshRenderer!");
            return;
        }

        // Calculate bounding box in camera space to determine how far the camera can move
        float maxX = 0;
        float maxY = 0;
        float maxZ = 0;

        for (int i = 0; i < originalCorners.Length; i++)
        {
            rotatedCorners[i] = transform.TransformPoint(originalCorners[i]); // World space
            rotatedCorners[i] = cameraTransform.InverseTransformPoint(rotatedCorners[i]); // Camera space
            maxX = Mathf.Max(maxX, Mathf.Abs(rotatedCorners[i].x));
            maxY = Mathf.Max(maxY, Mathf.Abs(rotatedCorners[i].y));
            maxZ = Mathf.Max(maxZ, Mathf.Abs(rotatedCorners[i].z));
        }

        maxHor = maxX;
        maxVert = maxY;
        distanceOffset = maxZ - (Vector3.Distance(cameraTransform.position, GetCenter()));
    }

    void DrawBox()
    {
        if (!meshRenderer)
        {
            Debug.LogError("Inspectable object missing MeshRenderer!");
            return;
        }

        // Draw bounding box for debugging
        for (int i = 0; i < originalCorners.Length; i++)
        {
            rotatedCorners[i] = transform.TransformPoint(originalCorners[i]); // World space
        }

        Vector3 v3FrontTopLeft = rotatedCorners[0];
        Vector3 v3FrontTopRight = rotatedCorners[1];
        Vector3 v3FrontBottomLeft = rotatedCorners[2];
        Vector3 v3FrontBottomRight = rotatedCorners[3];
        Vector3 v3BackTopLeft = rotatedCorners[4];
        Vector3 v3BackTopRight = rotatedCorners[5];
        Vector3 v3BackBottomLeft = rotatedCorners[6];
        Vector3 v3BackBottomRight = rotatedCorners[7];

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

    protected override void Update()
    {
        base.Update();
        if (gameManager.IsPaused()) return;
        if (inspecting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StopInspect();
                return;
            }

            HandleInspectMovement();

            // Set zoom
            if (Input.GetKey(KeyCode.Space)) zoom = maxZoom;
            else zoom = 1;

            OnUpdate();
        }

        //DrawBox();
        //CalculateBounds(Camera.main.transform);
    }

    public override void Interact()
    {
        StartInspect();
        if (objectiveManager && objectiveManager.IsObjectiveActive(objective))
        {
            objectiveEvent.Invoke();
        }
    }

    public override string GetTooltip()
    {
        return "Inspect " + displayName;
    }

    public override bool IsInteractable()
    {
        return true;
    }

    public void StartInspect()
    {
        //center = transform.position;
        prevCameraPosition = Camera.main.transform.position;
        prevCameraRotation = Camera.main.transform.rotation;
        Camera.main.transform.position = GetStartingPosition();
        Camera.main.transform.LookAt(GetCenter());
        CalculateBounds(Camera.main.transform);
        Camera.main.transform.position = GetStartingPosition(); // Calculate bounds might change the zoom distance
        inspecting = true;
        playerIsInspecting = true;
        gameManager.DisableControls();
        gameManager.HideCursor();
        hor = 0;
        vert = 0;
        OnStartInspect();

        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.SetTooltip2("Right click to exit");
        }
    }

    public void StopInspect()
    {
        playerIsInspecting = false;
        inspecting = false;
        Camera.main.transform.position = prevCameraPosition;
        Camera.main.transform.rotation = prevCameraRotation;
        gameManager.EnableControls();
        OnStopInspect();
        
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.SetTooltip2("");
        }
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

        /*
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
        */

        float h = inspectSpeed * hSpeed * mouseX * zoom * maxHor;
        float v = inspectSpeed * vSpeed * mouseY * zoom * maxVert;

        hor = Mathf.Clamp(hor + h, -maxHor, maxHor);
        vert = Mathf.Clamp(vert + v, -maxVert, maxVert);

        Camera.main.transform.position = GetStartingPosition();
        Camera.main.transform.position -= Camera.main.transform.right * hor;
        Camera.main.transform.position -= Camera.main.transform.up * vert;
    }

    Vector3 GetCenter()
    {
        //Debug.Log(localCenter);
        return transform.TransformPoint(localCenter);
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
        return GetCenter() + direction * distanceOffset + direction * inspectDistance * zoom;
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