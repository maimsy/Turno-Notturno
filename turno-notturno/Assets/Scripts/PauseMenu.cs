using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool reloadGameOnContinue = false;  // Set to true to act as a placeholder for main menu
    public Dropdown actDropdown;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        GenerateDropdown();
    }

    public void Continue()
    {
        if (reloadGameOnContinue) { gameManager.LoadGame(); }
        else { gameManager.SetPaused(false); }
    }

    public void NewGame()
    {
        gameManager.NewGame();
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void ChangeAct()
    {
        string selected = actDropdown.options[actDropdown.value].text;
        GameState state = (GameState)System.Enum.Parse(typeof(GameState), selected);
        gameManager.SetGameState(state);
        gameManager.LoadGame();
    }

    void GenerateDropdown()
    {
        actDropdown.ClearOptions();
        string[] states = System.Enum.GetNames(typeof(GameState));
        actDropdown.AddOptions(new List<string>(states));

        // Select the current state for convenience
        GameState currentState = gameManager.GetGameState();
        for (int i = 0; i < states.Length; i++)
        {
            string name = states[i];
            if (name == currentState.ToString())
            {
                actDropdown.value = i;
                return;
            }
        }
    }
}
