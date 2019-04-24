using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ArtPieceDragging : MonoBehaviour
{
    private Vector3 originalPosition;
    private Color originalColor;
    private bool isDraggable = true;

    void Awake()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
        originalPosition = transform.position;
    }

    public void Reset()
    {
        isDraggable = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<SpriteRenderer>().color = originalColor;
        transform.position = originalPosition;
    }

    private void OnMouseDown()
    {
        if (isDraggable)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(GameObject.Find("InstructionText"));
        }
    }

    private void OnMouseDrag()
    {
        if (isDraggable)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

    private void OnMouseUp()
    {
        if (isDraggable)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/minigamePaintingDrop");
        }
    }

    public void DisableDragging()
    {
        if(isDraggable)
        {
            isDraggable = false;
            Color color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.8f * color.a);
        }
        
    }
}
