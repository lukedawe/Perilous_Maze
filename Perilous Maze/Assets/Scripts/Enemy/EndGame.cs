using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        // when we collide with something, check if it is the player and then end the game if it is
        if (c.gameObject.tag == "Player")
        {
            GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().GameLost();
            Debug.Log("<color='red'>Game Over!</color>");
        }
    }
}
