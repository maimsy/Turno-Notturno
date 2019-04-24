using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class P2Dmovement : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] bool reDragging = false;
    private GameObject goal;
    private Vector2 losingXY;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(-50, 0, 10));
        losingXY = new Vector2(pos.x, pos.y);
        if (transform.position.x < losingXY.x)
        {
            //GetComponent<MinigameEnding>().EndGame(false);
            //enabled = false;
 
            FindObjectOfType<EnvironmentMove>().SmoothReset(EnvironmentMove.MinigameState.behind);
        }
        if(transform.position.y < losingXY.y)
        {
            FindObjectOfType<EnvironmentMove>().SmoothReset(EnvironmentMove.MinigameState.falling);
        }
    }


    private void Move()
    {
        //easy mode
        //GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);

        //hard mode
        GetComponent<Rigidbody2D>().AddForce(new Vector2(8, 0));
        if (GetComponent<Rigidbody2D>().velocity.x > speed)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        }
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Art")
        {
            if(!reDragging)
            {
                collision.gameObject.GetComponent<ArtPieceDragging>().DisableDragging();
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Goal")
        {
            GetComponent<MinigameEnding>().EndGame(true);
            ScribbleManager scribbleManager = FindObjectOfType<ScribbleManager>();
            if (scribbleManager)
            {
                DrawingLogic logic = GetComponent<DrawingLogic>();
                scribbleManager.SaveString(logic.GetLetters(), logic.GetPositions());
            }
            if(FindObjectOfType<EnvironmentMove>().GetState() != EnvironmentMove.MinigameState.behind)
            {
                enabled = false;
            }
        }
    } 
}
