using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
//http://blog.yellowmoleproductions.pl/handwriting/

public class TextAnimation : MonoBehaviour
{
    public Text text;
    public int speed = 15;

    private void Start()
    {
        text = this.GetComponent<Text>();
    }


    private void OnEnable()
    {;
        StartCoroutine("WriteTextOut", 500f);
    }

    private void OnDisable()
    {
        
    }

    public void ColorVertices(int lettersShown)
    {
        //we color the vertices that do not belong to the current displayed letter blue;
        UIVertex tempVertex;
        for (int i = Mathf.Max((lettersShown - 1) * 4, 0); i < lettersShown * 4; i++)
        {
            tempVertex = text.cachedTextGenerator.verts[i];
            tempVertex.color = Color.blue;
            text.cachedTextGenerator.verts[i] = tempVertex;
        }
        //we color the vertices that belong to the current displayed letter green
        for (int i = lettersShown * 4; i < lettersShown * 4 + 4; i++)
        {
            tempVertex = text.cachedTextGenerator.verts[i];
            tempVertex.color = Color.green;
            text.cachedTextGenerator.verts[i] = tempVertex;
        }
        //we color the vertices that come after the current displayed letter red
        for (int i = (lettersShown + 1) * 4; i < text.cachedTextGenerator.verts.Count; i++)
        {
            tempVertex = text.cachedTextGenerator.verts[i];
            tempVertex.color = Color.red;
            text.cachedTextGenerator.verts[i] = tempVertex;
        }
        //needed to pass this information on
        text.SetVerticesDirty();
    }

    IEnumerator WriteTextOut()
    {
        //We put color of a text on the material, for further use;
        text.material.SetColor("_Color", text.color);


        yield return new WaitForSeconds(0.5f);
        int lettersShown = 0;
        text.material.SetFloat("_IsAnimating", 1);
        while (lettersShown < text.text.Length)
        {
            ColorVertices(lettersShown);
            //We do not want to waste time on spaces
            if (!char.IsWhiteSpace(text.text[lettersShown]))
            {
                float cutoff = 1;
                while (cutoff > 0)
                {
                    cutoff = Mathf.Max(0, cutoff - Time.deltaTime * speed);
                    text.material.SetFloat("_Cutoff", cutoff);
                    yield return new WaitForEndOfFrame();
                }
            }
            lettersShown++;
        }
        text.material.SetFloat("_IsAnimating", 0);
    }
}
