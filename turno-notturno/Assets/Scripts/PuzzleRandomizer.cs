using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PropertyType
{
    Theme,
    Material,
    Color,
    Shape
}
public enum Theme
{
    A,
    B,
    C
}
public enum Material
{
    X,
    Y,
    Z
}
public enum Color
{
    R,
    G,
    B
}
public enum Shape
{
    Circle,
    Square,
    Triangle
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
            Theme theme = (Theme)Random.Range(0, System.Enum.GetValues(typeof(Theme)).Length);
            themeParent.transform.GetChild(i).GetComponent<Property>().theme = theme;
            if ((int)theme == solution[0])
            {
                howManyCorrect++;
            }
            themeParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            clues.Add(materialParent.transform.GetChild(i).gameObject);
            Material material = (Material)Random.Range(0, System.Enum.GetValues(typeof(Material)).Length);
            materialParent.transform.GetChild(i).GetComponent<Property>().material = material;
            if ((int)material == solution[1])
            {
                howManyCorrect++;
            }
            materialParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
            clues.Add(colorParent.transform.GetChild(i).gameObject);
            Color color = (Color)Random.Range(0, System.Enum.GetValues(typeof(Color)).Length);
            if ((int)color == solution[2])
            {
                if (howManyCorrect < 2)
                {
                    colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                    //colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
                    howManyCorrect++;
                }
                else
                {
                    bool found = false;
                    while (!found)
                    {
                        color = (Color)Random.Range(0, System.Enum.GetValues(typeof(Color)).Length);
                        if ((int)color != solution[2])
                        {
                            colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                            found = true;
                        }
                    }
                }
            }
            else if(howManyCorrect == 0)
            {
                bool found = false;
                while (!found)
                {
                    color = (Color)Random.Range(0, System.Enum.GetValues(typeof(Color)).Length);
                    if ((int)color == solution[2])
                    {
                        colorParent.transform.GetChild(i).GetComponent<Property>().color = color;
                        howManyCorrect++;
                        found = true;
                    }
                }
            }
            colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();

            clues.Add(shapeParent.transform.GetChild(i).gameObject);
            Shape shape = (Shape)Random.Range(0, System.Enum.GetValues(typeof(Shape)).Length);
            if ((int)shape == solution[3])
            {
                if (howManyCorrect < 2)
                {
                    shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                    //colorParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
                    howManyCorrect++;
                }
                else
                {
                    bool found = false;
                    while (!found)
                    {
                        shape = (Shape)Random.Range(0, System.Enum.GetValues(typeof(Shape)).Length);
                        if ((int)shape != solution[3])
                        {
                            shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                            found = true;
                        }
                    }
                }
            }
            else if (howManyCorrect == 1)
            {
                bool found = false;
                while (!found)
                {
                    shape = (Shape)Random.Range(0, System.Enum.GetValues(typeof(Shape)).Length);
                    if ((int)shape == solution[3])
                    {
                        shapeParent.transform.GetChild(i).GetComponent<Property>().shape = shape;
                        howManyCorrect++;
                        found = true;
                    }
                }
            }
            shapeParent.transform.GetChild(i).GetComponent<Property>().UpdateValue();
        }


        Debug.Log("solution "+solution[0]+" "+solution[1]+" "+solution[2]+" "+solution[3]);
        Debug.Log("HA");

}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighLightObjects(PropertyType type, int selection)
    {
        //Debug.Log(type);
        switch(type)
        {
            case PropertyType.Theme:
                foreach (GameObject obj in clues)
                {
                    if(type == obj.GetComponent<Property>().propertyType)
                    {
                        if ( (int)obj.GetComponent<Property>().theme == selection)
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
                    if(type == obj.GetComponent<Property>().propertyType)
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
                    if(type == obj.GetComponent<Property>().propertyType)
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
            case PropertyType.Shape:
                foreach (GameObject obj in clues)
                {
                    if(type == obj.GetComponent<Property>().propertyType)
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
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(Theme)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(Material)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(Color)).Length));
        solution.Add(Random.Range(0, System.Enum.GetValues(typeof(Shape)).Length));
    }

}
