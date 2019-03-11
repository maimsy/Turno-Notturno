using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMove : MonoBehaviour
{
    [SerializeField] float gameSpeed;
    [SerializeField] float accelerator = 0.01f;
    [SerializeField] List<Transform> artPieces;

    private List<float> layerSpeeds;
    private List<GameObject[]> layers;
    private float speedUp = 1f;
    private float timer = 0;
    private float timeLimit = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        layerSpeeds = new List<float>();
        layers = new List<GameObject[]>();
        layerSpeeds.Add(gameSpeed * 0.4f);
        layerSpeeds.Add(gameSpeed * 0.6f);
        layerSpeeds.Add(gameSpeed * 0.8f);
        layerSpeeds.Add(gameSpeed * 0.9f);
        layerSpeeds.Add(gameSpeed);
        layers.Add(GameObject.FindGameObjectsWithTag("Layer1"));
        layers.Add(GameObject.FindGameObjectsWithTag("Layer2"));
        layers.Add(GameObject.FindGameObjectsWithTag("Layer3"));
        layers.Add(GameObject.FindGameObjectsWithTag("Layer4"));
        layers.Add(GameObject.FindGameObjectsWithTag("Layer5"));
        Debug.Log("layers " + layers.Count);
        Debug.Log("objects " + layers[0].Length);
        GameObject.Find("Player").GetComponent<P2Dmovement>().speed = gameSpeed * 60;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("WHUH");
        MoveLayers();
        MoveArt();
        GameObject.Find("Player").GetComponent<P2Dmovement>().speed = gameSpeed * 60 * speedUp;
        Vector3 pos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(pos.x + gameSpeed * speedUp, pos.y, pos.z);
        timer += Time.deltaTime;
        if (timer > timeLimit)
        {
            timer = 0;
            speedUp += accelerator;
        }
    }

    private void MoveLayers()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            foreach (GameObject obj in layers[i])
            {
                Vector3 pos = obj.transform.position;
                obj.transform.position = new Vector3(pos.x + layerSpeeds[i] * speedUp, pos.y, pos.z);
            }
        }
    }

    private void MoveArt()
    {
        foreach (Transform art in artPieces)
        {
            if (art.GetComponent<BoxCollider2D>().isTrigger)
            {
                art.position = new Vector3(art.position.x + gameSpeed * speedUp, art.position.y, art.position.z);
            }
        }
    }
}
