using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlinkingEvent : MonoBehaviour
{
    [SerializeField] float interval = 1f;
    [SerializeField] List<UnityEvent> events;

    private int index = 0;

    void Start()
    {
        InvokeRepeating("InvokeNextEvent", interval, interval);
    }

    public void InvokeNextEvent()
    {
        if (events.Count == 0) return;
        if (index + 1 > events.Count) index = 0;
        events[index++].Invoke();
    }
}
