using System;
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

        GameObject obj = GameObject.Find("RoomTrigger");
        if (obj) obj.GetComponent<BoxCollider>().enabled = true;
        else Debug.LogError("Room trigger is missing!");

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
        GameObject.Find("Act1MigraineTrigger").GetComponent<BoxCollider>().enabled = true;
    }

    // set up objectives for act 2
    private void Act2()
    {
        PlayDialogue("14", 2f, abortPrevious: false);
        PlayDialogue("15", 6f, abortPrevious: false);
        GameObject obj = GameObject.Find("WakeUpPosition1");
        if (obj)
        {
            FindObjectOfType<Player>().transform.position = obj.transform.position;
            FindObjectOfType<Player>().RotateTo(obj.transform.rotation);
        }
        else Debug.LogError("Could not find Wake up position for player!");
        obj = GameObject.Find("door_04_group");
        if (obj)
        {
            obj.GetComponent<Door>().locked = false;
            obj.GetComponent<Door>().UpdateTooltip();
        }
        else Debug.LogError("Could not find storage door!");
        obj = GameObject.Find("RoomTrigger2");
        if (obj)
        {
            obj.GetComponent<BoxCollider>().enabled = true;
        }
        else Debug.LogError("Could not find Room trigger for art room!");

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
            Invoke("Room1Objectives",delayTime);
            //FindObjectOfType<MigrainEffect>().StartMigrain();
            //GameObject.Find("alarm-box").GetComponent<Interactable>().isInteractable = true;
        }
    }
    private void Room1Objectives()
    {
        StartCoroutine(NewObjective("alarm1", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece1", "Find the cause of the alarm", 1, 0));
        string[] names = { "alarm1", "artpiece1" };
        MultiObjective(names);
        GameObject.Find("control_alarm_02").GetComponent<Interactable>().isInteractable = true;
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
            Invoke("AddClue1Objectives", delayTime);
        }
    }

    private void AddClue1Objectives()
    {
        StartCoroutine(NewObjective("clue1", "Inspect the artwork for damage", 4, 0));
        GameObject.Find("art_main_01_mesh").GetComponent<Painting>().EnableClues(true);
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
            StartCoroutine(RemoveObjective("clue1"));
            multiObjectives.Remove("clue1");
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
        Invoke("PillsInteractable", migraineDelay + delayTime);
    }
    private void PillsInteractable()
    {
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
            Invoke("Room2Objectives", delayTime);
        }
    }
    private void Room2Objectives()
    {

        StartCoroutine(NewObjective("alarm2", "Turn off the alarm", 1, 0));
        StartCoroutine(NewObjective("artpiece2", "Find the cause of the alarm", 2, delayTime));
        string[] names = { "alarm2", "artpiece2" };
        MultiObjective(names);
        GameObject.Find("control_alarm_04").GetComponent<Interactable>().isInteractable = true;
    }

    //player inspects one of the paintings
    public void InspectPainting2(int whichPainting)
    {
        if(!paintingsChecked[whichPainting])
        {
            if(paintingsChecked[0] == false && paintingsChecked[1] == false)
            {
                PlayDialogue("16", 1f, abortPrevious: false);
            }
            paintingsChecked[whichPainting] = true;
            if (UpdateProgress("artpiece2"))
            {
                //PlayDialogue("06", 0.5f);
                StartCoroutine(RemoveObjective("artpiece2"));
                multiObjectives.Remove("artpiece2");
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
            StartCoroutine(RemoveObjective("alarm2"));
            multiObjectives.Remove("alarm2");
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
        GameObject.Find("bleachFall").SetActive(false);
    }

    //player arrives to the storage room
    public void StorageRoom()
    {
        
        if (UpdateProgress("storage"))
        {
            StartCoroutine(RemoveObjective("storage"));
            PlayDialogue("18", 0.5f, abortPrevious: false);
            PlayDialogue("19", 6f, abortPrevious: false);
            StartCoroutine(FadeToNextScene(9f));
        }
    }

    private IEnumerator FadeToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Find("FadeOut").GetComponent<FadeIn>().enabled = true;
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
