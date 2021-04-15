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
    Vector3[] FastestPath;
    int startIndex;
    public AStar PathFinder;
    [SerializeField] float TurnSpeed;

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
                TimeSinceLastRun = 0;

                var watch = System.Diagnostics.Stopwatch.StartNew();
                FastestPath = PathFinder.AStarSearch(ClosestPointToSelf, ClosestPointToPlayer);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Debug.Log("Time taken to find path: " + elapsedMs.ToString());

                if (FastestPath != null)
                {
                    startIndex = FastestPath.Length - 1;
                    target = FastestPath[startIndex];
                }
            }

            if (FastestPath != null && target != null && FastestPath.Length > 0)
            {
                target.y = -0.5f;
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);

                Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target) < 0.5f)
                {
                    startIndex--;
                    if (startIndex > 0)
                    {
                        target = FastestPath[startIndex];
                    }
                }
            }
        }
    }

    public void Constructor(List<Vector3> points, GameObject player)
    {
        Debug.Log("running constructor");
        this.PathFinder = GetComponent<AStar>();
        this.PointsGrid = points;
        PathFinder.maze = GameObject.Find("Map Modifier").GetComponent<NewMapCreator>().Maze;
        this.Player = player;
        MapMaintainer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
    }
}