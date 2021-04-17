using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    public float Speed;
    public float TurnSpeed;
    public GameObject[,] Maze;
    public List<Vector3> PointsGrid;
    public GameObject Player;

    public void Constructor()
    {
        this.Maze = GameObject.Find("Map Modifier").GetComponent<NewMapCreator>().Maze;
        this.PointsGrid = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().PointsGrid;
        this.Player = GameObject.Find("Map Modifier").GetComponent<NewMapCreator>().Player;
    }
}
