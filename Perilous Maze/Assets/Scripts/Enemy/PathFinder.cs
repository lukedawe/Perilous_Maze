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
    private float TimeSinceLastRun;
    [SerializeField] private int speed;
    Vector3 target;
    List<Vector3> FastestPath;
    int TargetIndex;

    void Update()
    {
        TimeSinceLastRun += Time.deltaTime;

        if (TimeSinceLastRun >= 1)
        {
            // keep track of the index of the target that the enemy needs to travel towards
            TargetIndex = 0;
            ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
            ClosestPointToPlayer = MapMaintainer.PointClosestToPlayer;
            TimeSinceLastRun = 0;
            RoutesToPlayer = new List<List<Vector3>>();
            VisitedPoints = new List<Vector3>();
            FastestPath = FindFastestPath();
            target = FastestPath[TargetIndex];
        }

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            // Swap the position of the cylinder.
            target *= -1.0f;
        }

        // if the enemy has reached the target, change the target to the next one
        if (transform.position == target)
        {
            TargetIndex++;
            if (FastestPath.Count > TargetIndex)
            {
                target = FastestPath[TargetIndex];
            }
        }
    }

    public void Constructor(List<Vector3> points, GameObject player)
    {
        this.PointsGrid = points;
        this.Player = player;
        MapMaintainer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
    }

    public List<Vector3> FindFastestPath()
    {

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

        int min = 100;
        if (RoutesToPlayer.Count > 0) min = RoutesToPlayer[0].Count;

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

        Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };

        if (currentPoint != ClosestPointToPlayer)
        {
            VisitedPoints.Add(currentPoint);

            foreach (Vector3 direction in directions)
            {
                if (PointsGrid.Contains(currentPoint - direction) && !VisitedPoints.Contains(currentPoint - direction))
                {
                    List<Vector3> newRouteToPlayer = routeToPlayer;
                    newRouteToPlayer.Add(currentPoint - direction);
                    FindAPath(currentPoint - direction, newRouteToPlayer);
                }
            }
        }
        else if (currentPoint == ClosestPointToPlayer)
        {
            if (routeToPlayer.Count > 0)
            {
                RoutesToPlayer.Add(routeToPlayer);
                Debug.Log("Successfully adding route to list of routes" + RoutesToPlayer.Count);
                Debug.Log("===========================================");
                foreach (List<Vector3> list in RoutesToPlayer)
                {
                    Debug.Log("------------------------------------------");
                    foreach (Vector3 v in list)
                    {
                        Debug.Log(v);
                    }
                    Debug.Log("------------------------------------------");
                }
                Debug.Log("===========================================");
            }
        }
    }
}
