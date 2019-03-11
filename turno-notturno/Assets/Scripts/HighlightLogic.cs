using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightLogic : MonoBehaviour
{

    public PropertyType propertyType;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindGameObjectWithTag("PuzzleRandomizer").GetComponent<PuzzleRandomizer>().HighLightObjects(propertyType, GetComponent<Dropdown>().value);
       FindObjectOfType<FinalPuzzle>().HighLightObjects(propertyType, GetComponent<Dropdown>().value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Highlight()
    {
       //GameObject.FindGameObjectWithTag("PuzzleRandomizer").GetComponent<PuzzleRandomizer>().HighLightObjects(propertyType, GetComponent<Dropdown>().value);
        FindObjectOfType<FinalPuzzle>().HighLightObjects(propertyType, GetComponent<Dropdown>().value);
    }
}
