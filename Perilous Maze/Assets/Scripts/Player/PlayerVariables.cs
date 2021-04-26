using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVariables : MonoBehaviour
{
    public int pointsAccumulated = 0;


    void Awake()
    {
        Object instance = GameObject.FindObjectOfType<PlayerVariables>();

        if (instance != null)
        {
            if (instance != GetComponent<PlayerVariables>())
            {
                Debug.Log("found");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Not found");
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject modifier = GameObject.Find("Map Modifier");
        if (modifier)
        {
            modifier.GetComponent<MapMaintainer>().variables = this;
        }
        else{
            Debug.LogError("modifier not found");
        }
    }

    public void ResetPoints()
    {
        pointsAccumulated = 0;
    }

    public void addPoints(float points)
    {
        this.pointsAccumulated += (int)points;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");

            if (pointsAccumulated > highScore)
            {
                PlayerPrefs.SetInt("HighScore", pointsAccumulated);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", pointsAccumulated);
        }
    }
}

