using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePieceUI : MonoBehaviour
{
    public GameObject bridgePiece;
    private bool drag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            transform.position = Input.mousePosition;
        }
    }


    public void Drag()
    {
        drag = true;
    }
    public void Release()
    {
        /*
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 objPos = new Vector3(mousePos.x, mousePos.y, 0);
        Instantiate(bridgePiece, objPos, Quaternion.identity);
        */
        GameObject obj = Instantiate(bridgePiece, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);

        Destroy(gameObject);
    }
}
