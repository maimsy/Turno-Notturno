using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActManager : MonoBehaviour
{
    [Serializable]
    public struct StateEventPair
    {
        public GameState state;
        public UnityEvent events;
    }
    public List<StateEventPair> acts;

    public void SetUpAct(GameState state)
    {
        Debug.Log("Loading act: " + state);
        foreach (StateEventPair eventPair in acts)
        {
            if (eventPair.state == state)
            {
                eventPair.events.Invoke();
                return;
            }
        }
    }
}
