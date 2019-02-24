using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public GameObject text;
    private GameObject icon;
    public float lineWidth;

    private LineRenderer lineRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        SetDiscovered(false);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetWidth(lineWidth, lineWidth);
        Vector3 start = transform.position;
        Vector3 end = text.transform.position;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(0, end);

        icon = Resources.Load<GameObject>("ClueIcon");
        icon = Instantiate(icon, transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetZOffset(float offset)
    {
        Vector3 newPos = icon.transform.localPosition;
        newPos.z = offset;
        icon.transform.localPosition = newPos;
    }

    public void AnimateDiscovery()
    {
        // Reveal the clue slowly
        StartCoroutine("AnimateLine", 0.5f);
        EnableColliders(false);
    }

    public void SetDiscovered(bool value)
    {
        // Instantly shows or hides the clue
        
        if (icon) icon.SetActive(value);
        if (text) text.SetActive(value);
        if (lineRenderer) lineRenderer.enabled = value;
        EnableColliders(!value);  // Colliders should be enabled if the clue has not yet been discovered
    }

    public void EnableColliders(bool value)
    {
        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = value;
        }
    }



    IEnumerator AnimateLine(float duration)
    {
        // Show icon
        if (icon) icon.SetActive(true);
        
        // Animate line
        Vector3 start = icon.transform.position;
        Vector3 end = text.transform.position;
        Vector3 direction = end - start;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        for (float f = 1f; f >= 0; f -= 1/duration * Time.deltaTime) 
        {
            lineRenderer.SetPosition(1, start + direction * (1-f));
            yield return null;
        }
        
        // Show text
        if (text) text.SetActive(true);
    }
}
