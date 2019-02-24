using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBar : MonoBehaviour
{
    private Vector3 endPos;
    private float moveSpeed = 0.1f;
    private bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        endPos = transform.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed);
        }
    }

    public void StartMoving()
    {
        moving = true;
        FindObjectOfType<ObjectiveManager>().CompleteObjective("window1");
        FindObjectOfType<ObjectiveManager>().NewObjective("door1", "Lock the doors");
    }
}
