﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingLogic : MonoBehaviour
{
    public GameObject drawingPrefab;
    public GameObject winText;
    
    private float drawDistance = 0.1f;
    private float speed = 2f;
    private GameObject goal;
    private Vector2 losingXY;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Draw();
        }
        Move();
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
        losingXY = new Vector2(pos.x, pos.y);
        if (transform.position.x < losingXY.x || transform.position.y < losingXY.y)
        {
            Lose();
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

    private void Lose()
    {
        winText.GetComponent<Text>().text = "Booo";
        winText.SetActive(true);
    }

    private void Move()
    {
        //easy mode
        //GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);

        //hard mode
        GetComponent<Rigidbody2D>().AddForce(new Vector2(8, 0));
        if(GetComponent<Rigidbody2D>().velocity.x > speed)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        }
        
        Vector3 pos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(pos.x + speed / 75f, pos.y, pos.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Goal")
        {
            winText.SetActive(true);
        }
    }

    
}