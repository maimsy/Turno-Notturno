using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private GameObject objectivePredab;
    private List<GameObject> objectives;
    // Start is called before the first frame update
    void Start()
    {
        objectives = new List<GameObject>();
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
        NewObjective("Collect clue from painting");
        NewObjective("Lock the windows");
    }

    public void NewObjective(string description)
    {
        GameObject obj = Instantiate(objectivePredab, transform);
        Objective objective = obj.GetComponent<Objective>();
        objective.SetUp(description, objectives.Count);
        objectives.Add(obj);
    }

}
