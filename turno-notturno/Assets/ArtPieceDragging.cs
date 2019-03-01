using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtPieceDragging : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnMouseDrag()
    {
        //Debug.Log(Input.mousePosition);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.position = new Vector3(pos.x, pos.y, 0);
        Debug.Log(transform.position);
    }

    private void OnMouseUp()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
