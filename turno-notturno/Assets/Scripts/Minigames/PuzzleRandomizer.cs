using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PropertyType
{
    Color = 0,
    Theme = 1,
    Material = 2,
    Special = 3
}
public enum ArtTheme
{
    Cityscape,
    Portrait,
    Abstract
}
public enum ArtMaterial
{
    Wood,
    Copper,
}
public enum ArtColour
{
    Red,
    Green,
    Blue
}
public enum ArtShape
{
    Spirals,
    Teeth,
    Technology
}

public class PuzzleRandomizer : MonoBehaviour
{
    public GameObject themeParent;
    public GameObject materialParent;
    public GameObject colorParent;
    public GameObject shapeParent;
    private List<GameObject> clues;
    private List<int> solution;
    public int clueAmount = 6;
    // Start is called before the first frame update
    void Awake()
    {
        clues = new List<GameObject>();
        SetCorrectSolution();
        for (int i = 0; i < clueAmount; i++)
        {

            int howManyCorrect = 0;
            clues.Add(themeParent.transform.GetChild(i).gameObject);
            ArtTheme theme = (ArtTheme)Random.Range(0, System.Enum.GetValues(typeof(ArtTheme)).Length);
            themeParent.transform.GetChild(i).GetComponent<Property>().theme = theme;
            if ((int)theme == solution[0])
            {
                howManyCorrect++;
            }
            Debug.Log("hooh "+i+" " + howManyCorrect);
            themeParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            clues.Add(materialParent.transform.GetChild(i).gameObject);
            ArtMaterial material = (ArtMaterial)Random.Range(0, System.Enum.GetValues(typeof(ArtMaterial)).Length);
            materialParent.transform.GetChild(i).GetComponent<Property>().material = material;
            if ((int)material == solution[1])
            {
                howManyCorrect++;
            }
            Debug.Log("hooh " + i + " " + howManyCorrect);
            materialParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            clues.Add(colorParent.transform.GetChild(i).gameObject);
            ArtColour color = (ArtColour)Random.Range(0, System.Enum.GetValues(typeof(ArtColour)).Length);
            colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
            if ((int)color == solution[2])
            {
                if (howManyCorrect < 2)
                {
                    colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                    //colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
                    howManyCorrect++;
                    Debug.Log("asd1 " + i);
                }
                else
                {
                    
                    while (true)
                    {
                        color = (ArtColour)Random.Range(0, System.Enum.GetValues(typeof(ArtColour)).Length);
                        if ((int)color != solution[2])
                        {
                            colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                            break;
                        }
                        Debug.Log("Here1 "+ i);
                    }
                }
            }
            else if(howManyCorrect == 0)
            {
                while (true)
                {
                    color = (ArtColour)Random.Range(0, System.Enum.GetValues(typeof(ArtColour)).Length);
                    if ((int)color == solution[2])
                    {
                        colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                        howManyCorrect++;
                        break;
                    }
                }
            }
            colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            Debug.Log("hooh " + i + " " + howManyCorrect);
            clues.Add(shapeParent.transform.GetChild(i).gameObject);
            ArtShape shape = (ArtShape)Random.Range(0, System.Enum.GetValues(typeof(ArtShape)).Length);
            shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
            if ((int)shape == solution[3])
            {
                if (howManyCorrect < 2)
                {
                    shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                    //colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
                    howManyCorrect++;
                    Debug.Log("asd2 " + i);
                }
                else
                {
                    
                    while (true)
                    {
                        shape = (ArtShape)Random.Range(0, System.Enum.GetValues(typeof(ArtShape)).Length);
                        if ((int)shape != solution[3])
                        {
                            shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                            break;
                        }
                        Debug.Log("Here2 " + i);
                    }
                }
            }
            else if (howManyCorrect == 1)
            {
               
                while (true)
                {
                    shape = (ArtShape)Random.Range(0, System.Enum.GetValues(typeof(ArtShape)).Length);
                    if ((int)shape == solution[3])
                    {
                        shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                        howManyCorrect++;
                        break;
                    }
                }
            }
            shapeParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            Debug.Log("hooh " + i + " " + howManyCorrect);
        }
        

        Debug.Log("solution "+solution[0]+" "+solution[1]+" "+solution[2]+" "+solution[3]);
        Debug.Log("HA");

}

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        UpdateFoundClues();
    }

    public void UpdateFoundClues()
    {
        // Set visibility depending on amount of clues found
        GameManager manager = FindObjectOfType<GameManager>();
        int cluesFound = 3;
        if (manager) cluesFound = manager.GetCluesFound();
        for (int i = 0; i < clueAmount; i++)
        {
            bool visible = (i < cluesFound);
            themeParent.transform.GetChild(i).gameObject.SetActive(visible);
            colorParent.transform.GetChild(i).gameObject.SetActive(visible);
            materialParent.transform.GetChild(i).gameObject.SetActive(visible);
            shapeParent.transform.GetChild(i).gameObject.SetActive(visible);
        }
    }

    public void HighLightObjects(PropertyType type, int selection)
    {
        //Debug.Log(type);
        switch (type)
        {
            case PropertyType.Theme:
                foreach (GameObject obj in clues)
                {
                    if (type == obj.GetComponent<Property>().propertyType)
                    {
                        if ((int)obj.GetComponent<Property>().theme == selection)
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0);
                        }
                    }

                }
                break;

            case PropertyType.Material:
                foreach (GameObject obj in clues)
                {
                    if (type == obj.GetComponent<Property>().propertyType)
                    {
                        if ((int)obj.GetComponent<Property>().material == selection)
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0);
                        }
                    }

                }
                break;
            case PropertyType.Color:
                foreach (GameObject obj in clues)
                {
                    if (type == obj.GetComponent<Property>().propertyType)
                    {
                        if ((int)obj.GetComponent<Property>().color == selection)
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0);
                        }
                    }

                }
                break;
            case PropertyType.Special:
                foreach (GameObject obj in clues)
                {
                    if (type == obj.GetComponent<Property>().propertyType)
                    {
                        if ((int)obj.GetComponent<Property>().shape == selection)
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0);
                        }
                    }

                }
                break;

        }

    }
    public void SetCorrectSolution()
    {
        solution = new List<int>();
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(ArtTheme)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(ArtMaterial)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(ArtColour)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(ArtShape)).Length));
    }

}
