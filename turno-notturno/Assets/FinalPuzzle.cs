using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalPuzzle : MonoBehaviour
{
    [SerializeField] List<Colour> artColours;
    [SerializeField] List<Theme> artThemes;
    [SerializeField] List<Material> artMaterials;
    [SerializeField] List<Special> artSpecials;
    [SerializeField] GameObject winText;
    private List<GameObject> clues;

    private struct Solution
    {
        public Colour color;
        public Theme theme;
        public Material material;
        public Special special;
    }
   

    private GameObject themes;
    private GameObject materials;
    private GameObject colors;
    private GameObject specials;
    private Solution solution;
    private List<int> highlightsPerArt;
    // Start is called before the first frame update
    void Awake()
    {
        highlightsPerArt = new List<int>(new int[] { 0, 0, 0, 0, 0, 0 });
        solution = new Solution();
        solution.color = Colour.Blue;
        solution.theme = Theme.Abstract;
        solution.material = Material.Copper;
        solution.special = Special.Spirals;
        themes = GameObject.Find("Themes");
        materials = GameObject.Find("Materials");
        colors = GameObject.Find("Colors");
        specials = GameObject.Find("Specials");
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
        for (int i = 0; i < specials.transform.childCount; i++)
        {
            clues.Add(specials.transform.GetChild(i).gameObject);
            Property property = specials.transform.GetChild(i).GetComponent<Property>();
            property.special = artSpecials[i];
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
                        if ((int)obj.GetComponent<Property>().special == selection)
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
