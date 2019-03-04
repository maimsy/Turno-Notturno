﻿using System.Collections;
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
        StartCoroutine(NewObjective("room1", "Go to the blinking room", 1, delayTime));
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
            //GameObject.Find("alarm-box").GetComponent<Interactable>().isInteractable = true;
        }
    }

    //Player turned off alarm
    public void TurnOffAlarm1()
    {
        if (UpdateProgress("alarm1"))
        {
            StartCoroutine(RemoveObjective("alarm1"));
            StartCoroutine(NewObjective("artpiece1", "Find the art piece",  1, delayTime));
            //GameObject.Find("artpiece").GetComponent<Interactable>().isInteractable = true;
        } 
    }

    //player inspects the painting
    public void InspectPainting1()
    {
        if (UpdateProgress("artpiece1"))
        {
            StartCoroutine(RemoveObjective("artpiece1"));
            StartCoroutine(NewObjective("window1", "Lock the windows", windowBars.Count, delayTime));
            StartCoroutine(NewObjective("door1", "Lock the main door", 1, delayTime));
            //GameObject.Find("window1").GetComponent<Interactable>().isInteractable = true;
            //GameObject.Find("window2").GetComponent<Interactable>().isInteractable = true;
            //GameObject.Find("maindoor").GetComponent<Interactable>().isInteractable = true;
            string[] names = { "window1", "door1" };
            MultiObjective(names);
            
        }
    }

    //One window was locked
    public void LockWindow()
    {
        windowBars[objectives["window1"].GetComponent<Objective>().GetProgress()].GetComponent<PlayableDirector>().enabled = true;
        if (UpdateProgress("window1"))
        {
            StartCoroutine(RemoveObjective("window1"));
            multiObjectives.Remove("window1");
            if(multiObjectives.Count == 0)
            {
                StartCoroutine(NewObjective("pills1", "Take some migraine pills", 1, delayTime));
                //GameObject.Find("pills").GetComponent<Interactable>().isInteractable = true;
            }
        }
    }

    //One door was locked
    public void LockDoor()
    {
        if (UpdateProgress("door1"))
        {
            StartCoroutine(RemoveObjective("door1"));
            multiObjectives.Remove("door1");
            if (multiObjectives.Count == 0)
            {
                StartCoroutine(NewObjective("pills1", "Take some migraine pills", 1, delayTime));
                //GameObject.Find("pills").GetComponent<Interactable>().isInteractable = true;
            }
        }
    }

    //player takes pills
    public void TakePills()
    {
        if (UpdateProgress("pills1"))
        {
            StartCoroutine(RemoveObjective("pills1"));
            //fall asleep for act 2 minigame
        }
    }


}
