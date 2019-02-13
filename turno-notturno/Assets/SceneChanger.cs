using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    // Start is called before the first frame update
    public void changeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
