using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using FMODUnity;

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

        foreach (GameObject windowBar in windowBars)
        {
            Animator animator = windowBar.GetComponent<Animator>();
            if (animator) animator.Play("ForceOpen");
        }

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
        PlayDialogue("14", 2f, abortPrevious: false);
        PlayDialogue("15", 6f, abortPrevious: false);
        StartCoroutine(NewObjective("room2", "Check the alarm", 1, delayTime));

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
            obj.GetComponent<Door>().UpdateTooltip();
        }
        obj = GetObject("RoomTrigger2");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
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

    private void Act3()
    {
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
            obj.GetComponent<Door>().UpdateTooltip();
        }
        Vector3 pos = Vector3.zero;
        obj = GetObject("FlashLightPos_Act3");
        if (obj) pos = obj.transform.position;
        obj = GetObject("flashlight_01");
        if (obj)
        {
            obj.transform.position = pos;
            obj.GetComponent<Interactable>().isInteractable = true;
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
                StartCoroutine(RemoveObjective(name));
                objectives[name].Complete();
                if(multiObjectives.Contains(name))
                {
                    multiObjectives.Remove(name);
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
            PlayDialogue("03", 0f);
            Invoke("Room1Objectives",delayTime);

        }
    }
    private void Room1Objectives()
    {
        StartCoroutine(NewObjective("alarm1", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece1", "Find the cause of the alarm", 1, 0));
        string[] names = { "alarm1", "artpiece1" };
        MultiObjective(names);
        GameObject obj = GetObject("control_alarm_02");
        if (obj) obj.GetComponent<Interactable>().isInteractable = true;
    }

    //Player turned off alarm
    public void TurnOffAlarm1()
    {
        if (UpdateProgress("alarm1"))
        {
            PlayDialogue("04", 0f);
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
            Invoke("AddClue1Objectives", delayTime);
        }
    }

    private void AddClue1Objectives()
    {
        StartCoroutine(NewObjective("clue1", "Inspect the artwork for damage", 4, 0));
        GameObject obj = GetObject("art_main_01_mesh");
        if (obj) obj.GetComponent<Painting>().EnableClues(true);
    }

    public void InspectClues1(int propertyType)
    {
        PropertyType property = (PropertyType)propertyType;
        if (property == PropertyType.Color) PlayDialogue("06a", 0.5f);
        if (property == PropertyType.Theme) PlayDialogue("06b", 0.5f);
        if (property == PropertyType.Material) PlayDialogue("06c", 0.5f);
        if (property == PropertyType.Special) PlayDialogue("06d", 0.5f);
        if (UpdateProgress("clue1"))
        {
            if (multiObjectives.Count == 0)
            {
                Locking();
            }
        }
    }

    //Start the locking objectives
    public void Locking()
    {
        StartCoroutine(NewObjective("window1", "Secure the windows", windowBars.Count, delayTime));
        StartCoroutine(NewObjective("door1", "Check the main door", 1, delayTime));
        string[] names = { "window1", "door1" };
        MultiObjective(names);
        GameObject obj = GetObject("control_windows_01");
        if (obj) obj.GetComponent<Interactable>().isInteractable = true;
        obj = GetObject("control_windows_02");
        if (obj) obj.GetComponent<Interactable>().isInteractable = true;
        obj = GetObject("door_02_group");
        if (obj) obj.GetComponent<Interactable>().SetTooltip("check doors");
        obj = GetObject("door_03_group");
        if (obj) obj.GetComponent<Interactable>().SetTooltip("check doors");
    }

    //One window was locked
    public void LockWindow(int whichWindow)
    {
        if (whichWindow == 0) PlayDialogue("07", 0.5f);
        if (whichWindow == 1) PlayDialogue("09", 0.5f);
        windowBars[whichWindow].GetComponent<Animator>().Play("Close");
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
            PlayDialogue("10", 0.5f);
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
        Invoke("PillsInteractable", migraineDelay + delayTime);
    }
    private void PillsInteractable()
    {
        GameObject obj = GetObject("bottle_pill_01");
        if (obj)
            obj.GetComponent<Interactable>().isInteractable = true;
    }

    //player takes pills
    public void TakePills()
    {
        if (UpdateProgress("pills1"))
        {
            PlayDialogue("13", 0f);
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
        if (obj) obj.GetComponent<Interactable>().isInteractable = true;
    }

    //player inspects one of the paintings
    public void InspectPainting2(int whichPainting)
    {
        if(!paintingsChecked[whichPainting] && objectives.ContainsKey("artpiece2"))
        {
            if(paintingsChecked[0] == false && paintingsChecked[1] == false)
            {
                PlayDialogue("16", 1f, abortPrevious: false);
            }
            paintingsChecked[whichPainting] = true;
            if (UpdateProgress("artpiece2"))
            {
                //PlayDialogue("06", 0.5f);
                if (multiObjectives.Count == 0)
                {
                    StorageRoomSetUp();
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
            if (multiObjectives.Count == 0)
            {
                StorageRoomSetUp();
            }
            //GameObject.Find("artpiece").GetComponent<Interactable>().isInteractable = true;
        }
    }

    private void StorageRoomSetUp()
    {
        StartCoroutine(NewObjective("storage", "Check the storage room", 1, 5f));
        PlayDialogue("17", 3f, abortPrevious: false);
        Invoke("BleachFall", 2f);
    }

    private void BleachFall()
    {
        GameObject obj = GetObject("bleachFall");
        if (obj) obj.GetComponent<StudioEventEmitter>().Play();
    }

    //player arrives to the storage room
    public void StorageRoom()
    {
        
        if (UpdateProgress("storage"))
        {
            PlayDialogue("18", 0.5f, abortPrevious: false);
            PlayDialogue("19", 6f, abortPrevious: false);
            StartCoroutine(FadeToNextScene(9f));
        }
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
        if(obj) obj.GetComponent<Interactable>().isInteractable = true;
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
            if (paintingsChecked[0] == false && paintingsChecked[1] == false)
            {
                PlayDialogue("20", 1f, abortPrevious: false);
            }
            paintingsChecked[whichPainting] = true;
            if (UpdateProgress("artpiece3"))
            {
                if (multiObjectives.Count == 0)
                {
                    Leave();
                }
            }
        }
    }

    private void Leave()
    {
        PlayDialogue("20", 1f, abortPrevious: false);
        PlayDialogue("21", 3f, abortPrevious: false);
        //panic breathing effect after the dialogue
        PlayDialogue("22", 6f, abortPrevious: false);
        GameObject obj = GetObject("door_02_group");
        if (obj) obj.GetComponent<Interactable>().SetTooltip("Escape");
        obj = GetObject("door_03_group");
        if (obj) obj.GetComponent<Interactable>().SetTooltip("Escape");
        StartCoroutine(NewObjective("leave", "Leave the museum", 1, 6f));
    }

    public void Leave2()
    {
        if(UpdateProgress("leave"))
        {
            PlayDialogue("23", 0f, abortPrevious: false);
            StartCoroutine(NewObjective("phone", "Use phone in guard room", 1, 1f));
        }
    }
    //Player started using the phone
    public void Phone()
    {
        if (UpdateProgress("phone"))
        {
            //phone sound
            PlayDialogue("24", 2f, abortPrevious: false);
            //play footsteps approaching sound
            PlayDialogue("25", 4f, abortPrevious: false);
            //Lights go out
            //phone dies because of batteries run out
            //HeartBeat sound from minigame comes back
            //Video installation sound starts playing
            //Disable notebook opening
            StartCoroutine(NewObjective("flashlight", "Get flashlight from storage room", 1, 6f));

        }
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
            case "14":
                dialogueMessage = "Haah! What? Where am I?";
                break;
            case "15":
                dialogueMessage = "What the fuck.";
                break;
            case "16":
                dialogueMessage = "So there is someone in here";
                break;
            case "17":
                dialogueMessage = "Gotcha. I'll teach you not to mess with me.";
                break;
            case "18":
                dialogueMessage = "Where are you hiding you little rat...";
                break;
            case "19":
                dialogueMessage = "What is this smell?";
                break;
            case "20":
                dialogueMessage = "Whispers";
                break;
            case "21":
                dialogueMessage = "Unknown voice: It's coming to get you";
                break;
            case "22":
                dialogueMessage = "Unknown voice: Run";
                break;
            case "23":
                dialogueMessage = "Shit";
                break;
            case "24":
                dialogueMessage = "Hey, it's me. There's something weird going on. I need help";
                break;
            case "25":
                dialogueMessage = "There's someone here, hur-";
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
}
