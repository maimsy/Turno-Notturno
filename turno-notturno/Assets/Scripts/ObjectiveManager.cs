﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using FMODUnity;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class ObjectiveManager : MonoBehaviour
{
    
    private List<GameObject> windowBars;
    private List<string> multiObjectives;
    private bool[] paintingsChecked;
    private GameObject objectivePredab;
    private Dictionary<string, Objective> objectives;
    private float delayTime = 2;
    private FMOD.Studio.EventInstance currentVoiceLine;
    private FMOD.Studio.EventInstance currentWhisper; // Guard voice and whispers are allowed to overlap
    private int act2ArtVoiceline = 0;

    public enum ClueObjective
    {
        NotImplemented = 0,

        // Act 1
        SpinningCityDescription = 10, // Numbering is used to simplify checking for active objective
        SpinningCityMesh = 11,

        // Act 2
        ToothTreeTrunk = 21,
        ToothTreeBase = 22,
        ToothTreeTeeth = 23,
        ToothTreeDescription = 24,

        BallsyPortraitBalls = 31,
        BallsyPortraitSpiral = 32,
        BallsyPortraitShadow = 33,
        BallsyPortraitDescription = 34,

        // Act 3-1
        MouthRobotTeeth = 41,
        MouthRobotDescription = 42,

        PaintingRedSpiral = 51,
        PaintingPart2 = 52,

        // Act 3-2
        VideoPart1 = 61,
        VideoPart2 = 62,
        VideoPart3 = 63
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GetObject("Act2VoicelineTrigger");
        if (obj) obj.GetComponent<BoxCollider>().enabled = false;
        MouthArtWork robot = FindObjectOfType<MouthArtWork>();
        if (robot) robot.disabled = true;
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
            case 5:
                Act3();
                break;
        }
        

    }

    // set up objectives for act 1
    private void Act1()
    {
        windowBars = new List<GameObject>();
        GameObject obj = GetObject("windows_bars_art_room");
        if (obj) windowBars.Add(obj);
        obj = GetObject("windows_bars_shop");
        if (obj) windowBars.Add(obj);
        obj = GetObject("RoomTrigger");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
        obj = GetObject("Act1MigraineTrigger");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
        obj = GetObject("art_main_01_sculptre");
        if (obj) obj.GetComponentInChildren<Rotate>().StartRotation();
        SetMainDoorTooltip("Main doors");
        OpenWindows();

        PlayDialogue("01", 1f, abortPrevious: false);
        PlayDialogue("02", 3f, abortPrevious: false);
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

    private void OpenWindows()
    {
        foreach (WindowAnimator windowAnimator in FindObjectsOfType<WindowAnimator>())
        {
            windowAnimator.ForceOpen();
        }
    }

    private void CloseWindows()
    {
        foreach (WindowAnimator windowAnimator in FindObjectsOfType<WindowAnimator>())
        {
            windowAnimator.ForceClose();
        }
    }

    // set up objectives for act 2
    private void Act2()
    {
        PlayDialogue("13", 2f, abortPrevious: false);
        PlayDialogue("14", 7f, abortPrevious: false);
        StartCoroutine(NewObjective("room2", "Check the alarm", 1, delayTime));
        GameObject[] paintings = GameObject.FindGameObjectsWithTag("Droppable");
        float forceDirection = GetObject("GroundColliderPaintingRoom").transform.position.x;
        foreach(GameObject painting in paintings)
        {
            painting.AddComponent<Rigidbody>();
            Vector3 force = new Vector3(0, 0, 0);
            Vector3 pos = painting.transform.position;
            force.x = pos.x < forceDirection ? 1.5f : -1.5f;
            painting.GetComponent<Rigidbody>().AddForceAtPosition(force, pos, ForceMode.Impulse);
        }
        GameObject obj = GetObject("WakeUpPosition_Act2");
        if (obj)
        {
            FindObjectOfType<Player>().transform.position = obj.transform.position;
            FindObjectOfType<Player>().RotateTo(obj.transform.rotation);
        }
        obj = GetObject("door_04_group");
        if (obj)
        { 
            obj.GetComponent<Door>().locked = false;
        }
        obj = GetObject("RoomTrigger2");
        if(obj) obj.GetComponent<BoxCollider>().enabled = true;
        AlarmManager alarmManager = FindObjectOfType<AlarmManager>();
        if (alarmManager)
        {
            alarmManager.ActivateAlarm(AlarmManager.Act.act_2);
            alarmManager.ActivateAlarm(AlarmManager.Act.act_3);
        }
        else
        {
            Debug.LogError("Alarm manager is missing!");
        }
    }

    private void Act3()
    {
        MouthArtWork robot = FindObjectOfType<MouthArtWork>();
        if (robot) robot.disabled = false;
        PlayDialogue("25", 1f);
        PlayDialogue("26", 5f);
        StartCoroutine(NewObjective("room3", "Check the alarm", 1, delayTime));

        GameObject obj = GetObject("WakeUpPosition_Act3");
        if (obj)
        {
            FindObjectOfType<Player>().transform.position = obj.transform.position;
            FindObjectOfType<Player>().RotateTo(obj.transform.rotation);
        }
        obj = GetObject("RoomTrigger4");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
        obj = GetObject("door_04_group");
        if (obj)
        {
            obj.GetComponent<Door>().locked = false;
            obj.GetComponent<Door>().Open();
            //obj.GetComponent<Door>().UpdateTooltip();
        }
        Vector3 pos = Vector3.zero;
        obj = GetObject("FlashLightPos_Act3");
        if (obj) pos = obj.transform.position;
        obj = GetObject("flashlight_01");
        if (obj)
        {
            obj.transform.position = pos;
            //obj.GetComponent<Interactable>().isInteractable = true;
        }
        AlarmManager alarmManager = FindObjectOfType<AlarmManager>();
        if (alarmManager)
        {
            alarmManager.ActivateAlarm(AlarmManager.Act.act_3);
        }
        else
        {
            Debug.LogError("Alarm manager is missing!");
        }
    }

    //Spawn new objective UI after delay
    IEnumerator NewObjective(string name, string description, int targetAmount, float delay)
    {
        if(delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, objectives.Count, targetAmount);
        objectives[name] = obj.GetComponent<Objective>();
    }

    //Remove objective after animations
    private void RemoveObjective(string objectiveName)
    {
        objectives.Remove(objectiveName);
        FillGap();
    }

    //fills the empty space left by the removed objective
    private void FillGap()
    {
        List<int> nums = new List<int>();
        int gap = -1;
        int max = 0;
        foreach (Objective obj in objectives.Values)
        {
            int pos = (int)obj.GetPos();
            nums.Add(pos);
            max = pos > max ? pos : max;
        }
        for(int i = 0; i < max+1; i++)
        {
            if(!nums.Contains(i))
            {
                gap = i;
            }
        }

        foreach (Objective obj in objectives.Values)
        {

            float pos = obj.GetPos();
            if (pos > gap && gap != -1)
            {
                obj.StartMoving((int)pos - 1);
            }
        }
    }

    //multiple objectives
    private void MultiObjective(string[] names)
    {
        multiObjectives.Clear();
        foreach (string objectiveName in names)
        {
            multiObjectives.Add(objectiveName);
        }
        
    }

    public bool IsObjectiveActive(string objectiveName)
    {
        return objectives.ContainsKey(objectiveName);
    }

    public bool IsObjectiveActive(ClueObjective clueObjective)
    {
        string objectiveName = ClueToString(clueObjective);
        return objectives.ContainsKey(objectiveName);
    }

    public bool UpdateProgress(string objectiveName)
    {
        if(IsObjectiveActive(objectiveName))
        {
            if (objectives[objectiveName].UpdateProgress(1))
            {
                objectives[objectiveName].Complete();
                RemoveObjective(objectiveName);
                if(multiObjectives.Contains(objectiveName))
                {
                    multiObjectives.Remove(objectiveName);
                }
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
            //PlayDialogue("03", 0f);
            Invoke("Room1Objectives",delayTime);

        }
    }
    private void Room1Objectives()
    {
        StartCoroutine(NewObjective("alarm1", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece1", "Find the cause of the alarm", 1, 0));
        string[] names = { "alarm1", "artpiece1" };
        MultiObjective(names);
        //GameObject obj = GetObject("control_alarm_02");
        //if (obj) obj.GetComponent<Interactable>().isInteractable = true;
    }

    //Player turned off alarm
    public void TurnOffAlarm1()
    {
        if (UpdateProgress("alarm1"))
        {
            if (multiObjectives.Count == 0)
            {
                WhispersBeforeLocking();
            }
        } 
    }

    //player inspects the painting
    public void InspectPainting1()
    {
        if (UpdateProgress("artpiece1"))
        {
            PlayDialogue("04", 0.0f);
            Invoke("AddClue1Objectives", delayTime);
        }
    }

    private void AddClue1Objectives()
    {
        int amount = CountClues("clue1");
        StartCoroutine(NewObjective("clue1", "Inspect the artwork for clues", amount, 0));
        multiObjectives.Add("clue1");
    }

    private void AddClue2Objectives()
    {
        int amount = CountClues("clue2");
        StartCoroutine(NewObjective("clue2", "Inspect the artwork for clues", amount, 0));
        multiObjectives.Add("clue2");
    }

    private void AddClue3Objectives()
    {
        int amount = CountClues("clue3");
        StartCoroutine(NewObjective("clue3", "Inspect the artwork for clues", amount, 0));
        multiObjectives.Add("clue3");
    }

    private void AddClue4Objectives()
    {
        int amount = CountClues("clue4");
        StartCoroutine(NewObjective("clue4", "Inspect the artwork for clues", amount, 0));
        multiObjectives.Add("clue4");
    }

    private void AddClue5Objectives()
    {
        int amount = CountClues("clue5");
        StartCoroutine(NewObjective("clue5", "Inspect the artwork for clues", amount, 0));
        multiObjectives.Add("clue5");
    }

    private void AddClue6Objectives()
    {
        int amount = CountClues("clue6");
        StartCoroutine(NewObjective("clue6", "Inspect the artwork for clues", amount, 0));
    }

    private int CountClues(string objective)
    {
        int found = 0;
        foreach (Clue clue in FindObjectsOfType<Clue>())
        {
            if (clue.isActiveAndEnabled && objective == ClueToString(clue.objective))
            {
                found += 1;
            }
        }

        return found;
    }

    public string ClueToString(ClueObjective objective)
    {
        // Returns something between "clue1" and "clue6"
        int i = (Int16) objective;
        return "clue" + i.ToString()[0];
    }

    

    public void InspectCluesGlobal(ClueObjective objective)
    {
        // Act 1
        string s = ClueToString(objective);

        // Spinning city
        if (s == "clue1" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.SpinningCityMesh:
                    PlayerPrefs.SetInt("ClueFoundAct11", 1);
                    PlayDialogue("c01", 0f);
                    break;
                case ClueObjective.SpinningCityDescription:
                    PlayerPrefs.SetInt("ClueFoundAct12", 1);
                    PlayDialogue("c02", 0f);
                    break;
            }
            if (UpdateProgress(s) && multiObjectives.Count == 0)
            {
                PlayDialogue("c03", 2f, false);
                WhispersBeforeLocking();
            }
        }

        // Tooth-tree
        if (s == "clue2" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.ToothTreeBase:
                    PlayerPrefs.SetInt("ClueFoundAct21", 1);
                    PlayDialogue("c08", 0f);
                    break;
                case ClueObjective.ToothTreeDescription:
                    PlayerPrefs.SetInt("ClueFoundAct22", 1);
                    PlayDialogue("c10", 0f);
                    break;
                case ClueObjective.ToothTreeTeeth:
                    PlayerPrefs.SetInt("ClueFoundAct23", 1);
                    PlayDialogue("c07", 0f);
                    break;
                case ClueObjective.ToothTreeTrunk:
                    PlayerPrefs.SetInt("ClueFoundAct24", 1);
                    PlayDialogue("c09", 0f);
                    break;
            }
            UpdateProgress(s);
            if (multiObjectives.Count == 0)
            {
                StorageRoomSetUp();
            }
        }

        // Ballsy-portrait
        if (s == "clue3" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.BallsyPortraitDescription:
                    PlayerPrefs.SetInt("ClueFoundAct31", 1);
                    PlayDialogue("c06", 0f);
                    break;
                case ClueObjective.BallsyPortraitBalls:
                    PlayerPrefs.SetInt("ClueFoundAct32", 1);
                    PlayDialogue("c04", 0f);
                    break;
                case ClueObjective.BallsyPortraitShadow:
                    PlayerPrefs.SetInt("ClueFoundAct33", 1);
                    PlayDialogue("c05", 0f);
                    break;
                case ClueObjective.BallsyPortraitSpiral:
                    PlayerPrefs.SetInt("ClueFoundAct34", 1);
                    break;
            }
            UpdateProgress(s);
            if (multiObjectives.Count == 0)
            {
                StorageRoomSetUp();
            }
        }


        // Mouth-robot
        if (s == "clue4" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.MouthRobotTeeth:
                    PlayerPrefs.SetInt("ClueFoundAct41", 1);
                    PlayDialogue("c13", 0f);
                    break;
                case ClueObjective.MouthRobotDescription:
                    PlayerPrefs.SetInt("ClueFoundAct42", 1);
                    PlayDialogue("c14", 0f);
                    break;
            }
            UpdateProgress(s);
            if (multiObjectives.Count == 0)
            {
                Leave();

            }
        }


        // Painting
        if (s == "clue5" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.PaintingRedSpiral:
                    PlayerPrefs.SetInt("ClueFoundAct51", 1);
                    PlayDialogue("c12", 0f);
                    break;
                case ClueObjective.PaintingPart2:
                    PlayerPrefs.SetInt("ClueFoundAct52", 1);
                    PlayDialogue("c11", 0f);
                    break;
            }
            UpdateProgress(s);
            if (multiObjectives.Count == 0)
            {
                Leave();
            }
        }

        // Video-art
        if (s == "clue6" && IsObjectiveActive(s))
        {
            switch (objective)
            {
                case ClueObjective.VideoPart1:
                    PlayerPrefs.SetInt("ClueFoundAct61", 1);
                    PlayDialogue("c15", 0f);
                    break;
                case ClueObjective.VideoPart2:
                    PlayerPrefs.SetInt("ClueFoundAct62", 1);
                    break;
                case ClueObjective.VideoPart3:
                    PlayerPrefs.SetInt("ClueFoundAct63", 1);
                    break;
            }
            UpdateProgress(s);
            if (multiObjectives.Count == 0)
            {
                // TODO
            }
        }
    }

    public void WhispersBeforeLocking()
    {
        PlayDialogue("w01", 5f);
        Invoke("Locking", 10f);
    }

    //Start the locking objectives
    public void Locking()
    {
        PlayDialogue("06", 0f);
        StartCoroutine(NewObjective("window1", "Secure the windows", windowBars.Count, delayTime));
        StartCoroutine(NewObjective("door1", "Check the main door", 1, delayTime));
        string[] names = { "window1", "door1" };
        MultiObjective(names);
        SetMainDoorTooltip("Lock main doors");
        GameObject obj;
        //GameObject obj = GetObject("control_windows_01");
        //if (obj) obj.GetComponent<Interactable>().isInteractable = true;
        //obj = GetObject("control_windows_02");
        //if (obj) obj.GetComponent<Interactable>().isInteractable = true;
        //obj = GetObject("door_02_group");
        //if (obj) obj.GetComponent<Interactable>().SetTooltip("check doors");
        //obj = GetObject("door_03_group");
        //if (obj) obj.GetComponent<Interactable>().SetTooltip("check doors");
    }

    //One window was locked
    public void LockWindow(int whichWindow)
    {
        //if (whichWindow == 0) PlayDialogue("07", 0.5f);
        if (whichWindow == 1) PlayDialogue("09", 0.5f);
        windowBars[whichWindow].GetComponent<WindowAnimator>().SmoothClose();
        if (UpdateProgress("window1"))
        {
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
            SetMainDoorTooltip("");
            PlayDialogue("10", 0.5f);
            if (multiObjectives.Count == 0)
            {
                AddPillObjective();
            }
        }
    }

    void AddPillObjective()
    {
        float migraineDelay = 10f;
        MigrainEffect migraine = FindObjectOfType<MigrainEffect>();
        if (migraine) migraine.StartMigrainDelayed(migraineDelay);

        StartCoroutine(NewObjective("pills1", "Find migraine pills in guard room", 1, migraineDelay + delayTime));
        PlayDialogue("w02", migraineDelay);
        PlayDialogue("11", migraineDelay + 3f);
    }


    //player takes pills
    public void TakePills()
    {
        if (UpdateProgress("pills1"))
        {
            PlayDialogue("12", 0f);
            //fall asleep for act 2 minigame 
            GameObject obj = GetObject("FadeOut");
            if (obj) obj.GetComponent<FadeIn>().enabled = true;

        }
    }

    //ACT 2 OBJECTIVES

    //Guard arrived to the alarm room
    public void Room2()
    {
        if (UpdateProgress("room2"))
        {
            Invoke("Room2Objectives", delayTime);
        }
    }
    private void Room2Objectives()
    {

        StartCoroutine(NewObjective("alarm2", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece2", "Find the cause of the alarm", 2, delayTime));
        string[] names = { "alarm2", "artpiece2" };
        MultiObjective(names);
        GameObject obj = GetObject("control_alarm_04");
        //if (obj) obj.GetComponent<Interactable>().isInteractable = true;
    }

    //player inspects one of the paintings
    public void InspectPainting2(int whichPainting)
    {
        if(!paintingsChecked[whichPainting] && objectives.ContainsKey("artpiece2"))
        {
            if (whichPainting == 0)
            {
                PlayDialogue("w04", 0f, abortPrevious: false);
                AddClue2Objectives();
            }
            else
            {
                PlayDialogue("w03", 0f, abortPrevious: false);
                AddClue3Objectives();
            }

            // Order of players response to whispers should be the same regardless of which art is inspected first
            if (act2ArtVoiceline == 0)
            {
                PlayDialogue("17", 3f, abortPrevious: false);
                act2ArtVoiceline = 1;
            }
            else if (act2ArtVoiceline == 1)
            {
                PlayDialogue("18", 3f, abortPrevious: false);
            }

            paintingsChecked[whichPainting] = true;
            UpdateProgress("artpiece2");
        }
    }

    //Player turned off alarm
    public void TurnOffAlarm2()
    {
        if (UpdateProgress("alarm2"))
        {
            //PlayDialogue("04", 0f);
            if (multiObjectives.Count == 0)
            {
                StorageRoomSetUp();
            }
            //GameObject.Find("artpiece").GetComponent<Interactable>().isInteractable = true;
        }
    }

    private void StorageRoomSetUp()
    {
        GameObject obj = GetObject("door_04_group");
        if (obj)
        {
            // Open door immediately, so the player wont see it opening
            obj.GetComponent<Door>().Open();
        }
        StartCoroutine(NewObjective("storage", "Check the storage room", 1, 8f));
        Invoke("BleachFall", 6f);
    }

    private void BleachFall()
    {
        //Act2VoicelineTrigger
        GameObject obj = GetObject("bleachFall");
        if (obj) obj.GetComponent<StudioEventEmitter>().Play();
        obj = GetObject("Act2VoicelineTrigger");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
        PlayDialogue("19", 1f, abortPrevious: false);
        obj = GetObject("RoomTrigger2");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;  
    }

    //player arrives to the storage room
    public void StorageRoom()
    {
        
        if (UpdateProgress("storage"))
        {
            GameObject obj = GetObject("door_04_group");
            
            PlayDialogue("21", 0.5f, abortPrevious: false);
            PlayDialogue("w05", 1f, abortPrevious: false);
            if (obj)
            {
                obj.GetComponent<Door>().SlamClose();
                obj.GetComponent<Door>().locked = true;
            }
            Invoke("Dizzyness", 8f);
            PlayDialogue("23", 14f, abortPrevious: false);
            StartCoroutine(FadeToNextScene(20f));
            Invoke("StopDizzyness", 24f);
            PlayDialogue("w06", 20f, abortPrevious: false);
        }
    }

    private void Dizzyness()
    {
        FindObjectOfType<DizzyEffect1>().StartDizzy();
    }

    private void StopDizzyness()
    {
        FindObjectOfType<DizzyEffect1>().EndDizzy();
    }
    //Enters the middle area upstairs
    public void Room3()
    {
        if (UpdateProgress("room3"))
        {
            Invoke("Room3Objectives", delayTime);
        }
    }

    private void Room3Objectives()
    {
        StartCoroutine(NewObjective("alarm3", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece3", "Find the cause of the alarm", 2, delayTime));
        string[] names = { "alarm3", "artpiece3" };
        MultiObjective(names);
        GameObject obj = GetObject("control_alarm_05");
        //if(obj) obj.GetComponent<Interactable>().isInteractable = true;
    }

    public void TurnOffAlarm3()
    {
        if (UpdateProgress("alarm3"))
        {
            if (multiObjectives.Count == 0)
            {
                Leave();

            }
        }
    }


    //player inspects one of the paintings in act 3
    public void InspectPainting3(int whichPainting)
    {
        if (!paintingsChecked[whichPainting] && objectives.ContainsKey("artpiece3"))
        {
            if (whichPainting == 0)
            {
                PlayDialogue("w07", 1f, abortPrevious: false);
                PlayDialogue("28", 2f, abortPrevious: false);
                AddClue4Objectives();
            }
            if (whichPainting == 1)
            {
                PlayDialogue("w08", 1f, abortPrevious: false);
                AddClue5Objectives();
            }

            paintingsChecked[whichPainting] = true;
            UpdateProgress("artpiece3");
        }
    }

    private void Leave()
    {
        
        PlayDialogue("30", 3f, abortPrevious: false);
        //run
        PlayDialogue("w09", 4f, abortPrevious: false);
        Invoke("PanicSound", 4f);
        MigrainEffect migraine = FindObjectOfType<MigrainEffect>();
        if (migraine) migraine.StartMigrainDelayed(4f);
        
        //PlayDialogue("22", 6f, abortPrevious: false);
        SetMainDoorTooltip("Escape");
        StartCoroutine(NewObjective("leave", "Leave the museum", 1, 5f));
    }
    private void PanicSound()
    {
        GameObject sound = GetObject("PanicSound");
        if (sound) sound.GetComponent<StudioEventEmitter>().Play();
    }

    public void Leave2()
    {
        if(UpdateProgress("leave"))
        {
            PlayDialogue("32", 0.5f, abortPrevious: false);
            StartCoroutine(NewObjective("phone", "Use phone in guard room", 1, 1f));
            SetMainDoorTooltip("");
            
        }
    }
    //Player started using the phone
    public void Phone()
    {
        if (UpdateProgress("phone"))
        {
            GameObject sound = GetObject("PanicSound");
            if (sound) sound.GetComponent<SoundFader>().FadeAway(13f, "panicSoundVol");
            //phone sound
            PlayDialogue("35", 2f, abortPrevious: false);
            Invoke("FootStepStart", 4f);
            PlayDialogue("36", 7f, abortPrevious: false);
            //lights go out
            Invoke("StartStorm",8f);
            //phone dies because of batteries run out
            //HeartBeat sound from minigame comes back
            Invoke("StartVideo", 8f);
            //Disable notebook opening
            StartCoroutine(NewObjective("flashlight", "Get flashlight from storage room", 1, 8f));

        }
    }

    private void StartStorm()
    {
        FindObjectOfType<ThunderManager>().StartStorm();
    }

    private void FootStepStart()
    {
        FindObjectOfType<FootSteps>().StartSound();
        MigrainEffect migraine = FindObjectOfType<MigrainEffect>();
        if (migraine) migraine.EndMigrain();
    }

    private void StartVideo()
    {
        GameObject obj = GetObject("art_main_04_video");
        if (obj) obj.GetComponent<VideoPlayer>().enabled = true;
        GameObject[] sounds = GameObject.FindGameObjectsWithTag("VideoSound");
        foreach(GameObject sound in sounds)
        {
            sound.GetComponent<StudioEventEmitter>().Play();
        }
        obj = GetObject("notebook");
        GameObject pos = GetObject("NotebookPos");
        if (obj && pos) obj.transform.position = pos.transform.position;
        
    }

    public void FlashLight()
    {
        if (UpdateProgress("flashlight"))
        {
            GameObject obj = GetObject("RoomTrigger5");
            if (obj) obj.GetComponent<BoxCollider>().enabled = true; 
            StartCoroutine(NewObjective("room4", "Check the noise", 1, delayTime));
        }
     }

    public void Room4()
    {
        if (UpdateProgress("room4"))
        {
            StartCoroutine(NewObjective("artpiece4", "Analyze the artwork", 1, delayTime));
        }
    }

    public void InspectPainting4()
    {
        if (objectives.ContainsKey("artpiece4"))
        {

            if (UpdateProgress("artpiece4"))
            {
                PlayDialogue("w11", 2f, abortPrevious: false);
                AddClue6Objectives();
                StartCoroutine(NewObjective("notebook", "Check the notebook", 1, delayTime));
            }
        }
    }

    public void NoteBook()
    {
        if (objectives.ContainsKey("notebook"))
        {
            if (UpdateProgress("notebook"))
            {
                StartCoroutine(NewObjective("storage2", "Go to the storage room", 1, delayTime));
                GameObject obj = GetObject("RoomTrigger3");
                if (obj) obj.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    public void StorageRoom2()
    {
        if (objectives.ContainsKey("storage2"))
        {
            if (UpdateProgress("storage2"))
            {
                StartCoroutine(NewObjective("finish", "Finish the artwork", 1, delayTime));
            }
        }
    }

    private IEnumerator FadeToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject obj = GetObject("FadeOut");
        if(obj) obj.GetComponent<FadeIn>().enabled = true;
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
        filename = filename.ToLower();
        String dialogueMessage = "";
        switch (filename)
        {
            case "01":
                dialogueMessage = "... a dream?";
                break;
            case "02":
                dialogueMessage = "Wait… someone broke in?";
                break;
            case "03":
                dialogueMessage = "03"; // Empty in latest script
                break;
            case "04":
                dialogueMessage = "It’s not supposed to be on… What if it’s damaged";
                break;
            case "05":
                dialogueMessage = "05"; // Empty in latest script
                break;
            case "06":
                dialogueMessage = "What… what was that?\r\nA-anyways [stuttering], I should go secure the place";
                break;
            case "06a":
                dialogueMessage = "Comment on color";
                break;
            case "06b":
                dialogueMessage = "Comment on theme";
                break;
            case "06c":
                dialogueMessage = "Comment on material";
                break;
            case "06d":
                dialogueMessage = "Comment on spiral";
                break;
            case "07":
                dialogueMessage = "07"; // Empty in latest script
                break;
            case "08":
                dialogueMessage = "08"; // Empty in latest script
                break;
            case "09":
                dialogueMessage = "It’s weird only one room was activated. I guess whoever it was got scared by the alarm and fled?... what a weird thief";
                break;
            case "10":
                dialogueMessage = "I’m sure I locked the door when I arrived";
                break;
            case "11":
                dialogueMessage = "Ugh...what is happening!!";
                break;
            case "12":
                dialogueMessage = "*gulping sound*";
                break;

            /*************          ACT 2           *************/
            case "13":
                dialogueMessage = "Haah! What? What the fuck. (intimidated tone)\r\nWhy am I here? (low voice, breathing)";
                break;
            case "14":
                dialogueMessage = "Again?";
                break;
            case "15":
                dialogueMessage = "15"; // Empty in latest script
                break;
            case "16":
                dialogueMessage = "16"; // Empty in latest script
                break;
            case "17":
                dialogueMessage = "This isn’t funny...";
                break;
            case "18":
                dialogueMessage = "Stop… [shaking]";
                break;
            case "19":
                dialogueMessage = "What was that? It sounds like it came from downstairs";
                break;
            case "20":
                dialogueMessage = "That door wasn’t open before...";
                break;
            case "21":
                dialogueMessage = "Is that… (confused, little surprised)";
                break;
            case "22":
                dialogueMessage = "22"; // Empty in latest script
                break;
            case "23":
                dialogueMessage = "Stop, stop! Please stop! I can’t take it anymore. I’m sorry, I’m sorry, I’m sorry...\r\n...What is this smell…? Ammonia?\r\n";
                break;


            /*************          ACT 3           *************/
            case "24":
                dialogueMessage = "24"; // Empty in latest script
                break;
            case "25":
                dialogueMessage = "(heavy breathing) … Still having those weird dreams….";
                break;
            case "26":
                dialogueMessage = "What the hell. I’m tired of this.\r\n(shouting) Wherever you are, come out!!";
                break;
            case "27":
                dialogueMessage = "27"; // Empty in latest script
                break;
            case "28":
                dialogueMessage = "...";
                break;
            case "29":
                dialogueMessage = "29"; // Empty in latest script
                break;
            case "30":
                dialogueMessage = "*panicked breathing of the guard*";
                break;
            case "31":
                dialogueMessage = "31"; // Empty in latest script
                break;
            case "32":
                dialogueMessage = "Shit!";
                break;
            case "33":
                dialogueMessage = "Something different here too";
                break;
            case "34":
                dialogueMessage = "34"; // Empty in latest script
                break;
            case "35":
                dialogueMessage = "Hey, it’s me. There’s something weird going on. I need help.";
                break;
            case "36":
                dialogueMessage = "There’s someone here, hur-";
                break;


            /*************          CLUES           *************/
            case "c01":
                dialogueMessage = "A blue, spiraling cityscape";
                break;
            case "c02":
                dialogueMessage = "It’s made of wood";
                break;
            case "c03":
                dialogueMessage = "Nothing seems to be missing or broken";
                break;
            case "c04":
                dialogueMessage = "Red spheres in a spiral pattern";
                break;
            case "c05":
                dialogueMessage = "It’s a portrait";
                break;
            case "c06":
                dialogueMessage = "It's made of copper";
                break;
            case "c07":
                dialogueMessage = "Ugh. Teeth.";
                break;
            case "c08":
                dialogueMessage = "It’s all copper";
                break;
            case "c09":
                dialogueMessage = "I hate this green";
                break;
            case "c10":
                dialogueMessage = "From a famous abstract artist";
                break;
            case "c11":
                dialogueMessage = "Spirals forming an abstract scenery";
                break;
            case "c12":
                dialogueMessage = "This is red wood";
                break;
            case "c13":
                dialogueMessage = "A cityscape of teeth";
                break;
            case "c14":
                dialogueMessage = "Made of blue copper";
                break;
            case "c15":
                dialogueMessage = "Blue, abstract, wood...what is the point of all this?";
                break;

            /*************          Secondary clues (when inspecting clues again)           *************/
            case "c16":
                dialogueMessage = "I feel like the artworks are connected somehow. Maybe I should see them again.";
                break;
            case "c17":
                dialogueMessage = "This is made of copper.How did she make it blue?";
                break;
            case "c18":
                dialogueMessage = "Spirals.That almost seems to be a theme in the exhibition";
                break;
            case "c19":
                dialogueMessage = "Mother would have wanted her portrait to be abstract like this one";
                break;
            case "c20":
                dialogueMessage = "This kind of abstract work...it looks like the works my mother used to do";
                break;
            case "c21":
                dialogueMessage = "I remember the copper workshop.And all the dangerous equipment in there";
                break;
            case "c22":
                dialogueMessage = "There was a lot of copper in the storage as well";
                break;
            case "c23":
                dialogueMessage = "Spirals...if I could just remember…";
                break;
            case "c24":
                dialogueMessage = "This shade of blue reminds me of something";
                break;
            case "c25":
                dialogueMessage = "I used to know someone who used spirals in her work";
                break;



            /*************          WHISPERS           *************/
            case "w01":
                dialogueMessage = "One day I will take you to the highest skyscraper there is. " +
                                  "We will sleep on the rooftop under a blanket of stars. " +
                                  "Feel the world spinning. " +
                                  "Stare into the eternity of the Universe. " +
                                  "Like the freest man alive, without the worries of the world underneath us.";
                break;
            case "w02":
                dialogueMessage = "Will you come with me? Do you trust me? Let’s run away.";
                break;
            case "w03":
                dialogueMessage = "Your father was the kindest man I had ever met. I really, really miss him. I’m lucky to have you. You have the same eyes as him.";
                break;
            case "w04":
                dialogueMessage = "I told you many times, your teeth will rot and decay and blacken " +
                                  "and fill with worms and fall all over the floor if you don’t take care of them. " +
                                  "Now go wash your teeth. I will check when it’s time to sleep.";
                break;
            case "w05":
                dialogueMessage = "Oh my God, there you are!! Why were you hiding?!?!? Come back out and stop crying! " +
                                  "Don’t you ever dare hide from me again!\r\n(loving tone) I love you.";
                break;
            case "w06":
                dialogueMessage = "How could you.";
                break;
            case "w07":
                dialogueMessage = "Come inside";
                break;
            case "w08":
                dialogueMessage = "It’s coming to get you";
                break;
            case "w09":
                dialogueMessage = "Run";
                break;
            case "w10":
                dialogueMessage = "Do you remember what you’ve done?";
                break;
            case "w11":
                dialogueMessage = "They don’t understand.";
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
        Dialogue[] dialogues = FindObjectsOfType<Dialogue>();
        Dialogue normalDialogue = null;
        Dialogue whisperDialogue = null;
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.name == "DialogueText")
            {
                normalDialogue = dialogue;
            }
            else
            {
                whisperDialogue = dialogue;
            }
        }
        if (filename.StartsWith("w") || filename.StartsWith("W"))
        {
            // Whispers
            if (whisperDialogue && dialogueMessage.Length > 0)
            {
                dialogueMessage = filename + ": " + dialogueMessage;
                whisperDialogue.DisplayText(dialogueMessage);
            }
            filename = filename.ToUpper();
            currentWhisper.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentWhisper = FMODUnity.RuntimeManager.CreateInstance("event:/whispers/" + filename);
            currentWhisper.start();
        }
        else
        {
            // Normal
            if (normalDialogue && dialogueMessage.Length > 0)
            {
                normalDialogue.DisplayText(dialogueMessage);
            }
            currentVoiceLine.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            if (filename.StartsWith("c") || filename.StartsWith("C"))
            {
                filename = filename.ToUpper();
                currentVoiceLine = FMODUnity.RuntimeManager.CreateInstance("event:/clueSpeaks/" + filename);
            }
            else
            {
                currentVoiceLine = FMODUnity.RuntimeManager.CreateInstance("event:/placeholderSpeaks/" + filename + "_placeholder");
            }
            currentVoiceLine.start();
        }
    }


    public GameObject GetObject(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj)
        {
            return obj;
        }
        else
        {
            Debug.LogError(name + " is missing!");
            return null;
        }
    }

    private void SetMainDoorTooltip(string tooltip)
    {
        foreach (MainDoor door in FindObjectsOfType<MainDoor>())
        {
            door.forcedTooltip = tooltip;
        }
    }
}
