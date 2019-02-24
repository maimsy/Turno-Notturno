using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public void Lock()
    {
        FindObjectOfType<ObjectiveManager>().CompleteObjective("door1");
        //Go back to guard room
    }
}
