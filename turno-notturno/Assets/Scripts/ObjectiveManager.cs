using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] windowBars;

    private GameObject objectivePredab;
    private Dictionary<string, GameObject> objectives;
    
    // Start is called before the first frame update
    void Start()
    {
        objectives = new Dictionary<string, GameObject>();
        objectivePredab = Resources.Load<GameObject>("Objective");
        Act1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set up objectives for act 1
    public void Act1()
    {
        // turn off the alarm should be the first
        NewObjective("window1", "Lock the windows", "Windows locked", windowBars.Length);
    }

    //Spawn new objective UI
    public void NewObjective(string name, string description, string progressText, int targetAmount)
    {
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, progressText, objectives.Count, targetAmount);
        objectives[name] = obj;
    }

    //Mark the objective UI as completed
    public void CompleteObjective(string name)
    {
        if(objectives.ContainsKey(name))
        {
            Objective objective = objectives[name].GetComponent<Objective>();
            objective.Complete();
        }
    }

    //One window was locked
    public void LockWindow()
    {
        string key = "window1";
        Objective objective = objectives[key].GetComponent<Objective>();
        windowBars[objective.GetProgress()].GetComponent<PlayableDirector>().enabled = true;
        if(objective.UpdateProgress(1))
        {
            CompleteObjective(key);
            NewObjective("door1", "Lock the doors", "Doors locked", 2);
        }
    }

    //One door was locked
    public void LockDoor()
    {
        string key = "door1";
        Objective objective = objectives[key].GetComponent<Objective>();
        if (objective.UpdateProgress(1))
        {
            CompleteObjective(key);
        }
    }
    

}
