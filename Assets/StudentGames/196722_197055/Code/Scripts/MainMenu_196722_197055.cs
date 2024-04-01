using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu_196722_197055 : MonoBehaviour
{
    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadSceneAsync("196722_197055");
    }
    public void OnExitToDesktopButtonPressed()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
