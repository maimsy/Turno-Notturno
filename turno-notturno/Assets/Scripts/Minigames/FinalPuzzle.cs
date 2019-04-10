using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalPuzzle : MonoBehaviour
{
    [SerializeField] List<ArtColour> artColours;
    [SerializeField] List<ArtTheme> artThemes;
    [SerializeField] List<ArtMaterial> artMaterials;
    [SerializeField] List<ArtShape> artShapes;
    [SerializeField] GameObject winText;
    private List<GameObject> clues;

    private struct Solution
    {
        public ArtColour color;
        public ArtTheme theme;
        public ArtMaterial material;
        public ArtShape special;
    }
   

    private GameObject themes;
    private GameObject materials;
    private GameObject colors;
    private GameObject shapes;
    private Solution solution;
    private List<int> highlightsPerArt;
    // Start is called before the first frame update
    void Awake()
    {
        highlightsPerArt = new List<int>(new int[] { 0, 0, 0, 0, 0, 0 });
        solution = new Solution();
        solution.color = ArtColour.Blue;
        solution.theme = ArtTheme.Abstract;
        solution.material = ArtMaterial.Copper;
        solution.special = ArtShape.Spirals;
        themes = GameObject.Find("Themes");
        materials = GameObject.Find("Materials");
        colors = GameObject.Find("Colors");
        shapes = GameObject.Find("Shapes");
        clues = new List<GameObject>();
        for (int i = 0; i < themes.transform.childCount; i++)
        {
            clues.Add(themes.transform.GetChild(i).gameObject);
            Property property = themes.transform.GetChild(i).GetComponent<Property>();
            property.theme = artThemes[i];
            property.UpdateValue();

        }
        for (int i = 0; i < materials.transform.childCount; i++)
        {
            clues.Add(materials.transform.GetChild(i).gameObject);
            Property property = materials.transform.GetChild(i).GetComponent<Property>();
            property.material = artMaterials[i];
            property.UpdateValue();
        }
        for (int i = 0; i < colors.transform.childCount; i++)
        {
            clues.Add(colors.transform.GetChild(i).gameObject);
            Property property = colors.transform.GetChild(i).GetComponent<Property>();
            property.color = artColours[i];
            property.UpdateValue();
        }
        for (int i = 0; i < shapes.transform.childCount; i++)
        {
            clues.Add(shapes.transform.GetChild(i).gameObject);
            Property property = shapes.transform.GetChild(i).GetComponent<Property>();
            property.shape = artShapes[i];
            property.UpdateValue();
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
                        int index = clues.IndexOf(obj)%6;
                        if ((int)obj.GetComponent<Property>().theme == selection)
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 0)))
                            {
                                highlightsPerArt[index]++;
                            }
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);

                        }
                        else
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 1)))
                            {
                                highlightsPerArt[index]--;
                            }
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
                        int index = clues.IndexOf(obj) % 6;
                        if ((int)obj.GetComponent<Property>().material == selection)
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 0)))
                            {
                                highlightsPerArt[index]++;
                            }
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            if(obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 1)))
                            {
                                highlightsPerArt[index]--;
                            }
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
                        int index = clues.IndexOf(obj) % 6;
                        if ((int)obj.GetComponent<Property>().color == selection)
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 0)))
                            {
                                highlightsPerArt[index]++;
                            }
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            if(obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 1)))
                            {
                                highlightsPerArt[index]--;
                            }
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
                        int index = clues.IndexOf(obj) % 6;
                        if ((int)obj.GetComponent<Property>().shape == selection)
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 0)))
                            {
                                highlightsPerArt[index]++;
                            }
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 1);
                        }
                        else
                        {
                            if (obj.GetComponent<Image>().color.Equals(new Color(1, 1, 1, 1)))
                            {
                                highlightsPerArt[index]--;
                            }
                            obj.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1, 0);
                        }
                    }

                }
                break;

        }
        for(int i = 0; i < highlightsPerArt.Count; i++)
        {
            if(highlightsPerArt[i] != 2)
            {
                break;
            }
            else if(i == 5)
            {
                Win();
            }
        }

    }
    private void Win()
    {
        winText.SetActive(true);
    }
}
