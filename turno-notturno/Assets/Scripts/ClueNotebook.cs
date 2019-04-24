using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClueNotebook : MonoBehaviour
{
    public GameObject Book;
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

    public ObservableCollection<String> FinalCluesGuessed = new ObservableCollection<String>();

    public ArrayList Act1ClueChosen = new ArrayList();
    public ArrayList Act2ClueChosen = new ArrayList();
    public ArrayList Act3ClueChosen = new ArrayList();
    public ArrayList Act4ClueChosen = new ArrayList();
    public ArrayList Act5ClueChosen = new ArrayList();
    public ArrayList Act6ClueChosen = new ArrayList();

    private void Start()
    {
        FinalCluesGuessed.CollectionChanged += CluesGuessed_CollectionChanged;
    }

    public void Update()
    { 
        if (CheckforCorrectSolution()) {
            Debug.Log("Hurrah You Win!");
        } 
    }

    public bool CheckforCorrectSolution()
    { 
        String[] correctClues = { "Blue", "Spirals", "Copper", "Abstract" };
        String[] cluesFromTextBox = { Regex.Replace(ClueTexts[0].text, "[^a-zA-Z]", ""), Regex.Replace(ClueTexts[1].text, "[^a-zA-Z]", ""), Regex.Replace(ClueTexts[2].text, "[^a-zA-Z]", ""), Regex.Replace(ClueTexts[3].text, "[^a-zA-Z]", "") };
        
        if (correctClues.Any(cluesFromTextBox.Contains)) {  return true; }
        return false;
    }

    private void CluesGuessed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        //CluesGuessed = new ObservableCollection<String>(CluesGuessed.Distinct().ToList());

        //Remove clues more than 4
        int j = FinalCluesGuessed.Count;
        Debug.Log(j);
        while (FinalCluesGuessed.Count > 4)
        {
            FinalCluesGuessed.RemoveAt(j);
            j--;
        }

        Debug.Log("Collection Changed! " + string.Join(", ", FinalCluesGuessed.ToArray()));
        for (int i = 0; i < FinalCluesGuessed.Count; i++)
        {
            Debug.Log("Collection Changed! Inside loop. Size of Clues guessed " + FinalCluesGuessed.Count);
            ClueTexts[i].text = FinalCluesGuessed[i]; 
            Debug.Log(ClueTexts[i].text + " Clue text " + i + FinalCluesGuessed[i]);
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



    public void OnClickClue()
    {
        GameObject clueButtonObject = EventSystem.current.currentSelectedGameObject;
        String clickedClueText = ( clueButtonObject.GetComponentInChildren<Text>().text);
      
        String parentActofClickedClue = clueButtonObject.transform.parent.name;

        if (parentActofClickedClue == "Act1")
        {

            if (!Act1ClueChosen.Contains(clickedClueText))
            {
                Act1ClueChosen.Add(clickedClueText);
                if (Act1ClueChosen.Count == 3) { Act1ClueChosen.RemoveAt(0); }
            }
            else
            {
                Act1ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act1ClueChosen, Texts1);
        }
        else if (parentActofClickedClue == "Act2")
        {

            if (!Act2ClueChosen.Contains(clickedClueText))
            {
                Act2ClueChosen.Add(clickedClueText);
            }
            else
            {
                Act2ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act2ClueChosen, Texts2);
        }
        else if (parentActofClickedClue == "Act3")
        {

            if (!Act3ClueChosen.Contains(clickedClueText))
            {
                Act3ClueChosen.Add(clickedClueText);
            }
            else
            {
                Act3ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act3ClueChosen, Texts3);
        }
        else if (parentActofClickedClue == "Act4")
        {

            if (!Act4ClueChosen.Contains(clickedClueText))
            {
                Act4ClueChosen.Add(clickedClueText);
            }
            else
            {
                Act4ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act4ClueChosen, Texts4);
        }
        else if (parentActofClickedClue == "Act5")
        {

            if (!Act5ClueChosen.Contains(clickedClueText))
            {
                Act5ClueChosen.Add(clickedClueText);
            }
            else
            {
                Act5ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act5ClueChosen, Texts5);
        }
        else if (parentActofClickedClue == "Act6")
        {

            if (!Act6ClueChosen.Contains(clickedClueText))
            {
                Act6ClueChosen.Add(clickedClueText);
            }
            else
            {
                Act6ClueChosen.Remove(clickedClueText);
            }

            TrackActRowForCluesChosen(Act6ClueChosen, Texts6);
        }

       
    }


    void TrackActRowForCluesChosen(ArrayList actChosenClues, GameObject[] textsFromActRow)
    {
        //Debug.Log(string.Join(", ", actChosenClues.ToArray()));
        if (actChosenClues.Count == 2)
        {
            foreach (GameObject t in textsFromActRow)
            {
                if (!actChosenClues.Contains(t.GetComponent<Text>().text))
                {
                    t.GetComponent<Text>().text = StrikeThrough(t.GetComponent<Text>().text);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/menuClick");
                }
                else
                {
                    if (!FinalCluesGuessed.Contains(t.GetComponent<Text>().text))
                    {
                        FinalCluesGuessed.Add(t.GetComponent<Text>().text);
                        Debug.Log("Clues guessed " + string.Join(", ", FinalCluesGuessed.ToArray()));
                    }
                }
            }
        }
        else
        {
            //unstrike text
            //foreach (GameObject t in textsFromRow)
            //{
            //    Debug.Log("Unstriked word " + UnstrikeThrough(t.GetComponent<Text>().text));
            //    t.GetComponent<Text>().text = UnstrikeThrough(t.GetComponent<Text>().text);
            //}
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


   
    public string UnstrikeThrough(string str)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        str = rgx.Replace(str, "");
        return str;
    }


    public void TurnPage6To5()
    {
        Animator anim = Book.GetComponent<Animator>();

        anim.SetBool("Turn6to5", true);
        anim.Play("TurnPage6To5");
    }

}










