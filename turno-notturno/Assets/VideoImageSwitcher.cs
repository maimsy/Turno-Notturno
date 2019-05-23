using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoImageSwitcher : MonoBehaviour
{
    private float speed = 0.2f;
    private List<Sprite> sprites;
    private bool canSwitch = true;
    private Image image;
    private int index = 0;
    private int max = 361;
    // Start is called before the first frame update
    void Start()
    {
        sprites = new List<Sprite>();
        string filler = "";
        //Images = Images.GetRange(0, 73);
        for(int i = 1; i <= max; i += 5)
        {
            if (i < 100) filler = "0";
            if (i < 10) filler = "00";
            if (i >= 100) filler = "";
            sprites.Add(Resources.Load<Sprite>("Images/scene00" + filler + i.ToString()));
        }
        image = GetComponent<Image>();
        image.sprite = sprites[index];
        index++;
        if (index == sprites.Count) index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(canSwitch)
        {
            StartCoroutine(Switch());
            canSwitch = false;
        }
    }

    IEnumerator Switch()
    {
        yield return new WaitForSeconds(speed);
        image.sprite = sprites[index];
        index++;
        if (index == sprites.Count) index = 0;
        canSwitch = true;
    }
}
