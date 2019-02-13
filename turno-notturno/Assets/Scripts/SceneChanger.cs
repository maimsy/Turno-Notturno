using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public void changeScene(string name)
    {
        FindObjectOfType<GameManager>().SavePlayerPosition();

        //Only useful for the drawing game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        

        SceneManager.LoadScene(name);
    }
}
