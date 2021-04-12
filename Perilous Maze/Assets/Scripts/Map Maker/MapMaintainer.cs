using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class MapMaintainer : MonoBehaviour
{

    public List<Vector3> PointsGrid;
    public GameObject Player;
    public Vector3 PointClosestToPlayer;

    // Update is called once per frame
    void Update()
    {
        PointClosestToPlayer = VectorMaths.FindPointClosestToEntity(Player.transform, PointsGrid);
        Vector3[] playerToPoint = { Player.transform.position, PointClosestToPlayer };
    }
}
