using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLost : MonoBehaviour
{
    [SerializeField] Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it
    [SerializeField] string timeText; //Will assign our Time Text to this variable so we can modify the text it displays.
    bool isPaused; //Used to determine paused state


    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens
    }

    public void ShowPanel()
    {
        enabled = true;
        Pause();
    }

    public void Pause()
    {
        isPaused = true;
        UIPanel.gameObject.SetActive(true); //turn on the pause menu
        Time.timeScale = 0f; //pause the game
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        enabled = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
