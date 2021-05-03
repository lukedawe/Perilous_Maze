using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;


public class Patrol : MonoBehaviour, IState
{
    [HideInInspector] public GameObject Player { get; set; }
    Vector3 point1;
    Vector3 point2;
    Vector3 CurrentTarget;
    [HideInInspector] public Vector3 IntermediateTarget;
    float Speed;
    Vector3[] route;
    int currentPoint;
    List<Vector3> ReturnRoute;
    EnemyVariables Variables;

    public void Constructor()
    {
        Variables = GetComponent<EnemyVariables>();
        List<Vector3> PointsGrid = Variables.PointsGrid;
        this.point1 = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
        this.Player = Variables.Player;
        this.Speed = Variables.Speed;
        GetComponent<AStar>().maze = Variables.Maze;

        do
        {
            // the last point of the patrol is a random point on the map
            this.point2 = PointsGrid[Random.Range(0, PointsGrid.Count)];
            route = GetComponent<AStar>().AStarSearch(point1, point2);
        }
        while (route == null || point1 == point2 || route.Length < 4);

        if (route.Length == 0)
        {
            Debug.Log("Point1: " + point1.ToString() + "\n" + "Point2: " + point2.ToString()
            + "\n" + "Points grid size: " + PointsGrid.Count
            + "\n" + "Failed to find route");
        }
        currentPoint = route.Length - 1;
        this.CurrentTarget = route[currentPoint];
    }

    // Update is called once per frame
    public bool Activate(float deltaTime)
    {
        if (route == null) return false;
        if (route.Length == 0) return false;

        // if we are within 0.5 of our end-point, then set the current target to be the other point
        if (Vector3.Distance(transform.position, route[route.Length - 1]) < 0.5f || Vector3.Distance(transform.position, route[0]) < 0.5f)
        {
            if (CurrentTarget == point1) CurrentTarget = point2;
            else CurrentTarget = point1;
        }

        // if the distance between our position and the current point is less than 0.5, go to the next point
        if (Vector3.Distance(transform.position, route[currentPoint]) < 0.5f)
        {
            if (CurrentTarget == point1)
            {
                if (currentPoint < route.Length - 1)
                {
                    currentPoint++;
                }
            }
            else
            {
                if (currentPoint > 0)
                {
                    currentPoint--;
                }
            }

            IntermediateTarget = route[currentPoint];
        }

        float step = Speed * deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, route[currentPoint], step);

        Quaternion targetRotation = Quaternion.LookRotation(route[currentPoint] - transform.position);
        // Smoothly rotate towards the target point.
        float turnSpeed = Variables.TurnSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        return true;
    }
}
