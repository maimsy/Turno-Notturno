using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidMovement : MonoBehaviour
{

    private float moveSpeed = 0.1f;
    public GameObject goalText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x-moveSpeed, transform.position.y);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x+moveSpeed, transform.position.y);
        }
        transform.position = new Vector2(Mathf.Max(-9.75f, transform.position.x), transform.position.y);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Dangerous")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Goal")
        {
            goalText.SetActive(true);
        }
    }
}
