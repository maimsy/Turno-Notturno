using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Property : MonoBehaviour
{
    public PropertyType propertyType;
    public ArtTheme theme;
    public ArtMaterial material;
    public ArtColour color;
    public ArtShape shape;


    public void UpdateValue()
    {
        propertyType = (PropertyType)System.Enum.Parse(typeof(PropertyType), tag);
        //Debug.Log(propertyType);
        switch (propertyType)
        {
            case PropertyType.Theme:
                transform.GetChild(0).GetComponent<Text>().text = theme.ToString();
                break;
            case PropertyType.Material:
                transform.GetChild(0).GetComponent<Text>().text = material.ToString();
                break;
            case PropertyType.Color:
                transform.GetChild(0).GetComponent<Text>().text = color.ToString();
                break;
            case PropertyType.Special:
                transform.GetChild(0).GetComponent<Text>().text = shape.ToString();
                //transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + shape.ToString());
                break;

        }

    }
}
