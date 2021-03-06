﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AlarmManager : MonoBehaviour
{
    public enum Act
    {
        act_1,
        act_2,
        act_3,
        act_4
    }

    [Serializable]
    public class AlarmSystem
    {
        public GameObject light;
        public GameObject sound;
        public GameObject alarmLight;
        public Material alarmMinimap;
    }

    public bool alarmIsEnabled = false;
 
    public GameObject minimapScreen;
    public Material emptyMinimap;
    public AlarmSystem guardRoomAlarm;
    public AlarmSystem act1Alarm;
    public AlarmSystem act2Alarm;
    public AlarmSystem act3Alarm;
    public AlarmSystem act4Alarm;

    private AlarmSystem[] alarmSystems;
    private Material alarmMinimap;
    private MeshRenderer minimapRenderer;
    private bool minimapToggle = false;
    private Vector3 act1Pos;
    private Vector3 act2Pos;
    private Vector3 act3Pos;
    private Vector3 act4Pos;
    private Vector3 position;

    public string writing;
    public List<Vector2> posses;

    void Awake()
    {
        position = transform.position;
        act1Pos = act1Alarm.sound.transform.parent.position;
        act2Pos = act2Alarm.sound.transform.parent.position;
        act3Pos = act3Alarm.sound.transform.parent.position;
        act4Pos = act4Alarm.sound.transform.parent.position;
        if (minimapScreen) minimapRenderer = minimapScreen.GetComponent<MeshRenderer>();
        alarmSystems = new AlarmSystem[] { guardRoomAlarm, act1Alarm, act2Alarm, act3Alarm, act4Alarm };
        GameObject[] managers = GameObject.FindGameObjectsWithTag("AlarmManager");
        if (managers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void SwapMinimapImage()
    {
        if (!minimapRenderer) return;
        if (!alarmIsEnabled)
        {
            minimapRenderer.material = emptyMinimap;
            return;
        }
        if (minimapToggle)
        {
            minimapToggle = false;
            minimapRenderer.material = emptyMinimap;
        }
        else
        {
            minimapToggle = true;
            minimapRenderer.material = alarmMinimap;
        }
        Invoke("SwapMinimapImage", 1f);
    }

    public void StopAlarm()
    {
        StopAlarm(guardRoomAlarm);
        StopAlarm(act1Alarm);
        StopAlarm(act2Alarm);
        StopAlarm(act3Alarm);
        StopAlarm(act4Alarm);
        alarmIsEnabled = false;
        SwapMinimapImage();
    }

    public void StopAlarm(int act)
    {
        switch (act)
        {
            case 1:
                StopAlarm(act1Alarm);
                break;
            case 2:
                StopAlarm(act2Alarm);
                break;
            case 3:
                StopAlarm(act3Alarm);
                break;
            case 4:
                StopAlarm(act4Alarm);
                break;
        }
        alarmIsEnabled = false;
        SwapMinimapImage();
    }

    public void ActivateAlarm(Act act)
    {
        //ActivateAlarm(guardRoomAlarm);
        if (act == Act.act_1) ActivateAlarm(act1Alarm);
        else if (act == Act.act_2) ActivateAlarm(act2Alarm);
        else if (act == Act.act_3) ActivateAlarm(act3Alarm);
        else if (act == Act.act_4) ActivateAlarm(act4Alarm);
        else
        {
            Debug.LogError("AlarmSystem: Invalid act!");
        }

        alarmIsEnabled = true;
        SwapMinimapImage();
    }

    private void ActivateAlarm(AlarmSystem alarmSystem)
    {
        Rotate rotate = alarmSystem.light.GetComponentInParent<Rotate>();
        if (rotate) rotate.StartRotation();
        if (alarmSystem.light) alarmSystem.light.SetActive(true);
        if (alarmSystem.sound)
        {
            alarmSystem.sound.GetComponent<StudioEventEmitter>().Play();
            alarmSystem.sound.transform.localPosition = Vector3.zero;
        }
        if (alarmSystem.alarmLight) alarmSystem.alarmLight.SetActive(true);
        alarmMinimap = alarmSystem.alarmMinimap;
    }

    private void StopAlarm(AlarmSystem alarmSystem)
    {
        Rotate rotate = alarmSystem.light.GetComponentInParent<Rotate>();
        if (rotate) rotate.StopRotation();
        if (alarmSystem.light) alarmSystem.light.SetActive(false);
        if (alarmSystem.alarmLight) alarmSystem.alarmLight.SetActive(false);
        if (alarmSystem.sound)  alarmSystem.sound.GetComponent<StudioEventEmitter>().Stop();
    }

    public void ResetPositions()
    {
        transform.position = position;
        act1Alarm.sound.transform.parent.position = act1Pos;
        act2Alarm.sound.transform.parent.position = act2Pos;
        act3Alarm.sound.transform.parent.position = act3Pos;
        act4Alarm.sound.transform.parent.position = act4Pos;
        act1Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        //act1Alarm.light.SetActive(true);
        act2Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        //act2Alarm.light.SetActive(true);
        act3Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        //act3Alarm.light.SetActive(true);
        act4Alarm.sound.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        //act4Alarm.light.SetActive(true);
        guardRoomAlarm.light.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        //guardRoomAlarm.light.SetActive(true);
    }
}
