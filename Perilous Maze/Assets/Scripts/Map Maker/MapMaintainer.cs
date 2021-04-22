using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class MapMaintainer : MonoBehaviour
{

    [HideInInspector] public List<Vector3> PointsGrid;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public Vector3 PointClosestToPlayer;
    [HideInInspector] public bool GameWon = false;


    // Update is called once per frame
    void Update()
    {
        PointClosestToPlayer = VectorMaths.FindPointClosestToEntity(Player.transform, PointsGrid);
        Vector3[] playerToPoint = { Player.transform.position, PointClosestToPlayer };
        if (PointClosestToPlayer == GetComponent<NewMapCreator>().EndPoint && !GameWon)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        GameObject.Find("Menu Controller").GetComponent<WinMenu>().ShowPanel();
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
