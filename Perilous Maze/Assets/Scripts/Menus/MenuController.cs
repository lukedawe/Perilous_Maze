using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject winMenu;

    // Update is called once per frame
    void Update()
    {
        if (winMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            pauseMenu.GetComponent<PauseMenu>().isPaused = true;
        }
    }
}
