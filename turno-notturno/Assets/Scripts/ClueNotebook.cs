using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClueNotebook : MonoBehaviour
{
    public GameObject Act1;
    public GameObject Act2;
    public GameObject Act3;
    public GameObject Act4;
    public GameObject Act5;
    public GameObject Act6;

    public GameObject[] Texts1;
    public GameObject[] Texts2;
    public GameObject[] Texts3;
    public GameObject[] Texts4;
    public GameObject[] Texts5;
    public GameObject[] Texts6;

    public GameObject Guesses;

    public Text[] ClueTexts;

    public ObservableCollection<String> CluesGuessed = new ObservableCollection<String>();

    public ArrayList Act1ClueChosen = new ArrayList();
    public ArrayList Act2ClueChosen = new ArrayList();
    public ArrayList Act3ClueChosen = new ArrayList();
    public ArrayList Act4ClueChosen = new ArrayList();
    public ArrayList Act5ClueChosen = new ArrayList();
    public ArrayList Act6ClueChosen = new ArrayList();

    private void Start()
    { 
        CluesGuessed.CollectionChanged += CluesGuessed_CollectionChanged;
    }

    private void CluesGuessed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (CluesGuessed.Count > 4) {
            for (int i = CluesGuessed.Count; i > 4; i--) {
                CluesGuessed.RemoveAt(i);
            }
        }

        Debug.Log("Collection Changed! " + string.Join(", ", CluesGuessed.ToArray()));
        for (int i = 0; i < CluesGuessed.Count; i++)
        {
            Debug.Log("Collection Changed! Inside loop. Size of Clues guessed "+ CluesGuessed.Count);
            ClueTexts[i].text = CluesGuessed[i];
            
            Debug.Log(ClueTexts[i].text + " Clue text " + i + CluesGuessed[i]);
        }
    }

    void OnEnable()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");

        VerticalLayoutGroup hlg = gameObject.GetComponent<VerticalLayoutGroup>();
        Canvas.ForceUpdateCanvases();
        hlg.CalculateLayoutInputHorizontal();
        hlg.CalculateLayoutInputVertical();
        hlg.SetLayoutHorizontal();
        hlg.SetLayoutVertical();

        UnityEngine.UI.Button[] buttons = this.GetComponentsInChildren<UnityEngine.UI.Button>();

        if (CheckClueCollectionProgress())
        {
            Guesses.SetActive(true);
            foreach (UnityEngine.UI.Button b in buttons)
            {
                b.interactable = true;
            }
        }
        else
        {
            foreach (UnityEngine.UI.Button b in buttons)
            {
                b.interactable = false;
            }
        }
    }



    private bool CheckClueCollectionProgress()
    {
        if (PlayerPrefs.HasKey("ClueFoundAct11")) { Texts1[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct12")) { Texts1[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct13")) { Texts1[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct14")) { Texts1[1].SetActive(true); }

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

        for (int i = 0; i < Texts1.Length; i++)
        {
            if (!Texts1[i].activeInHierarchy) { return false; }
        }
        for (int i = 0; i < Texts2.Length; i++)
        {
            if (!Texts2[i].activeInHierarchy) return false;
        }
        for (int i = 0; i < Texts3.Length; i++)
        {
            if (!Texts3[i].activeInHierarchy) return false;
        }
        for (int i = 0; i < Texts4.Length; i++)
        {
            if (!Texts4[i].activeInHierarchy) return false;
        }
        for (int i = 0; i < Texts5.Length; i++)
        {
            if (!Texts5[i].activeInHierarchy) return false;
        }
        for (int i = 0; i < Texts6.Length; i++)
        {
            if (!Texts6[i].activeInHierarchy) return false;
        }
        return true;
    }

    

    public void onClickClue()
    {
        GameObject clueButtonObject = EventSystem.current.currentSelectedGameObject;
        Debug.Log(clueButtonObject);
        String clueText = clueButtonObject.GetComponentInChildren<Text>().text;

        String par = clueButtonObject.transform.parent.name;

        if (par == "Act1") {

            if (!Act1ClueChosen.Contains(clueText)) {
                Act1ClueChosen.Add(clueText);
                if (Act1ClueChosen.Count == 3) { Act1ClueChosen.RemoveAt(0); }
            }
            else
            {
                Act1ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act1ClueChosen, Texts1);
        }
        else if (par == "Act2")
        {

            if (!Act2ClueChosen.Contains(clueText)) {
                Act2ClueChosen.Add(clueText);
            }
            else
            {
                Act2ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act2ClueChosen, Texts2);
        }
        else if (par == "Act3")
        {

            if (!Act3ClueChosen.Contains(clueText))
            {
                Act3ClueChosen.Add(clueText);
            }
            else
            {
                Act3ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act3ClueChosen, Texts3);
        }
        else if (par == "Act4")
        {

            if (!Act4ClueChosen.Contains(clueText))
            {
                Act4ClueChosen.Add(clueText);
            }
            else
            {
                Act4ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act4ClueChosen, Texts4);
        }
        else if (par == "Act5")
        {

            if (!Act5ClueChosen.Contains(clueText))
            {
                Act5ClueChosen.Add(clueText);
            }
            else
            {
                Act5ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act5ClueChosen, Texts5);
        }
        else if (par == "Act6")
        {

            if (!Act6ClueChosen.Contains(clueText))
            {
                Act6ClueChosen.Add(clueText);
            }
            else
            {
                Act6ClueChosen.Remove(clueText);
            }

            TrackActRowForCluesChosen(Act6ClueChosen, Texts6);
        }
    }


    void TrackActRowForCluesChosen(ArrayList actClues, GameObject[] texts)
    {
        Debug.Log(  string.Join(", ", actClues.ToArray()));
        if(actClues.Count == 2)
        { 
            foreach(GameObject t in texts)
            {
                if (!actClues.Contains(t.GetComponent<Text>().text))
                {
                    t.GetComponent<Text>().text = StrikeThrough(t.GetComponent<Text>().text );
                     
                }
                else
                {
                    if (!CluesGuessed.Contains(t.GetComponent<Text>().text))
                    {
                        CluesGuessed.Add(t.GetComponent<Text>().text);
                        Debug.Log("Clues guessed " + string.Join(", ", CluesGuessed.ToArray()));
                    }
                }
            }
        }
        else
        {
            //unstrike text
            //UnstrikeThrough(t.GetComponent<Text>().text);
        }
    }

    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }


    //TODO
    public string UnstrikeThrough(string s)
    {
        string strikethrough = "";
        s.Replace("\u0336", "");
        //foreach (char c in s)
        //{
        //    strikethrough = strikethrough + c + '\u0336';
        //}
        return strikethrough;
    }

}










