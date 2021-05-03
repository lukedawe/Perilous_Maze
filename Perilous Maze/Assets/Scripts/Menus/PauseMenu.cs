using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// from https://www.studica.com/blog/create-ui-unity-tutorial
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform UIPanel; 
    [SerializeField] string timeText;
    public bool isPaused; 

    void Start()
    {
        UIPanel.gameObject.SetActive(false); 
        isPaused = false; 
    }

    void Update()
    {
        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            Pause();
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            UnPause();
    }

    public void Pause()
    {
        isPaused = true;
        UIPanel.gameObject.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void UnPause()
    {
        isPaused = false;
        UIPanel.gameObject.SetActive(false); 
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().variables.ResetPoints();
        SceneManager.LoadScene(0);
    }
}