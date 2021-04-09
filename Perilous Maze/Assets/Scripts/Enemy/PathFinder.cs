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
    public List<List<Vector3>> RoutesToPlayer = new List<List<Vector3>>();
    public MapMaintainer MapMaintainer;
    private float TimeSinceLastRun;
    [SerializeField] private float speed;
    Vector3 target;
    List<Vector3> FastestPath = new List<Vector3>();
    int TargetIndex;

    void FixedUpdate()
    {
        TimeSinceLastRun += Time.deltaTime;
        ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
        ClosestPointToPlayer = MapMaintainer.PointClosestToPlayer;

        if (ClosestPointToSelf != ClosestPointToPlayer)
        {
            if (TimeSinceLastRun >= 1)
            {
                // keep track of the index of the target that the enemy needs to travel towards
                TargetIndex = 0;
                TimeSinceLastRun = 0;
                RoutesToPlayer.Clear();
                FastestPath.Clear();
                FastestPath = FindFastestPath();
                // GetComponent<LineRenderer>().SetPositions(FastestPath.ToArray());
                if (FastestPath.Count > 0) target = FastestPath[TargetIndex];
            }

            if (target != null && FastestPath.Count > 0)
            {
                target.y = 0;
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);
                transform.LookAt(target);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target) < 0.001f)
                {
                    TargetIndex++;
                    if (FastestPath.Count > TargetIndex)
                    {
                        target = FastestPath[TargetIndex];
                    }
                }
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

        if (RoutesToPlayer.Count > 0)
        {
            fastestRoute.AddRange(RoutesToPlayer[0]);
            min = RoutesToPlayer[0].Count;
            foreach (List<Vector3> route in RoutesToPlayer)
            {
                if (route.Count < min)
                {
                    Debug.Log("New min: " + min);
                    min = route.Count;
                    fastestRoute.Clear();
                    fastestRoute.AddRange(route);
                }
            }
        }

        Debug.Log("RoutesToPlayer: " + RoutesToPlayer.Count + " fastest route: " + fastestRoute.Count);

        return fastestRoute;
    }

    public void FindAPath(Vector3 currentPoint, List<Vector3> routeToPlayer)
    {
        if (routeToPlayer.Count > 30)
        {
            return;
        }
        Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };

        if (currentPoint != ClosestPointToPlayer)
        {
            foreach (Vector3 direction in directions)
            {
                Vector3 newPoint = currentPoint + direction;
                if (PointsGrid.Contains(newPoint) && !routeToPlayer.Contains(newPoint))
                {
                    List<Vector3> newRoute = new List<Vector3>();
                    newRoute.AddRange(routeToPlayer);
                    newRoute.Add(newPoint);
                    // this line causes a stack overflow?
                    // routeToPlayer.Clear();
                    FindAPath(newPoint, newRoute);
                }
            }
        }
        else
        {
            if (routeToPlayer.Count > 0)
            {
                List<Vector3> temp = new List<Vector3>();
                temp.AddRange(routeToPlayer);
                RoutesToPlayer.Add(temp);
                Debug.Log("Successfully adding route to list of routes" + RoutesToPlayer.Count);
            }
        }
    }
}
