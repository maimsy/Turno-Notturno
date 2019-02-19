using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject finalPuzzle;

    [Serializable]
    public struct SceneStatePair
    {
        public GameState state;
        public string scene;
    }
    public List<SceneStatePair> scenes;

    private Player player;

    private UnityAction escapeActions;

    public static GameManager GetInstance()
    {
        GameManager instance = FindObjectOfType<GameManager>();
        if (!instance)
        {
            GameObject obj = Resources.Load<GameObject>("GameManager");
            instance = obj.GetComponent<GameManager>();
        }
        return instance;
    }

    void Awake()
    {
        player = FindObjectOfType<Player>();
        SetPlayerPosition();
    }

    void Start()
    {
        LoadActSpecificElements();
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
        foreach (SceneStatePair pair in scenes)
        {
            if (state == pair.state)
            {
                SceneManager.LoadScene(pair.scene);
                return;
            }
        }
        Debug.LogError("Attempted to load " + state);
        Debug.LogError("Cannot load game! No scene found for current GameState!");
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        LoadGame();
    }

    public void LoadActSpecificElements()
    {

    }
}
