using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClueNotebook : MonoBehaviour
{
     
    public GameObject[] Texts1;
    public GameObject[] Texts2;
    public GameObject[] Texts3;
    public GameObject[] Texts4;
    public GameObject[] Texts5;
    public GameObject[] Texts6;

    void Start()
    {
        
    }

    private void InitializeClueTexts(GameObject art1Row)
    {
        //for(int i = 0; i< 4; i++)
        //{
        //    Texts1[i] = Art1Row.transform.GetChild(i).GetChild(0).gameObject;
        //}
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape)))_
        checkClueCollectionProgress();
    }

    private void checkClueCollectionProgress()
    {
        Debug.Log(PlayerPrefs.GetInt("ClueFoundAct11") + PlayerPrefs.GetInt("ClueFoundAct12"));

        if (PlayerPrefs.HasKey("ClueFoundAct11")) { Texts1[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct12")) { Texts1[1].SetActive(true); }

        if (PlayerPrefs.HasKey("ClueFoundAct21")) { Texts2[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct22")) { Texts2[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct23")) { Texts2[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct24")) { Texts2[3].SetActive(true); }

        if (PlayerPrefs.HasKey("ClueFoundAct31")) { Texts3[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct32")) { Texts3[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct33")) { Texts3[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct34")) { Texts3[3].SetActive(true); }

        if (PlayerPrefs.HasKey("ClueFoundAct41")) { Texts4[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct42")) { Texts4[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct43")) { Texts4[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct44")) { Texts4[3].SetActive(true); }

        if (PlayerPrefs.HasKey("ClueFoundAct51")) { Texts5[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct52")) { Texts5[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct53")) { Texts5[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct54")) { Texts5[3].SetActive(true); }


        if (PlayerPrefs.HasKey("ClueFoundAct61")) { Texts6[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct62")) { Texts6[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct63")) { Texts6[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct64")) { Texts6[3].SetActive(true); }

    }
}