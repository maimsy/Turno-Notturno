using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundPos : MonoBehaviour
{
    private AlarmManager manager;
    private void Awake()
    {
        manager = GetComponent<AlarmManager>();
        GameObject[] managers = GameObject.FindGameObjectsWithTag("AlarmManager");
        if (managers.Length > 1)
        {
            foreach(GameObject obj in managers)
            {
                if(obj != this.gameObject)
                {
                    int pos = obj.GetComponent<SoundPos>().GetPosition(obj.GetComponent<AlarmManager>());
                    SetPosition(pos);
                    Destroy(obj);
                    break;
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public int GetPosition(AlarmManager alarmManager)
    {
        int act = PlayerPrefs.GetInt("GameState");
        int position = 0;
        switch (act)
        {
            case (1):
                alarmManager.act1Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.getTimelinePosition(out position);
                break;
            case (3):
                alarmManager.act2Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.getTimelinePosition(out position);
                break;
            case (5):
                alarmManager.act3Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.getTimelinePosition(out position);
                break;
        }
        return position;
    }

    private void SetPosition(int pos)
    {
        int act = PlayerPrefs.GetInt("GameState");
        switch (act)
        {
            case (1):
                manager.ActivateAlarm(AlarmManager.Act.act_1);
                manager.act1Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setTimelinePosition(pos);

                break;
            case (3):
                manager.ActivateAlarm(AlarmManager.Act.act_2);
                manager.act2Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setTimelinePosition(pos);

                break;
            case (5):
                manager.ActivateAlarm(AlarmManager.Act.act_3);
                manager.act3Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setTimelinePosition(pos);

                break;
        }
    }
}
