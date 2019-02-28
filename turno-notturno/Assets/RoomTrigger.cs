using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] bool triggersOnce;
    [SerializeField] List<UnityEvent> triggerEvents;
    private int nextEventIndex = 0;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PlayerCharacter")
        {
            if (!hasTriggered)
            {
                if (triggerEvents.Count == 0) return;
                if (nextEventIndex + 1 > triggerEvents.Count) nextEventIndex = 0;
                triggerEvents[nextEventIndex++].Invoke();
            }
            if (triggersOnce)
            {
                //hasTriggered = true;
                Destroy(gameObject);
            }
        }
    }
}
