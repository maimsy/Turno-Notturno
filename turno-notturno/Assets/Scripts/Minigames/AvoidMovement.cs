using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AvoidMovement : MonoBehaviour
{
    private Vector3 originalPosition;
    private float moveSpeed = 0.1f;
    public GameObject goalText;
    public float resetTime = 1f;


    private bool resetting;
    private Vector3 resetVelocity;
    private Collider2D collider;

    struct ObjPos
    {
        public ObjPos(GameObject obj, Vector3 pos)
        {
            this.obj = obj;
            this.pos = pos;
            resetVelocity = Vector3.zero;
        }
        public GameObject obj;
        public Vector3 pos;
        public Vector3 resetVelocity;
    }

    private List<ObjPos> backgroundObjects;

    // Start is called before the first frame update
    void Start()
    {
        resetting = false;
        originalPosition = transform.position;
        collider = GetComponent<Collider2D>();
        backgroundObjects = new List<ObjPos>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Layer1"))
        {
            backgroundObjects.Add(new ObjPos(obj, obj.transform.position));
        }
        GetComponent<Animator>().speed = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resetting)
        {
            // Smoothly move player and background layers back to position (Slerp/Lerp could also be used instead of SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, originalPosition, ref resetVelocity, resetTime);

            foreach (ObjPos backgroundObject in backgroundObjects)
            {
                ObjPos op = backgroundObject;
                float time = resetTime / 5;  // Background objects need to move faster for some reason (Maybe affected by scale?)
                op.obj.transform.position = Vector3.SmoothDamp(op.obj.transform.position, op.pos, ref op.resetVelocity, time); 
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
            }
            transform.position = new Vector2(Mathf.Max(-9.75f, transform.position.x), transform.position.y);
        }
    }

    public void SmoothReset()
    {
        StudioEventEmitter sound = GameObject.Find("BirthSound").GetComponent<StudioEventEmitter>();
        if (!sound.IsPlaying())
        {
            sound.Play();
        }
        resetting = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        resetVelocity = Vector3.zero;
        collider.enabled = false;
        Invoke("Reset", resetTime*2);
        MiniSound sounds = FindObjectOfType<MiniSound>();
        sounds.Reset();
        foreach (ObjPos backgroundObject in backgroundObjects)
        {
            ObjPos op = backgroundObject;
            op.resetVelocity = Vector3.zero;
            BackGroundMovement movement = op.obj.GetComponent<BackGroundMovement>();
            if (movement) movement.enabled = false;
        }
    }

    void Reset()
    {
        //transform.position = originalPosition;
        collider.enabled = true;
        resetting = false;
        foreach (ObjPos backgroundObject in backgroundObjects)
        {
            ObjPos op = backgroundObject;
            BackGroundMovement movement = op.obj.GetComponent<BackGroundMovement>();
            if (movement) movement.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Dangerous")
        {
            //GetComponent<MinigameEnding>().EndGame(false);
            //GetComponent<SpriteRenderer>().enabled = false;
            //enabled = false;
            SmoothReset();
            foreach (EnemyMovement enemyMovement in FindObjectsOfType<EnemyMovement>())
            {
                enemyMovement.SmoothReset();
            }
        }
        else if(collision.gameObject.tag == "Goal")
        {
            GetComponent<MinigameEnding>().EndGame(true);
            GetComponent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
    }
}
