using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScribbleManager : MonoBehaviour
{
    private string pathScribble = "/Resources/Scribble.txt";
    private string pathPositions = "/Resources/ScribblePositions.txt";
    private string localPath;
    private bool build = false;
    [SerializeField] GameObject letter;
    private void Start()
    {
       
        localPath = Application.dataPath;
        Debug.LogError(localPath);
        if (localPath.Contains("build"))
        {
            build = true;
            pathScribble += "/Scribble.txt";
            pathPositions += "/ScribblePositions.txt";
        }
        else
        {
            pathScribble = localPath + pathScribble;
            pathPositions = localPath + pathPositions;
        }
        
        //Debug.Log(pathScribble);
        //Debug.Log(pathPositions);
    }
    public void SaveString(string str, List<Vector2> positions)
    {
        FileStream stream = new FileStream(pathScribble, FileMode.Truncate);
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(str);
        writer.Close();
        stream.Close();
        stream = new FileStream(pathPositions, FileMode.Truncate);
        writer = new StreamWriter(stream);
        foreach(Vector2 pos in positions)
        {
            writer.WriteLine(pos.x.ToString() + ","+ pos.y.ToString());
        }
        writer.Close();
        stream.Close();
    }

    public void ScribbleOnFloor()
    {
        StreamReader reader = new StreamReader(pathScribble);
        string scribble = reader.ReadToEnd();
        reader.Close();
        reader = new StreamReader(pathPositions);
        string pos = reader.ReadToEnd();
        string[] positions = pos.Split("\n"[0]);
        for(int i = 0; i < scribble.Length; i++)
        {
            if (positions[i].Length == 0) break;
            string[] coordinates = positions[i].Split(","[0]);
            GameObject obj = Instantiate(letter, transform);
            obj.transform.localPosition 
                = new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), 0);
            obj.GetComponent<TextMesh>().text = scribble[i].ToString();

        }

    }
}
