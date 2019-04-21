using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MinigameSound : MonoBehaviour
{
    private bool stop = false;
    private int act = 0;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartAlarmSound();
    }
    private void Update()
    {
        if(stop)
        {
            AlarmManager manager = FindObjectOfType<AlarmManager>();
            StudioEventEmitter emitter;
            switch (act)
            {
                case (1):
                    emitter = manager.act1Alarm.sound.GetComponent<StudioEventEmitter>();
                    SetPosition(emitter);
                    break;
                case (2):
                    emitter = manager.act2Alarm.sound.GetComponent<StudioEventEmitter>();
                    SetPosition(emitter);
                    break;
                case (3):
                    emitter = manager.act3Alarm.sound.GetComponent<StudioEventEmitter>();
                    SetPosition(emitter);
                    break;
            }
            
        }
    }

    private void StartAlarmSound()
    {
        transform.GetChild(0).GetComponent<StudioEventEmitter>().Play();
    }
    private void SetPosition(StudioEventEmitter emitter)
    {
        if(true)
        {
            int pos = 0;
            transform.GetChild(0).GetComponent<StudioEventEmitter>().EventInstance.getTimelinePosition(out pos);
            FMOD.RESULT result = emitter.EventInstance.setTimelinePosition(pos + 7 * 23);
            Debug.Log("Pos " + pos);
        }
        int pos1;
        emitter.EventInstance.getTimelinePosition(out pos1);
        //Debug.Log("pos " + pos1);
        if (pos1 == 0) count++;
        if (pos1 != 0)
        {
            Debug.Log("Delay in frames " + count);
            transform.GetChild(0).GetComponent<StudioEventEmitter>().Stop();
            //emitter.EventInstance.getTimelinePosition(out pos);
            Destroy(this.gameObject);
        }
    }

    public void ReadyToStop(int whichAct)
    {
        stop = true;
        act = whichAct;
    }
}
