using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{

    public float moveSpeed = 0.01f;
    private Transform player;
    private float width;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        width = GetComponent<Renderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
        if(transform.position.x  < player.position.x - width)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x + width * 2, pos.y, pos.z);
        }
    }
}
