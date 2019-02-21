using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : MonoBehaviour
{
    [SerializeField] float inspectDistance = 10f;
    [SerializeField] float inspectSpeed = 0.05f;
    [SerializeField] float maxZoom = 0.4f;

    private bool inspecting;
    private float x;
    private float y;
    private float maxX = 4;
    private float maxY = 4;

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

        // Find the bounds to determine how far the camera can move
        Bounds bounds = GetComponent<MeshRenderer>().bounds;
        Vector3 centeredBounds = bounds.max - transform.position;
        maxX = Mathf.Max(centeredBounds.x, centeredBounds.z);
        maxY = centeredBounds.y;
        OnStart();
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
        inspecting = true;
        manager.DisableControls();
        manager.HideCursor();
        prevCameraPosition = Camera.main.transform.position;
        prevCameraRotation = Camera.main.transform.rotation;
        Camera.main.transform.position = transform.position + transform.forward * inspectDistance;
        Camera.main.transform.LookAt(transform.position);
        x = 0;
        y = 0;
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
            hSpeed = Mathf.Clamp(maxX - x, 0, 1);
        }
        if (mouseX < 0)
        {
            hSpeed = Mathf.Clamp(maxX + x, 0, 1);
        }
        if (mouseY > 0)
        {
            vSpeed = Mathf.Clamp(maxY - y, 0, 1);
        }
        if (mouseY < 0)
        {
            vSpeed = Mathf.Clamp(maxY + y, 0, 1);
        }

        float h = inspectSpeed * hSpeed * mouseX * zoom * maxX;
        float v = inspectSpeed * vSpeed * mouseY * zoom * maxY;

        x = Mathf.Clamp(x + h, -maxX, maxX);
        y = Mathf.Clamp(y + v, -maxY, maxY);

        Camera.main.transform.position = transform.position + transform.forward * inspectDistance * zoom;
        Camera.main.transform.position -= Camera.main.transform.right * x;
        Camera.main.transform.position -= Camera.main.transform.up * y;
    }
}
