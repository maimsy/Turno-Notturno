using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static float resetTime = 1f;
    public float moveSpeed = 4f;

    private bool resetting = false;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 resetVelocity;

    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        resetting = false;
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        collider = GetComponent<Collider2D>();
        RandomizeDirection();
    }

    public void SmoothReset()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        resetting = true;
        resetVelocity = Vector3.zero;
        collider.enabled = false;
        Invoke("Reset", resetTime * 2);
    }

    void Reset()
    {
        //transform.position = originalPosition;
        transform.rotation = Quaternion.Euler(originalRotation);
        collider.enabled = true;
        resetting = false;
        RandomizeDirection();
    }

    void RandomizeDirection()
    {
        while(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < moveSpeed*0.3f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, Random.Range(-moveSpeed, moveSpeed));
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (resetting)
        {
            transform.position = Vector3.SmoothDamp(transform.position, originalPosition, ref resetVelocity, resetTime);
        }
    }
}
