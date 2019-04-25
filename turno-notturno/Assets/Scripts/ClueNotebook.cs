using System;
using System.Collections.ObjectModel;
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

    public VerticalLayoutGroup hlg;
   
    
    public enum ClueState { Normal, Chosen, Striked }

    [System.Serializable]
    public class ClueClass
    {
        public string Name;
        public ClueState State;
    };
     
    public ClueClass[,] Clue2DArray = new ClueClass[6, 4];

    private void Start()
    {

        
        FinalCluesGuessed.CollectionChanged += FinalCluesGuessed_CollectionChanged;

        Clue2DArray[0, 0] = new ClueClass { Name = "Blue", State = ClueState.Normal };
        Clue2DArray[0, 1] = new ClueClass { Name = "Cityscape", State = ClueState.Normal };
        Clue2DArray[0, 2] = new ClueClass { Name = "Wood", State = ClueState.Normal };
        Clue2DArray[0, 3] = new ClueClass { Name = "Spirals", State = ClueState.Normal };


        Clue2DArray[1, 0] = new ClueClass { Name = "Red", State = ClueState.Normal };
        Clue2DArray[1, 1] = new ClueClass { Name = "Portrait", State = ClueState.Normal };
        Clue2DArray[1, 2] = new ClueClass { Name = "Copper", State = ClueState.Normal };
        Clue2DArray[1, 3] = new ClueClass { Name = "Spirals", State = ClueState.Normal };

        Clue2DArray[2, 0] = new ClueClass { Name = "Green", State = ClueState.Normal };
        Clue2DArray[2, 1] = new ClueClass { Name = "Abstract", State = ClueState.Normal };
        Clue2DArray[2, 2] = new ClueClass { Name = "Copper", State = ClueState.Normal };
        Clue2DArray[2, 3] = new ClueClass { Name = "Teeth", State = ClueState.Normal };

        Clue2DArray[3, 0] = new ClueClass { Name = "Red", State = ClueState.Normal };
        Clue2DArray[3, 1] = new ClueClass { Name = "Abstract", State = ClueState.Normal };
        Clue2DArray[3, 2] = new ClueClass { Name = "Wood", State = ClueState.Normal };
        Clue2DArray[3, 3] = new ClueClass { Name = "Spirals", State = ClueState.Normal };

        Clue2DArray[4, 0] = new ClueClass { Name = "Blue", State = ClueState.Normal };
        Clue2DArray[4, 1] = new ClueClass { Name = "Cityscape", State = ClueState.Normal };
        Clue2DArray[4, 2] = new ClueClass { Name = "Copper", State = ClueState.Normal };
        Clue2DArray[4, 3] = new ClueClass { Name = "Teeth", State = ClueState.Normal };

        Clue2DArray[5, 0] = new ClueClass { Name = "Blue", State = ClueState.Normal };
        Clue2DArray[5, 1] = new ClueClass { Name = "Abstract", State = ClueState.Normal };
        Clue2DArray[5, 2] = new ClueClass { Name = "Teeth", State = ClueState.Normal };
        Clue2DArray[5, 3] = new ClueClass { Name = "Technology", State = ClueState.Normal };
         
    }

     

    //Function: Populate Text components with modified texts
    void UpdateTextElementWithClue2DArray()
    {
        for (int row = 0; row < 6; row++)
        {
            for (int column = 0; column < 4; column++)
            {
                if (row == 0)
                {
                    Texts1[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
                else if (row == 1)
                {
                    Texts2[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
                else if (row == 2)
                {
                    Texts3[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
                else if (row == 3)
                {
                    Texts4[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
                else if (row == 4)
                {
                    Texts5[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
                else if (row == 5)
                {
                    Texts6[column].GetComponent<Text>().text = Clue2DArray[row, column].Name;
                }
            }
        }
    }

    //Function: Check if 2 chosen clues are made per row then strike the rest
    //Adds to Observable Collection
    void CheckIf2CluesChosen()
    {
        int chosenNum = 0; 

        for (int row = 0; row < 6; row++)
        {
            ////Mark chosen ones from Collection
            //for (int column = 0; column < 4; column++)
            //{
            //    if (FinalCluesGuessed.Contains( Clue2DArray[row, column].Name))
            //    {
            //        Clue2DArray[row, column].State = ClueState.Chosen;
            //    }
            //}

            //Count chosens
            for (int column = 0; column < 4; column++)
            {
                if (Clue2DArray[row, column].State == ClueState.Chosen)
                {
                    chosenNum++;

                    //if (row == 4) {
                    //    Debug.Log(chosenNum+" "+Clue2DArray[row, column].Name);
                    //}
                }
            }


            //If 2 chosen then mark rest Striked and add chosens in Collection
            if (chosenNum == 2)
            {
                for (int column = 0; column < 4; column++)
                {
                    if (Clue2DArray[row, column].State == ClueState.Normal)
                    {
                        //Clue2DArray[row, column].State = ClueState.Striked;
                        UpdateDependentCluesIn2DArrayClues(Clue2DArray[row, column].Name, ClueState.Striked);
                    }
                    else if (Clue2DArray[row, column].State == ClueState.Chosen)
                    {
                        if (!FinalCluesGuessed.Contains(CleanString(Clue2DArray[row, column].Name)))
                        {
                            FinalCluesGuessed.Add( CleanString(Clue2DArray[row, column].Name));
                        }
                    }
                }

            }

            chosenNum = 0;
        } 
    }

    void ButtonUpdate(string clueName, bool isUnderline) {
        clueName = CleanString(clueName);
        foreach (GameObject t in Texts1) {
            if (CleanString(t.GetComponent<Text>().text) == (clueName)) { 
                    t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts1)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts2)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts3)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts4)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts5)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }
        foreach (GameObject t in Texts6)
        {
            if (CleanString(t.GetComponent<Text>().text) == (clueName))
            {
                t.GetComponentInParent<Image>().enabled = isUnderline;
                break;
            }
        }

    }

    //function: Go through array and do visual according to state
    private void VisualiseClueTexts()
    {
        for (int row = 0; row < 6; row++)
        { 
            for (int column = 0; column < 4; column++)
            {
                if (Clue2DArray[row, column].State == ClueState.Chosen) {
                    //Add ! to show chosen
                    //////if(!Clue2DArray[row, column].Name.Contains("@"))
                    //////    Clue2DArray[row, column].Name = "@"+Clue2DArray[row, column].Name;
                     
                    //Find the button for the chosen clue and enable its image
                    ButtonUpdate(Clue2DArray[row, column].Name, true);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/menuClick");

                }
                else if (Clue2DArray[row, column].State == ClueState.Normal)
                { 
                    //Clean the string 
                    Clue2DArray[row, column].Name = CleanString(Clue2DArray[row, column].Name); 
                    //Remove Underline from clue
                    ButtonUpdate(Clue2DArray[row, column].Name, false);
                }
                else if (Clue2DArray[row, column].State == ClueState.Striked)
                {
                    //Strike it through
                    if (!isStriked(Clue2DArray[row, column].Name))
                    {
                        Clue2DArray[row, column].Name = StrikeThrough(Clue2DArray[row, column].Name);
                        FMODUnity.RuntimeManager.PlayOneShot("event:/menuClick");
                    }
                } 
            }
        }
    }

    private string CleanString(string name)
    {
        return Regex.Replace(name, "[^a-zA-Z]", ""); 
    }

    //Mark clicked clue as Chosen or Normal 
    public void OnClickClue()
    {
        GameObject clueButtonObject = EventSystem.current.currentSelectedGameObject;
        String clickedClueText = (clueButtonObject.GetComponentInChildren<Text>().text);
        String parentActofClickedClue = clueButtonObject.transform.parent.name;

        //Find clicked clue in 2Darray
        int row = Convert.ToInt32( Regex.Match(parentActofClickedClue, @"\d+").Value) - 1;
        for (int column = 0; column < 4; column++)
        {
            if (clickedClueText.Contains(Clue2DArray[row, column].Name))
            { 
                if (Clue2DArray[row, column].State == ClueState.Striked)
                {
                    //Make all clues in row normal
                    //Remove Chosen clues from final clue
                    for (int col = 0; col < 4; col++)
                    {
                        UpdateDependentCluesIn2DArrayClues(Clue2DArray[row, col].Name, ClueState.Normal);
                        //Clue2DArray[row, col].State = ClueState.Normal;
                        if (FinalCluesGuessed.Contains(Clue2DArray[row, col].Name)) {
                            //Debug.Log("1Removing "+ Clue2DArray[row, col].Name);
                            FinalCluesGuessed.Remove(Clue2DArray[row, col].Name);
                        }

                    }
                } 
                else if (Clue2DArray[row, column].State == ClueState.Chosen)
                {
                    UpdateDependentCluesIn2DArrayClues(Clue2DArray[row, column].Name, ClueState.Normal);
                    //Clue2DArray[row, column].State = ClueState.Normal;
                    if (FinalCluesGuessed.Contains(Clue2DArray[row, column].Name)) {
                       // Debug.Log("2Removing " + Clue2DArray[row, column].Name);
                        FinalCluesGuessed.Remove(Clue2DArray[row, column].Name);
                    }
                }
                else if (Clue2DArray[row, column].State == ClueState.Normal)
                {
                    //Clue2DArray[row, column].State = ClueState.Chosen;
                    UpdateDependentCluesIn2DArrayClues(Clue2DArray[row, column].Name, ClueState.Chosen);
                }
            } 
        }
         
        CheckIf2CluesChosen();
        VisualiseClueTexts();
        UpdateTextElementWithClue2DArray();
        //PrintNameStateOfClueArray(5,4);
    }

    private void UpdateDependentCluesIn2DArrayClues(String clueName, ClueState state)
    {
        for (int row = 0; row < 6; row++)
        {
            for (int column = 0; column < 4; column++)
            {
                if (Clue2DArray[row, column].Name == clueName) {
                    Clue2DArray[row, column].State = state;
                }
            }
        }
    }

    void PrintNameStateOfClueArray(int r, int c)
    {
        Debug.Log("\n"); Debug.Log("\n");
        for (int row = 0; row < r; row++)
        {
            for (int column = 0; column < c; column++)
            {
                Debug.Log(Clue2DArray[row, column].Name + "="+Clue2DArray[row, column].State.ToString());
            }
            Debug.Log(" ");
        }
    }


    private void FinalCluesGuessed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    { 
        for (int j = 0; j < FinalCluesGuessed.Count; j++)
        {
            ClueTexts[j].text = FinalCluesGuessed[j];
        }

        for (int j = FinalCluesGuessed.Count; j < 4-FinalCluesGuessed.Count; j++)
        {
            ClueTexts[j].text = "xxxxx";
        }



        if (CheckforCorrectSolution()) {
            Debug.Log("CHAMPION");
            FindObjectOfType<ObjectiveManager>().FinishedPuzzle();
        }
    }

    private void OnDisable()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/pageTurn");
    }


    private void Update()
    {
        hlg.enabled = false; hlg.enabled = true;
    }

    void OnEnable()
    {
        hlg = gameObject.GetComponent<VerticalLayoutGroup>();
        
        hlg.CalculateLayoutInputHorizontal();
        hlg.CalculateLayoutInputVertical();
        hlg.SetLayoutHorizontal();
        hlg.SetLayoutVertical();
        Canvas.ForceUpdateCanvases();
        

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


    //Activates clues in notebook on collecting
    private bool CheckClueCollectionProgress()
    {
        if (PlayerPrefs.HasKey("ClueFoundAct11")) { Texts1[0].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct12")) { Texts1[1].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct13")) { Texts1[2].SetActive(true); }
        if (PlayerPrefs.HasKey("ClueFoundAct14")) { Texts1[3].SetActive(true); }

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


        //Checking for enabling Guesses
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
    
    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    public bool isStriked(string str)
    {
        foreach (char c in str)
        {
            if (c == '\u0336')
            {
                return true;
            }
        }
        return false;
    }

    public string UnstrikeThrough(string str)
    {
        Regex rgx = new Regex("[^a-zA-Z0-9 -]");
        str = rgx.Replace(str, "");
        return str;
    }

    
    public bool CheckforCorrectSolution()
    { 
        if (FinalCluesGuessed.Contains("Blue"))
        {
            if (FinalCluesGuessed.Contains("Spirals"))
            {
                if (FinalCluesGuessed.Contains("Copper"))
                {
                    if (FinalCluesGuessed.Contains("Abstract"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    } 

     
}










