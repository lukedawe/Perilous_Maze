using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class OptionsHandler : MonoBehaviour
{
    [SerializeField] Dropdown options;


    public void BeginGame()
    {
        switch (options.value)
        {
            case 0:
                PlayerPrefs.SetString("Difficulty", "Easy");
                break;
            case 1:
                PlayerPrefs.SetString("Difficulty", "Medium");
                break;
            case 2:
                PlayerPrefs.SetString("Difficulty", "Hard");
                break;
            default:
                PlayerPrefs.SetString("Difficulty", "Medium");
                break;
        }

        Debug.Log("This is your playerpref: " + PlayerPrefs.GetString("Difficulty"));

        SceneManager.LoadScene(1);
    }

    public void LaunchSettings()
    {
        SceneManager.LoadScene(2);
    }

    public void LaunchTutorial()
    {
        SceneManager.LoadScene(3);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
