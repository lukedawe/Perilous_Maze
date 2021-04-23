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
    [HideInInspector] float timeTaken;
    [HideInInspector] float pointsEarned;
    [SerializeField] GameObject pointsDisplay;
    public PlayerVariables variables;


    // Update is called once per frame
    void Update()
    {
        timeTaken += Time.deltaTime;
        PointClosestToPlayer = VectorMaths.FindPointClosestToEntity(Player.transform, PointsGrid);
        Vector3[] playerToPoint = { Player.transform.position, PointClosestToPlayer };
        if (PointClosestToPlayer == GetComponent<NewMapCreator>().EndPoint && !GameWon)
        {
            GameWon = true;
            GameWin();
        }
    }

    public void GameWin()
    {
        pointsEarned += (1 / timeTaken) * PointsGrid.Count * 100;
        variables.addPoints(pointsEarned);

        Debug.Log(variables.pointsAccumulated);

        GameObject.Find("Menu Controller").GetComponent<WinMenu>().ShowPanel();
        pointsDisplay.GetComponent<Text>().text = "Points: " + variables.pointsAccumulated.ToString();
    }

    void ResetMap()
    {
        return;
    }

    public void GameLost()
    {
        GameObject.Find("Menu Controller").GetComponent<GameLost>().ShowPanel();
    }
}
