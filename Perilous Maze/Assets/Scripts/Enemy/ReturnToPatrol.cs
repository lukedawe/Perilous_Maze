using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class ReturnToPatrol : MonoBehaviour, IState
{
    // the end point for the enemy's journey
    Vector3 EndPoint;
    // where the enemy is currently
    Vector3 StartPoint;
    // the point which the enemy is currently heading towards
    Vector3 CurrentTarget;
    int CurrentTargetIndex;
    public GameObject Player { get; set; }
    List<Vector3> route;
    float speed;

    public bool CalculateRoute(float speed)
    {
        StartPoint = VectorMaths.FindPointClosestToEntity(transform, GetComponent<PathFinder>().PointsGrid);
        EndPoint = GetComponent<Patrol>().point1;
        route = GetComponent<PathFinder>().FindFastestPath(StartPoint, EndPoint);
        CurrentTargetIndex = 0;
        if (route != null && route.Count != 0)
        {
            CurrentTarget = route[CurrentTargetIndex];
        }
        else
        {
            return false;
        }
        this.speed = speed;
        return true;
    }

    public bool Activate()
    {
        if (route == null || route.Count == 0) CalculateRoute(speed);

        if (Vector3.Distance(transform.position, EndPoint) < 0.5f)
        {
            return false;
        }
        if (Vector3.Distance(transform.position, CurrentTarget) < 0.5f)
        {
            CurrentTargetIndex++;
            CurrentTarget = route[CurrentTargetIndex];
        }
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, route[CurrentTargetIndex], step);
        transform.LookAt(route[CurrentTargetIndex]);
        return true;
    }
}
