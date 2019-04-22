using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using System.Linq;


public class DrawingLogic : MonoBehaviour
{
    public GameObject drawingPrefab;
    public GameObject drawingLetterPrefab;
    public GameObject inkMeter;
    public StudioEventEmitter letterSound;

    private string letters;
    private string originalLetters = "One day I will take you to the highest skyscraper there is. " +
                                  "We will sleep on the rooftop under a blanket of stars. " +
                                  "Feel the world spinning. " +
                                  "Stare into the eternity of the Universe. " +
                                  "Like the freest man alive, without the worries of the world underneath us." +
        "Your father was the kindest man I had ever met. I really, really miss him. I’m lucky to have you. You have the same eyes as him." +
        "I told you many times, your teeth will rot and decay and blacken " +
                                  "and fill with worms and fall all over the floor if you don’t take care of them. " +
                                  "Now go wash your teeth. I will check when it’s time to sleep";
    private List<Vector2> positions;
    private float drawDistance = 0.1f;
    private int currentLetter = 0;
    private int inkUsed = 0;
    private string lettersUsed;
    [SerializeField] int inkAmount = 42;

    // Start is called before the first frame update
    void Awake()
    {
        positions = new List<Vector2>();
        drawDistance = drawingPrefab.GetComponent<CircleCollider2D>().radius * drawingPrefab.transform.localScale.x;
        SetLetters();
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
        if (inkUsed < inkAmount && !Physics2D.OverlapCircle(new Vector3(worldPos.x,worldPos.y,0), drawDistance*0.5f))
        {
            GameObject obj = Instantiate(drawingPrefab, worldPos, Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            GameObject letter = Instantiate(drawingLetterPrefab, worldPos, Quaternion.identity);
            letter.transform.position = new Vector3(letter.transform.position.x, letter.transform.position.y, 10);
            positions.Add(letter.transform.position);
            letter.transform.GetChild(0).GetComponent<Text>().text = letters[currentLetter].ToString();
            lettersUsed += letters[currentLetter];
            float scale = 2*drawDistance/ letter.GetComponent<RectTransform>().sizeDelta.x;
            letter.transform.localScale = new Vector2(scale, scale);
            currentLetter = currentLetter + 1 >= letters.Length ? 0 : currentLetter + 1;
            inkUsed++;
            inkMeter.GetComponent<Slider>().value = 1 - (float) inkUsed / inkAmount;
            letterSound.Play();
            //obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
        }
    }

    private void SetLetters()
    {
        lettersUsed = "";
        letters = originalLetters;
        letters = string.Concat(letters.Where(c => !char.IsWhiteSpace(c)));
        letters = string.Concat(letters.Where(c => (',' != c)));
        int count = letters.Split('.').Length;
        int startPoint = Random.Range(0, count);
        int found = 0;
        
        for (int i = 0; i < letters.Length; i++)
        {
            if (found == startPoint)
            {
                currentLetter = i;
                break;
            }
            if (letters[i] == '.')
            {
                found++;
            }
        }
        letters = string.Concat(letters.Where(c => ('.' != c)));
        currentLetter -= startPoint;
    }

    public void Reset()
    {
        inkUsed = 0;
        inkMeter.GetComponent<Slider>().value = 1;
        SetLetters();
        positions = new List<Vector2>();
    }
    public string GetLetters()
    {
        return lettersUsed;
    }

    public List<Vector2> GetPositions()
    {
        return positions;
    }
    
}
