using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;


public class DrawingLogic : MonoBehaviour
{
    public GameObject drawingPrefab;
    public GameObject drawingLetterPrefab;
    public GameObject inkMeter;
    public StudioEventEmitter letterSound;

    private string letters = "Someverycreepystuffthatcreepsyououtverymuch";
    private List<Vector2> positions;
    private float drawDistance = 0.1f;
    private int currentLetter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        positions = new List<Vector2>();
        drawDistance = drawingPrefab.GetComponent<CircleCollider2D>().radius * drawingPrefab.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Draw();
        }
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<ScribbleManager>().SaveString(GetLetters(), positions);
        }

    }

    private void Draw()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentLetter < letters.Length && !Physics2D.OverlapCircle(new Vector3(worldPos.x,worldPos.y,0), drawDistance*0.5f))
        {
            GameObject obj = Instantiate(drawingPrefab, worldPos, Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            GameObject letter = Instantiate(drawingLetterPrefab, worldPos, Quaternion.identity);
            letter.transform.position = new Vector3(letter.transform.position.x, letter.transform.position.y, 10);
            positions.Add(letter.transform.position);
            letter.transform.GetChild(0).GetComponent<Text>().text = letters[currentLetter].ToString();
            float scale = 2*drawDistance/ letter.GetComponent<RectTransform>().sizeDelta.x;
            letter.transform.localScale = new Vector2(scale, scale);
            currentLetter++;
            inkMeter.GetComponent<Slider>().value = 1 - (float) currentLetter / letters.Length;
            letterSound.Play();
            //obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
        }
    }

    public void Reset()
    {
        currentLetter = 0;
        inkMeter.GetComponent<Slider>().value = 1;
    }
    public string GetLetters()
    {
        return letters.Substring(0,currentLetter);
    }

    public List<Vector2> GetPositions()
    {
        return positions;
    }
}
