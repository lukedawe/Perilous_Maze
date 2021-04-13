using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class RouteToPlayer : MonoBehaviour
{
    public List<Vector3> PointsGrid;
    Vector3 ClosestPointToSelf;
    Vector3 ClosestPointToPlayer;
    GameObject Player;
    public MapMaintainer MapMaintainer;
    float TimeSinceLastRun;
    [SerializeField] float speed;
    Vector3 target;
    List<Vector3> FastestPath = new List<Vector3>();
    int TargetIndex;
    public PathFinder PathFinder;

    void FixedUpdate()
    {
        TimeSinceLastRun += Time.deltaTime;
        ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
        ClosestPointToPlayer = MapMaintainer.PointClosestToPlayer;

        // if the player is too far away, don't calculate the route to take
        if (Vector3.Distance(ClosestPointToPlayer, ClosestPointToSelf) > 30)
        {
            return;
        }

        if (ClosestPointToSelf != ClosestPointToPlayer)
        {
            if (TimeSinceLastRun >= 1)
            {
                // keep track of the index of the target that the enemy needs to travel towards
                TargetIndex = 0;
                TimeSinceLastRun = 0;
                FastestPath.Clear();

                var watch = System.Diagnostics.Stopwatch.StartNew();
                FastestPath = GetComponent<PathFinder>().FindFastestPath(ClosestPointToSelf, ClosestPointToPlayer);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Debug.Log("Time taken to find path: " + elapsedMs.ToString());

                // GetComponent<LineRenderer>().SetPositions(FastestPath.ToArray());
                if (FastestPath.Count > 0) target = FastestPath[TargetIndex];
            }

            if (target != null && FastestPath.Count > 0)
            {
                target.y = -0.5f;
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);
                transform.LookAt(target);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target) < 0.5f)
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
        this.PathFinder = GetComponent<PathFinder>();
        this.PointsGrid = points;
        PathFinder.PointsGrid = points;
        this.Player = player;
        MapMaintainer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
    }
}