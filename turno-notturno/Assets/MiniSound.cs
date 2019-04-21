using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSound : MonoBehaviour
{
    private AlarmManager manager;
    [SerializeField] int act;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<AlarmManager>();
        SetSoundPosition();
    }

    private void SetSoundPosition()
    {
        if (manager)
        {
            manager.StopAlarm();
            switch (act)
            {
                case 1:
                    float angle = PlayerPrefs.GetFloat("act1angle");
                    float dist = PlayerPrefs.GetFloat("act1distance");
                    float z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                    Vector3 pos = new Vector3(Camera.main.transform.position.x + x, 
                        Camera.main.transform.position.y, Camera.main.transform.position .z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_1);
                    manager.act1Alarm.sound.transform.parent.position = pos;
                    break;
                case 2:
                    angle = PlayerPrefs.GetFloat("act2angle");
                    dist = PlayerPrefs.GetFloat("act2distance");
                    z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                    pos = new Vector3(Camera.main.transform.position.x + x,
                        Camera.main.transform.position.y, Camera.main.transform.position.z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_2);
                    manager.act2Alarm.sound.transform.parent.position = pos;
                    break;
                case 3:
                    angle = PlayerPrefs.GetFloat("act3angle");
                    dist = PlayerPrefs.GetFloat("act3distance");
                    z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                    pos = new Vector3(Camera.main.transform.position.x + x,
                        Camera.main.transform.position.y, Camera.main.transform.position.z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_3);
                    manager.act3Alarm.sound.transform.parent.position = pos;
                    break;
            }
        }
    }
}
