using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectiveManager : MonoBehaviour
{
    private List<GameObject> windowBars;
    private GameObject objectivePredab;
    private Dictionary<string, Objective> objectives;
    
    // Start is called before the first frame update
    void Start()
    {
        windowBars = new List<GameObject>();
        if(GameObject.Find("windowBars1")) windowBars.Add(GameObject.Find("windowBars1"));
        if(GameObject.Find("windowBars2")) windowBars.Add(GameObject.Find("windowBars2"));
        foreach(GameObject obj in windowBars)
        {
            obj.SetActive(false);
        }
        objectives = new Dictionary<string, Objective>();
        objectivePredab = Resources.Load<GameObject>("Objective");
        Act1();
    }

    // set up objectives for act 1
    public void Act1()
    {
        NewObjective("room1", "Go to the blinking room", "Done", 1);
    }

    //Spawn new objective UI
    public void NewObjective(string name, string description, string progressText, int targetAmount)
    {
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, progressText, objectives.Count, targetAmount);
        objectives[name] = obj.GetComponent<Objective>();
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
        if(UpdateProgress("room1"))
            NewObjective("alarm1", "Turn off the alarm", "Alarms turned off", 1);
    }

    //Player turned off alarm
    public void TurnOffAlarm1()
    {
        if (UpdateProgress("alarm1"))
            NewObjective("artpiece1", "Find the art piece", "Art pieces found", 1);
    }

    //player inspects the painting
    public void InspectPainting1()
    {
        if (UpdateProgress("artpiece1"))
            NewObjective("window1", "Lock the windows", "Windows locked", windowBars.Count);
    }

    //One window was locked
    public void LockWindow()
    {
        windowBars[objectives["window1"].GetComponent<Objective>().GetProgress()].GetComponent<PlayableDirector>().enabled = true;
        if (UpdateProgress("window1"))
            NewObjective("door1", "Lock the doors", "Doors locked", 2);
    }

    //One door was locked
    public void LockDoor()
    {
        if (UpdateProgress("door1"))
            NewObjective("pills1", "Find some migraine pills", "Portions of pills taken", 1);
    }
    

}
