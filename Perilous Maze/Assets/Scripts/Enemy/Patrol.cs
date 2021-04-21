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
            this.point2 = PointsGrid[Random.Range(0, PointsGrid.Count)];
            route = GetComponent<AStar>().AStarSearch(point1, point2);
        }
        while (route == null);

        // GetComponent<LineRenderer>().SetPositions(route.ToArray());

        if (route.Length == 0)
        {
            Debug.Log("Point1: " + point1.ToString());
            Debug.Log("Point2: " + point2.ToString());
            Debug.Log("Points grid size: " + PointsGrid.Count);
            Debug.Log("Failed to find route");
        }
        currentPoint = route.Length - 1;
        this.CurrentTarget = route[currentPoint];

    }

    // Update is called once per frame
    public bool Activate(float deltaTime)
    {
        if (route == null) return false;
        if (route.Length == 0) return false;

        if (Vector3.Distance(transform.position, route[route.Length - 1]) < 0.5f || Vector3.Distance(transform.position, route[0]) < 0.5f)
        {
            if (CurrentTarget == point1) CurrentTarget = point2;
            else CurrentTarget = point1;
        }
        if (Vector3.Distance(transform.position, route[currentPoint]) < 0.5f)
        {
            if (CurrentTarget == point1) currentPoint++;
            else currentPoint--;

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
