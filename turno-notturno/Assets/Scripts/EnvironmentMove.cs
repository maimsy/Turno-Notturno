using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnvironmentMove : MonoBehaviour
{
    [SerializeField] float gameSpeed;
    [SerializeField] float accelerator = 0.01f;
    [SerializeField] List<Transform> artPieces;

    private List<float> layerSpeeds;
    private List<GameObject[]> layers;
    private List<List<Vector3>> layerPositions;
    private float speedUp = 1f;
    private float timer = 0;
    private float timeLimit = 0.5f;
    private P2Dmovement playerMovement;
    private Vector3 originalPos;

    private Vector3 fallFollowVelocity = Vector3.zero;
    private float fallFollowDampening = 0.3f;

    private MinigameState state = MinigameState.playing;

    enum MinigameState
    {
        playing,
        falling
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        layerSpeeds = new List<float>();
        layers = new List<GameObject[]>();
        layerSpeeds.Add(gameSpeed * 0.4f);
        layerSpeeds.Add(gameSpeed * 0.6f);
        layerSpeeds.Add(gameSpeed * 0.8f);
        layerSpeeds.Add(gameSpeed * 0.9f);
        layerSpeeds.Add(gameSpeed);
        int i = 1;
        try
        {
            while (GameObject.FindGameObjectsWithTag("Layer" + i.ToString()).Length != 0)
            {
                layers.Add(GameObject.FindGameObjectsWithTag("Layer" + i.ToString()));
                i++;
            }
        }
        catch { }
        
        //Debug.Log("layers " + layers.Count);
        //Debug.Log("objects " + layers[0].Length);
        playerMovement = FindObjectOfType<P2Dmovement>();
        playerMovement.speed = gameSpeed * 60;

        layerPositions = new List<List<Vector3>>();
        for (int layer = 0; layer < layers.Count; layer++)
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (var subobject in layers[layer])
            {
                positions.Add(subobject.transform.position);
            }
            layerPositions.Add(positions);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveLayers();

        MoveArt();
        playerMovement.speed = gameSpeed * 60 * speedUp;
        Vector3 pos = Camera.main.transform.position;
        pos.x += gameSpeed * speedUp;
        pos.x = Mathf.Max(pos.x, playerMovement.transform.position.x);  // Player cannot walk faster than camera
        if (state == MinigameState.playing)
        {
            // Normal movement
            Camera.main.transform.position = pos;

            // Smooth falling after reset
            Camera.main.transform.position = Vector3.SmoothDamp(transform.position, new Vector3(pos.x, originalPos.y, pos.z), ref fallFollowVelocity, fallFollowDampening);

            // Acceleration
            timer += Time.fixedDeltaTime;
            if (timer > timeLimit)
            {
                timer = 0;
                speedUp += accelerator;
            }
        }
        else
        {
            // Follow player smoothly while falling
            Camera.main.transform.position = pos;
            pos.y = playerMovement.transform.position.y;
            Camera.main.transform.position = Vector3.SmoothDamp(transform.position, pos, ref fallFollowVelocity, fallFollowDampening);
            if (pos.y < -20)
            {
                Reset();
            }
        }

        
    }

    private void MoveLayers()
    {

        float yOffset = Camera.main.transform.position.y - originalPos.y;
        for (int i = 0; i < layers.Count; i++)
        {
            foreach (GameObject obj in layers[i])
            {
                Vector3 pos = obj.transform.position;
                obj.transform.position = new Vector3(pos.x + layerSpeeds[i] * speedUp, pos.y, pos.z);
            }
        }
        /*
        for (int layer = 0; layer < layers.Count; layer++)
        {
            for (int i = 0; i < layers[layer].Length; i++)
            {
                float originalY = layerPositions[layer][i].y;
                GameObject obj = layers[layer][i];
                Vector3 pos = obj.transform.position;
                pos.x += layerSpeeds[layer] * speedUp;
                if (layer > 2)
                {
                    // Only some layers fall with the camera
                    pos.y = originalY + yOffset;
                }
                
                obj.transform.position = pos;
            }
        }*/
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

    public void SmoothReset()
    {
        StudioEventEmitter sound = GameObject.Find("BirthSound").GetComponent<StudioEventEmitter>();
        if (!sound.IsPlaying())
        {
            sound.Play();
        }
        if (state == MinigameState.falling) return;  // Reset already in progress
        fallFollowVelocity = Vector3.zero;
        speedUp = 1f;
        timer = 0;
        state = MinigameState.falling;
        playerMovement.speed = gameSpeed * 60;
        playerMovement.GetComponent<CircleCollider2D>().isTrigger = true;
        //Invoke("Reset", 1f);
    }

    private void Reset()
    {
        state = MinigameState.playing;
        GameObject start = GameObject.Find("PlayerSpawn");
        playerMovement.transform.position = start.transform.position;
        transform.position = originalPos;
        transform.position += new Vector3(0, 10, 0);
        playerMovement.GetComponent<CircleCollider2D>().isTrigger = false;
        ResetArt();
        ResetLayers();
    }

    private void ResetArt()
    {
        foreach (var artPiece in artPieces)
        {
            artPiece.GetComponent<ArtPieceDragging>().Reset();
        }
    }

    private void ResetLayers()
    {
        for (int layer = 0; layer < layers.Count; layer++)
        {
            for (int i = 0; i < layers[layer].Length; i++)
            {
                layers[layer][i].transform.position = layerPositions[layer][i];
            }
        }
    }

    
}
