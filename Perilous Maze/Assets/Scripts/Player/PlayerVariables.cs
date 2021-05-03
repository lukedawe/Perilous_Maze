using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVariables : MonoBehaviour
{
    public int pointsAccumulated = 0;


    // this is the singleton code
    void Awake()
    {
        // find any instances of this gameobject type
        Object instance = GameObject.FindObjectOfType<PlayerVariables>();

        if (instance != null)
        {
            // if the instance is not this class
            if (instance != GetComponent<PlayerVariables>())
            {
                Debug.Log("found");
                Destroy(gameObject);
            }
            else
            {
                // otherwise, there is no other player variables so we can leave things as they are
                Debug.Log("Not found");
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // find the map modifier and make it have a reference to this class
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

    // add points to the player's score
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

