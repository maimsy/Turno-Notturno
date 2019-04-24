﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject finalPuzzle;
    public GameObject Notes;
    private bool isBookOpen = false;

    [Serializable]
    public struct SceneStatePair
    {
        public GameState state;
        public string scene;
    }
    public List<SceneStatePair> scenes;

    private Player player;

    private UnityAction escapeActions;
    private bool paused = false;
    private bool enableControlsAfterPause = true;
    private bool controlsEnabled = true;
    private PauseMenu pauseMenu;
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        instance = FindObjectOfType<GameManager>();
        if (!instance)
        {
            GameObject obj = Resources.Load<GameObject>("GameManager");
            obj = Instantiate(obj);
            instance = obj.GetComponent<GameManager>();
        }
        return instance;
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
        //SetPlayerPosition();
    }

    void Start()
    {
        ActManager actManager = FindObjectOfType<ActManager>();
        if (actManager) actManager.SetUpAct(GetGameState());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (escapeActions != null)
            {
                escapeActions.Invoke();
                ClearEscapeActions();
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPaused(!paused);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenCloseBook();
        }
    }

    private void OpenCloseBook()
    {
        isBookOpen = !isBookOpen;
        Notes.SetActive(isBookOpen);
        if (isBookOpen) DisableControls();
        else EnableControls();
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void SetPaused(bool value)
    {
        paused = value;
        if (!pauseMenu) LoadPauseMenuPrefab();
        if (paused)
        {
            pauseMenu.gameObject.SetActive(true);
            enableControlsAfterPause = controlsEnabled;
            DisableControls();
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            if (enableControlsAfterPause) EnableControls();
            Time.timeScale = 1f;
        }
    }

    void LoadPauseMenuPrefab()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        if (!pauseMenu)
        {
            GameObject obj = Resources.Load<GameObject>("PauseMenuCanvas");
            obj = Instantiate(obj);
            pauseMenu = obj.GetComponentInChildren<PauseMenu>();
            pauseMenu.gameObject.SetActive(false);
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
        controlsEnabled = true;
        if (player)
        {
            player.HideCursor(true);
            player.enabled = true;
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    public void DisableControls()
    {
        controlsEnabled = false;
        if (player)
        {
            player.HideCursor(false);
            player.enabled = false;
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.isKinematic = true;
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
        if (!player) return;
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"),
                                                PlayerPrefs.GetFloat("PlayerPositionY"),
                                                PlayerPrefs.GetFloat("PlayerPositionZ"));
    }

    public void LoadNextAct()
    {
        // Increment game state by one and reload scene
        GameState state = GetGameState();
        int index = scenes.FindIndex(t => t.state == state);
        if (index + 1 < scenes.Count)
        {
            index++;
            SetGameState(scenes[index].state);
            LoadGame();
        }
        else
        {
            Debug.LogError("Cannot load next act! Already playing the final act!");
        }
    }

    public void SetGameState(GameState state)
    {
        PlayerPrefs.SetInt("GameState", (int)state);
    }

    public GameState GetGameState()
    {
        return (GameState)PlayerPrefs.GetInt("GameState", 0);
    }

    public void LoadGame()
    {
        // Loads the scene associated with the current GameState
        GameState state = GetGameState();
        Debug.Log("Loading " + state);
        foreach (SceneStatePair pair in scenes)
        {
            if (state == pair.state)
            {
                SceneManager.LoadScene(pair.scene);

                // Avoid possible bugs where the game starts paused or without cursor
                SetPaused(false);  
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
        }
        Debug.LogError("Attempted to load state " + state);
        Debug.LogError("Cannot load game! No scene found for current GameState!");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("GameState");

        PlayerPrefs.DeleteKey("ClueGuessed");

        PlayerPrefs.DeleteKey("ClueFoundAct11");
        PlayerPrefs.DeleteKey("ClueFoundAct12");
        PlayerPrefs.DeleteKey("ClueFoundAct13");
        PlayerPrefs.DeleteKey("ClueFoundAct14");

        PlayerPrefs.DeleteKey("ClueFoundAct21");
        PlayerPrefs.DeleteKey("ClueFoundAct22");
        PlayerPrefs.DeleteKey("ClueFoundAct23");
        PlayerPrefs.DeleteKey("ClueFoundAct24");

        PlayerPrefs.DeleteKey("ClueFoundAct31");
        PlayerPrefs.DeleteKey("ClueFoundAct32");
        PlayerPrefs.DeleteKey("ClueFoundAct33");
        PlayerPrefs.DeleteKey("ClueFoundAct34");

        PlayerPrefs.DeleteKey("ClueFoundAct41");
        PlayerPrefs.DeleteKey("ClueFoundAct42");
        PlayerPrefs.DeleteKey("ClueFoundAct43");
        PlayerPrefs.DeleteKey("ClueFoundAct44");
        PlayerPrefs.DeleteKey("ClueFoundAct51");
        PlayerPrefs.DeleteKey("ClueFoundAct52");
        PlayerPrefs.DeleteKey("ClueFoundAct53");
        PlayerPrefs.DeleteKey("ClueFoundAct54");


        PlayerPrefs.DeleteKey("ClueFoundAct61");
        PlayerPrefs.DeleteKey("ClueFoundAct62");
        PlayerPrefs.DeleteKey("ClueFoundAct63");
        PlayerPrefs.DeleteKey("ClueFoundAct64");

        LoadGame();
    }
}
