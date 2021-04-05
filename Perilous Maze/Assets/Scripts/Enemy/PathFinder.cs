using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<Vector3> PointsGrid;
    private Vector3 ClosestPointToSelf;
    private Vector3 ClosestPointToPlayer;
    private GameObject Player;
    public List<List<Vector3>> RoutesToPlayer;
    private List<Vector3> VisitedPoints;

    void FixedUpdate()
    {
        RoutesToPlayer = new List<List<Vector3>>();
        VisitedPoints = new List<Vector3>();
        List<Vector3> fastestPath = FindFastestPath();
    }

    public void Constructor(List<Vector3> points, GameObject player)
    {
        this.PointsGrid = points;
        this.Player = player;
    }

    private Vector3 FindPointClosestToEntity(Transform t)
    {
        Vector3 closestPoint = new Vector3();
        float min = (this.PointsGrid[0] - t.position).magnitude;
        foreach (Vector3 point in this.PointsGrid)
        {
            float distance = (point - t.position).magnitude;
            if (distance < min)
            {
                closestPoint = point;
                min = distance;
            }
        }

        return closestPoint;
    }

    public List<Vector3> FindFastestPath()
    {
        ClosestPointToSelf = FindPointClosestToEntity(transform);
        ClosestPointToPlayer = FindPointClosestToEntity(Player.transform);

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
        int min = RoutesToPlayer[0].Count;
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
        Vector3 direction1 = new Vector3(0, 0, 1);
        Vector3 direction2 = new Vector3(0, 0, -1);
        Vector3 direction3 = new Vector3(1, 0, 0);
        Vector3 direction4 = new Vector3(-1, 0, 0);

        Debug.Log(VisitedPoints);

        if (currentPoint != ClosestPointToPlayer && !VisitedPoints.Contains(currentPoint))
        {
            VisitedPoints.Add(currentPoint);

            if (PointsGrid.Contains(currentPoint - direction1))
            {
                List<Vector3> newRouteToPlayer = routeToPlayer;
                newRouteToPlayer.Add(currentPoint - direction1);
                FindAPath(currentPoint - direction1, newRouteToPlayer);
            }
            if (PointsGrid.Contains(currentPoint - direction2))
            {
                List<Vector3> newRouteToPlayer = routeToPlayer;
                newRouteToPlayer.Add(currentPoint - direction2);
                FindAPath(currentPoint - direction2, newRouteToPlayer);
            }
            if (PointsGrid.Contains(currentPoint - direction3))
            {
                List<Vector3> newRouteToPlayer = routeToPlayer;
                newRouteToPlayer.Add(currentPoint - direction3);
                FindAPath(currentPoint - direction3, newRouteToPlayer);
            }
            if (PointsGrid.Contains(currentPoint - direction4))
            {
                List<Vector3> newRouteToPlayer = routeToPlayer;
                newRouteToPlayer.Add(currentPoint - direction4);
                FindAPath(currentPoint - direction4, newRouteToPlayer);

            }
        }
        else if (currentPoint == ClosestPointToPlayer)
        {
            RoutesToPlayer.Add(routeToPlayer);
        }
    }
}
