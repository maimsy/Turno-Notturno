using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject finalPuzzle;

    private Player player;

    private UnityAction escapeActions;


    void Awake()
    {
        player = FindObjectOfType<Player>();
        SetPlayerPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeActions != null)
            {
                escapeActions.Invoke();
                ClearEscapeActions();
                
            }
        }
    }

    void ClearEscapeActions()
    {
        /*foreach (UnityAction action in escapeActions.GetInvocationList())
        {
            escapeActions -= action;
        }*/
    }

    public void EnableControls()
    {
        if (player)
        {
            player.HideCursor(true);
            player.enabled = true;
            player.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void DisableControls()
    {
        if (player)
        {
            player.HideCursor(false);
            player.enabled = false;
            player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void HideCursor()
    {
        if (player) player.HideCursor(true);
    }


    public void HideFinalPuzzle()
    {
        finalPuzzle.SetActive(false);
    }

    public void ShowFinalPuzzle()
    {
        DisableControls();
        finalPuzzle.SetActive(true);

        ClearEscapeActions();
        escapeActions = EnableControls;
        escapeActions += HideFinalPuzzle;
    }

    public void IncrementDiscoveredClues()
    {
        int val = GetCluesFound() + 1;
        PlayerPrefs.SetInt("CluesFound", val);
    }

    public int GetCluesFound()
    {
        return PlayerPrefs.GetInt("CluesFound", 0);
    }

    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", player.transform.position.z);
    }

    public void SetPlayerPosition()
    {
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"),
            PlayerPrefs.GetFloat("PlayerPositionY"),
            PlayerPrefs.GetFloat("PlayerPositionZ"));
    }
}
