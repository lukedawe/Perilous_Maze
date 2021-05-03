using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] Transform UIPanel; 
    [SerializeField] string timeText; 
    bool isPaused; 


    void Start()
    {
        UIPanel.gameObject.SetActive(false);
        isPaused = false; 
    }

    public void ShowPanel()
    {
        Pause();
    }

    public void Pause()
    {
        isPaused = true;
        UIPanel.gameObject.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        enabled = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
