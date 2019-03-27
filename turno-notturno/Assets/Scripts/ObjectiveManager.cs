﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectiveManager : MonoBehaviour
{
    private List<GameObject> windowBars;
    private List<string> multiObjectives;
    private bool[] paintingsChecked;
    private GameObject objectivePredab;
    private Dictionary<string, Objective> objectives;
    private float delayTime = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("GameState", 3);
        paintingsChecked = new bool[2] { false, false };
        multiObjectives = new List<string>();
        objectives = new Dictionary<string, Objective>();
        objectivePredab = Resources.Load<GameObject>("Objective");
        int actNum = PlayerPrefs.GetInt("GameState", 0);
        switch (actNum)
        {
            case 1:
                Act1();
                break;
            case 3:
                Act2();
                break;
        }
        

    }

    // set up objectives for act 1
    private void Act1()
    {
        windowBars = new List<GameObject>();
        if (GameObject.Find("windowBars1")) windowBars.Add(GameObject.Find("windowBars1"));
        if (GameObject.Find("windowBars2")) windowBars.Add(GameObject.Find("windowBars2"));
        PlayDialogue("01", 2f, abortPrevious: false);
        PlayDialogue("02", 6f, abortPrevious: false);
        //PlayDialogue("03", 20f, abortPrevious: false);
        AlarmManager alarmManager = FindObjectOfType<AlarmManager>();
        if (alarmManager)
        {
            alarmManager.ActivateAlarm(AlarmManager.Act.act_1);
        }
        else
        {
            Debug.LogError("Alarm manager is missing!");
        }
        StartCoroutine(NewObjective("room1", "Check the alarm", 1, delayTime));
    }

    // set up objectives for act 2
    private void Act2()
    {
        FindObjectOfType<Player>().transform.position = GameObject.Find("WakeUpPosition1").transform.position;
        StartCoroutine(NewObjective("room2", "Check the alarm", 1, delayTime));
        AlarmManager alarmManager = FindObjectOfType<AlarmManager>();
        if (alarmManager)
        {
            alarmManager.ActivateAlarm(AlarmManager.Act.act_2);
        }
        else
        {
            Debug.LogError("Alarm manager is missing!");
        }
    }

    //Spawn new objective UI after delay
    IEnumerator NewObjective(string name, string description, int targetAmount, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, objectives.Count, targetAmount);
        objectives[name] = obj.GetComponent<Objective>();
    }

    //Remove objective after animations
    IEnumerator RemoveObjective(string name)
    {
        yield return new WaitForSeconds(0);
        objectives.Remove(name);
    }

    //multiple objectives
    private void MultiObjective(string[] names)
    {
        multiObjectives.Clear();
        foreach (string name in names)
        {
            multiObjectives.Add(name);
        }
        
    }

    public bool UpdateProgress(string name)
    {
        if(objectives.ContainsKey(name)){
            if (objectives[name].UpdateProgress(1))
            {
                objectives[name].Complete();
                return true;
            }
        }
        return false;
    }

    //Guard arrived to the first blinking room
    public void Room1()
    {
        if (UpdateProgress("room1"))
        {
            PlayDialogue("03", 0f);
            StartCoroutine(RemoveObjective("room1"));
            StartCoroutine(NewObjective("alarm1", "Turn off the alarm",  1, 0));
            StartCoroutine(NewObjective("artpiece1", "Find the cause of the alarm", 1, delayTime));
            string[] names = { "alarm1", "artpiece1" };
            MultiObjective(names);
            //FindObjectOfType<MigrainEffect>().StartMigrain();
            //GameObject.Find("alarm-box").GetComponent<Interactable>().isInteractable = true;
        }
    }

    //Player turned off alarm
    public void TurnOffAlarm1()
    {
        if (UpdateProgress("alarm1"))
        {
            PlayDialogue("04", 0f);
            StartCoroutine(RemoveObjective("alarm1"));
            multiObjectives.Remove("alarm1");
            if (multiObjectives.Count == 0)
            {
                Locking();
            }
            //GameObject.Find("artpiece").GetComponent<Interactable>().isInteractable = true;
        } 
    }

    //player inspects the painting
    public void InspectPainting1()
    {
        if (UpdateProgress("artpiece1"))
        {
            PlayDialogue("06", 0.5f);
            StartCoroutine(RemoveObjective("artpiece1"));
            multiObjectives.Remove("artpiece1");
            if (multiObjectives.Count == 0)
            {
                Locking();
            }
            //FindObjectOfType<MigrainEffect>().EndMigrain();
        }
    }

    //Start the locking objectives
    public void Locking()
    {
        StartCoroutine(NewObjective("window1", "Secure the windows", windowBars.Count, delayTime));
        StartCoroutine(NewObjective("door1", "Check the main door", 1, delayTime));
        string[] names = { "window1", "door1" };
        MultiObjective(names);
        GameObject.Find("control_windows_01").GetComponent<Interactable>().isInteractable = true;
        GameObject.Find("control_windows_02").GetComponent<Interactable>().isInteractable = true;
        GameObject.Find("door_02_group").GetComponent<Interactable>().SetTooltip("check doors");
        GameObject.Find("door_03_group").GetComponent<Interactable>().SetTooltip("check doors");
    }

    //One window was locked
    public void LockWindow(int whichWindow)
    {
        if (whichWindow == 0) PlayDialogue("07", 0.5f);
        if (whichWindow == 1) PlayDialogue("09", 0.5f);
        windowBars[whichWindow].GetComponent<Animator>().enabled = true;
        if (UpdateProgress("window1"))
        {
            StartCoroutine(RemoveObjective("window1"));
            multiObjectives.Remove("window1");
            if (multiObjectives.Count == 0)
            {
                AddPillObjective();
            }
        }
    }

    //One door was locked
    public void LockDoor()
    {
        if (UpdateProgress("door1"))
        {
            PlayDialogue("10", 0.5f);
            StartCoroutine(RemoveObjective("door1"));
            multiObjectives.Remove("door1");
            if (multiObjectives.Count == 0)
            {
                AddPillObjective();
            }
            else
            {
                
            }
        }
    }

    void AddPillObjective()
    {
        float migraineDelay = 6f;
        MigrainEffect migraine = FindObjectOfType<MigrainEffect>();
        if (migraine) migraine.StartMigrainDelayed(migraineDelay);

        StartCoroutine(NewObjective("pills1", "Find migraine pills in guard room", 1, migraineDelay + delayTime));
        PlayDialogue("11", migraineDelay);
        PlayDialogue("12", migraineDelay + 3f);
        GameObject.Find("bottle_pill_01").GetComponent<Interactable>().isInteractable = true;
    }

    //player takes pills
    public void TakePills()
    {
        if (UpdateProgress("pills1"))
        {
            PlayDialogue("13", 0f);
            StartCoroutine(RemoveObjective("pills1"));
            //fall asleep for act 2 minigame 
            GameObject.Find("FadeOut").GetComponent<FadeIn>().enabled = true;
        }
    }

    //ACT 2 OBJECTIVES

    //Guard arrived to the alarm room
    public void Room2()
    {
        if (UpdateProgress("room2"))
        {
            StartCoroutine(RemoveObjective("room2"));
            StartCoroutine(NewObjective("alarm2", "Turn off the alarm", 1, 0));
            StartCoroutine(NewObjective("artpiece2", "Find the cause of the alarm", 2, delayTime));
            string[] names = { "alarm2", "artpiece2" };
            MultiObjective(names);
        }
    }

    //player inspects one of the paintings
    public void InspectPainting2(int whichPainting)
    {
        if(!paintingsChecked[whichPainting])
        {
            paintingsChecked[whichPainting] = true;
            if (UpdateProgress("artpiece2"))
            {
                //PlayDialogue("06", 0.5f);
                StartCoroutine(RemoveObjective("artpiece2"));
                multiObjectives.Remove("artpiece1");
                if (multiObjectives.Count == 0)
                {
                    Locking();
                }
            }
        }
    }

    //Player turned off alarm
    public void TurnOffAlarm2()
    {
        if (UpdateProgress("alarm2"))
        {
            //PlayDialogue("04", 0f);
            StartCoroutine(RemoveObjective("alarm2"));
            multiObjectives.Remove("alarm2");
            if (multiObjectives.Count == 0)
            {
                Locking();
            }
            //GameObject.Find("artpiece").GetComponent<Interactable>().isInteractable = true;
        }
    }

    public void AbortDialogue()
    {
        // Used to prevent overlapping dialogues
        StopCoroutine("DelayedVoiceline");
    }

    public void PlayDialogue(String filename)
    {
        PlayDialogue(filename, 0f);
    }

    public void PlayDialogue(String filename, float delay, bool abortPrevious=true)
    {
        if (abortPrevious) AbortDialogue();
        String dialogueMessage = "";
        switch (filename)
        {
            case "01":
                dialogueMessage = "Ugh...what the...?";
                break;
            case "02":
                dialogueMessage = "Why the hell is the alarm on…? Did someone break in?";
                break;
            case "03":
                dialogueMessage = "Uhh, my head is gonna split open from this bloody sound!";
                break;
            case "04":
                dialogueMessage = "Finally. What could have caused this?";
                break;
            case "05":
                dialogueMessage = "Hngggh…";
                break;
            case "06":
                dialogueMessage = "Someone’s been messing with this. It’s not supposed to be on";
                break;
            case "07":
                dialogueMessage = "Why would you break in and not steal the art? Such a waste of fine criminal activity";
                break;
            case "08":
                dialogueMessage = "I guess I should check the doors and windows.";
                break;
            case "09":
                dialogueMessage = "Why did they only mess that one artwork...it’s not like it’s that great.";
                break;
            case "10":
                dialogueMessage = "Why is the door open? I’m sure I locked it earlier.";
                break;
            case "11":
                dialogueMessage = "Ugh...not again!";
                break;
            case "12":
                dialogueMessage = "Where did I leave my pills…?";
                break;
            case "13":
                dialogueMessage = "*gulping sound for eating pills*";
                break;
            default:
                Debug.LogError("Invalid voiceline: " + filename);
                break;
        }
        IEnumerator delayedCoroutine = DelayedVoiceline(dialogueMessage, filename, delay);
        StartCoroutine(delayedCoroutine);
    }

    IEnumerator DelayedVoiceline(String dialogueMessage, String filename, float delay)
    {
        yield return new WaitForSeconds(delay);
        Dialogue dialogue = FindObjectOfType<Dialogue>();
        if (dialogue)
        {
            dialogue.DisplayText(dialogueMessage);
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/placeholderSpeaks/" + filename + "_placeholder");
    }
}
