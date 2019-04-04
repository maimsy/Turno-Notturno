using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawingLogic : MonoBehaviour
{
    public GameObject drawingPrefab;
    
    
    private float drawDistance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Draw();
        }
       
    }

    private void Draw()
    {
        if (!Physics.CheckSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), drawDistance))
        {
            GameObject obj = Instantiate(drawingPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
        }
    }

    
}
