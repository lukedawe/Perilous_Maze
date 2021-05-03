using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;
using UnityEngine.UI;

public class MapMaintainer : MonoBehaviour
{

    [HideInInspector] public List<Vector3> PointsGrid;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Vector3 PointClosestToPlayer;
    [HideInInspector] public bool GameWon = false;
    // used for scoring the player
    [HideInInspector] float timeTaken;
    [HideInInspector] float pointsEarned;
    [SerializeField] GameObject pointsDisplay;
    public PlayerVariables variables;
    [SerializeField] Text pointsDisplayHUD;
    [SerializeField] AudioSource winSound;


    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
        PointClosestToPlayer = VectorMaths.FindPointClosestToEntity(Player.transform, PointsGrid);
        UpdateHUD();
        if (PointClosestToPlayer == GetComponent<NewMapCreator>().EndPoint && !GameWon)
        {
            GameWon = true;
            GameWin();
        }
    }

    public void GameWin()
    {
        winSound.Play();
        pointsEarned += (1 / timeTaken) * PointsGrid.Count * 100;
        variables.addPoints(pointsEarned);

        GameObject.Find("Menu Controller").GetComponent<WinMenu>().ShowPanel();
        pointsDisplay.GetComponent<Text>().text = "Points: " + variables.pointsAccumulated.ToString();
    }

    void ResetMap()
    {
        return;
    }

    public void GameLost()
    {
        variables.ResetPoints();
        GameObject.Find("Menu Controller").GetComponent<GameLost>().ShowPanel();
    }

    public void UpdateHUD()
    {
        pointsDisplayHUD.text = "Points: " + variables.pointsAccumulated.ToString();
    }
}
