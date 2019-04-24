using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MiniSound : MonoBehaviour
{
    private AlarmManager manager;
    [SerializeField] int act;
    [SerializeField] float riseTime;
    private bool started = false;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<AlarmManager>();
        if (manager)
        {
            Reset();
        }
        
        
    }
    private void Update()
    {
        if(started)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Min(1, timer / riseTime);
            SetVolume(volume);
        }
    }

    private void SetSoundPosition()
    {
        if (manager)
        {
            
            float angle, dist, z, x;
            Vector3 pos;
            switch (act)
            {
                case 1:
                    //float angle = PlayerPrefs.GetFloat("act1angle");
                    //float dist = PlayerPrefs.GetFloat("act1distance");
                    //float z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    //float x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                   // Vector3 pos = new Vector3(Camera.main.transform.position.x + x, 
                       // Camera.main.transform.position.y, Camera.main.transform.position .z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_1);
                    //manager.act1Alarm.light.SetActive(false);
                    //manager.act1Alarm.sound.transform.parent.position = pos;
                    manager.act1Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(0f);
                    break;
                case 2:
                    angle = PlayerPrefs.GetFloat("act2angle");
                    dist = PlayerPrefs.GetFloat("act2distance");
                    z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                    pos = new Vector3(Camera.main.transform.position.x + x,
                    Camera.main.transform.position.y, Camera.main.transform.position.z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_2);
                    //manager.act2Alarm.light.SetActive(false);
                    manager.act2Alarm.sound.transform.parent.position = pos;
                    manager.act2Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(0f);
                    break;
                case 3:
                    angle = PlayerPrefs.GetFloat("act3angle");
                    dist = PlayerPrefs.GetFloat("act3distance");
                    z = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    x = dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                    pos = new Vector3(Camera.main.transform.position.x + x,
                        Camera.main.transform.position.y, Camera.main.transform.position.z + z);
                    manager.ActivateAlarm(AlarmManager.Act.act_3);
                    //manager.act3Alarm.light.SetActive(false);
                    manager.act3Alarm.sound.transform.parent.position = pos;
                    manager.act3Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(0f);
                    break;
            }
            started = true;
        }
    }
    private void SetVolume(float volume)
    {
        switch (act)
        {
            case 1:
                manager.act1Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(volume);
                break;
            case 2:
                manager.act2Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(volume);
                break;
            case 3:
                manager.act3Alarm.sound.GetComponent<StudioEventEmitter>().EventInstance.setVolume(volume);
                break;
        }
    }
    public void Reset()
    {
        timer = 0;
        started = false;
        if(manager)
        {
            manager.StopAlarm();
            manager.act1Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            manager.act1Alarm.light.SetActive(false);
            manager.act2Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            manager.act2Alarm.light.SetActive(false);
            manager.act3Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            manager.act3Alarm.light.SetActive(false);
            manager.act4Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            manager.act4Alarm.light.SetActive(false);
            manager.guardRoomAlarm.light.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            manager.guardRoomAlarm.light.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SetSoundPosition();
        }
    }
}
