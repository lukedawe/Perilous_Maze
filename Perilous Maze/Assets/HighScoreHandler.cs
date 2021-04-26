using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreHandler : MonoBehaviour
{
    [SerializeField] Text HighScore;

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighScore.text = "High score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }
        else
        {
            Debug.Log("No highscore found");
        }
    }
}
