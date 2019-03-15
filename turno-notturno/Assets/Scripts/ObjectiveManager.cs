using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectiveManager : MonoBehaviour
{
    private List<GameObject> windowBars;
    private List<string> multiObjectives;
    private GameObject objectivePredab;
    private Dictionary<string, Objective> objectives;
    private float delayTime = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        windowBars = new List<GameObject>();
        multiObjectives = new List<string>();
        if(GameObject.Find("windowBars1")) windowBars.Add(GameObject.Find("windowBars1"));
        if(GameObject.Find("windowBars2")) windowBars.Add(GameObject.Find("windowBars2"));
        objectives = new Dictionary<string, Objective>();
        objectivePredab = Resources.Load<GameObject>("Objective");
        Act1();
    }

    // set up objectives for act 1
    public void Act1()
    {
        PlayDialogue("01", 2f, abortPrevious: false);
        //PlayDialogue("02", 10f, abortPrevious: false);
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
        StartCoroutine(NewObjective("room1", "Find the cause of the alarm", 1, delayTime));
    }

    //Spawn new objective UI after animations
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
        yield return new WaitForSeconds(delayTime);
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
        if (objectives[name].UpdateProgress(1))
        {
            objectives[name].Complete();
            return true;
        }
        return false;
    }

    //Guard arrived to the first blinking room
    public void Room1()
    {
        if (UpdateProgress("room1"))
        {
            StartCoroutine(RemoveObjective("room1"));
            StartCoroutine(NewObjective("alarm1", "Turn off the alarm",  1, delayTime));
            StartCoroutine(NewObjective("artpiece1", "Find the art piece", 1, delayTime));
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
            PlayDialogue("05", 0f);
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
        StartCoroutine(NewObjective("window1", "Lock the windows", windowBars.Count, delayTime));
        StartCoroutine(NewObjective("door1", "Lock the main door", 1, delayTime));
        string[] names = { "window1", "door1" };
        MultiObjective(names);
        GameObject.Find("control_windows_03").GetComponent<Interactable>().isInteractable = true;
        GameObject.Find("control_windows_01").GetComponent<Interactable>().isInteractable = true;
        GameObject.Find("door_02_group").GetComponent<Interactable>().SetTooltip("check doors");
        GameObject.Find("door_03_group").GetComponent<Interactable>().SetTooltip("check doors");
    }

    //One window was locked
    public void LockWindow(int whichWindow)
    {
        if (whichWindow == 0) PlayDialogue("07", 0.5f);
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
            PlayDialogue("08", 0.5f);
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

        StartCoroutine(NewObjective("pills1", "Take some migraine pills", 1, migraineDelay + delayTime));
        PlayDialogue("09", migraineDelay);
        GameObject.Find("bottle_pill_01").GetComponent<Interactable>().isInteractable = true;
    }

    //player takes pills
    public void TakePills()
    {
        if (UpdateProgress("pills1"))
        {
            PlayDialogue("10", 0f);
            StartCoroutine(RemoveObjective("pills1"));
            //fall asleep for act 2 minigame 
            GameObject.Find("FadeOut").GetComponent<FadeIn>().enabled = true;
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
                dialogueMessage = "Ugh... what?";
                break;
            case "02":
                dialogueMessage = "Ugh...";
                break;
            case "03":
                dialogueMessage = "Ugh...";
                break;
            case "04":
                dialogueMessage = "What is this... an intruder?";
                break;
            case "05":
                dialogueMessage = "Shut up!";
                break;
            case "06":
                dialogueMessage = "This is a comment on the artwork";
                break;
            case "07":
                dialogueMessage = "God damn security always giving me extra work... *grumble*";
                break;
            case "08":
                dialogueMessage = "No one could have gotten in...";
                break;
            case "09":
                dialogueMessage = "Ugh...not again!";
                break;
            case "10":
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
