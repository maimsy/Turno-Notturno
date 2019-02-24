using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
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
        NewObjective("window1", "Lock the windows");
    }

    public void NewObjective(string name, string description)
    {
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, objectives.Count);
        objectives[name] = obj;
    }

    public void CompleteObjective(string name)
    {
        if(objectives.ContainsKey(name))
        {
            Objective objective = objectives[name].GetComponent<Objective>();
            objective.Complete();
        }
    }

    
}
