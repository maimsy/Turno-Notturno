using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool reloadGameOnContinue = false;  // Set to true to act as a placeholder for main menu
    public Dropdown actDropdown;
    private Slider mouseSlider;
    public InputField mouseInputField;

    private GameManager gameManager;
    private bool updatingMouseValues = false; // A workaround for infinite loop when updating mouse sensitivity slider/input

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        GenerateDropdown();
        mouseSlider = GetComponentInChildren<Slider>();
        mouseInputField = GetComponentInChildren<InputField>();

        mouseSlider.value = PlayerPrefs.GetFloat("MouseSensitivityX", 5);
        mouseInputField.text = PlayerPrefs.GetFloat("MouseSensitivityX", 5).ToString();

    }

    public void UpdateMouseSlider()
    {
        if (updatingMouseValues) return;
        updatingMouseValues = true;
        float value = mouseSlider.value;
        SetMouseSensitivity(value);
        mouseInputField.text = value.ToString(); //.Replace(",", "."); // Input field fails with decimal comma
        updatingMouseValues = false;
    }

    public void UpdateMouseInputField()
    {
        if (updatingMouseValues) return;
        updatingMouseValues = true;
        float value;
        try
        {
            value = float.Parse(mouseInputField.text);
        }
        catch (Exception e)
        {
            value = PlayerPrefs.GetFloat("MouseSensitivityX", 5);
            mouseInputField.text = value.ToString();
            Debug.Log(e);
        }
        
        SetMouseSensitivity(value);
        mouseSlider.value = value;
        updatingMouseValues = false;
    }

    void SetMouseSensitivity(float value)
    {   
        PlayerPrefs.SetFloat("MouseSensitivityX", value);
        PlayerPrefs.SetFloat("MouseSensitivityY", value);
        Player player = FindObjectOfType<Player>();
        if (player) player.UpdateMouseSensitivity();
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
