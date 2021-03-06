﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pMovement : MonoBehaviour {


    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public float movespeed = 5f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.W))
        {
            p1.transform.position = new Vector3(transform.position.x, transform.position.y + movespeed);
            p2.transform.position = new Vector3(p2.transform.position.x, p2.transform.position.y + movespeed);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            p1.transform.position = new Vector3(transform.position.x - movespeed, transform.position.y);
            p2.transform.position = new Vector3(p2.transform.position.x + movespeed, p2.transform.position.y);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            p1.transform.position = new Vector3(transform.position.x, transform.position.y - movespeed);
            p2.transform.position = new Vector3(p2.transform.position.x, p2.transform.position.y - movespeed);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            p1.transform.position = new Vector3(transform.position.x + movespeed, transform.position.y);
            p2.transform.position = new Vector3(p2.transform.position.x - movespeed, p2.transform.position.y);
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("standard-test");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "p2") {
            p1.SetActive(false);
            p2.SetActive(false);
            p3.SetActive(true);
            p3.GetComponent<MinigameEnding>().EndGame(true);
        }
    }

}
