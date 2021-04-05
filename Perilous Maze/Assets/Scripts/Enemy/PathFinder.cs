using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class PathFinder : MonoBehaviour
{
    public List<Vector3> PointsGrid;
    private Vector3 ClosestPointToSelf;
    private Vector3 ClosestPointToPlayer;
    private GameObject Player;
    public List<List<Vector3>> RoutesToPlayer;
    private List<Vector3> VisitedPoints;
    public MapMaintainer MapMaintainer;

    void Update()
    {
        RoutesToPlayer = new List<List<Vector3>>();
        VisitedPoints = new List<Vector3>();
        List<Vector3> fastestPath = FindFastestPath();
    }

    public void Constructor(List<Vector3> points, GameObject player)
    {
        this.PointsGrid = points;
        this.Player = player;
        MapMaintainer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
    }

    public List<Vector3> FindFastestPath()
    {
        ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
        ClosestPointToPlayer = MapMaintainer.PointClosestToPlayer;

        Debug.Log(ClosestPointToPlayer + " " + ClosestPointToSelf);

        if (ClosestPointToPlayer == ClosestPointToSelf)
        {
            Debug.Log("Player reached");
            return null;
        }

        // now, we start at the point closest to the enemy, and then explore
        // nearby points from there

        List<Vector3> temp = new List<Vector3>();
        FindAPath(ClosestPointToSelf, temp);

        List<Vector3> fastestRoute = new List<Vector3>();
        int min = 0;
        if(RoutesToPlayer.Count > 0){
            min = RoutesToPlayer[0].Count;
        }
        foreach (List<Vector3> route in RoutesToPlayer)
        {
            if (route.Count < min)
            {
                min = route.Count;
                fastestRoute = route;
            }
        }

        return fastestRoute;
    }

    public void FindAPath(Vector3 currentPoint, List<Vector3> routeToPlayer)
    {

        Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0), };

        if (currentPoint != ClosestPointToPlayer)
        {
            VisitedPoints.Add(currentPoint);

            foreach (Vector3 direction in directions)
            {
                if (PointsGrid.Contains(currentPoint - direction) && !VisitedPoints.Contains(currentPoint - direction))
                {
                    List<Vector3> newRouteToPlayer = routeToPlayer;
                    newRouteToPlayer.Add(currentPoint - direction);

                    // foreach (Vector3 vector in newRouteToPlayer)
                    // {
                    //     Debug.Log(vector);
                    // }

                    // FindAPath(currentPoint - direction, newRouteToPlayer);
                }
            }
        }
        else if (currentPoint == ClosestPointToPlayer)
        {
            RoutesToPlayer.Add(routeToPlayer);
        }
    }
}
