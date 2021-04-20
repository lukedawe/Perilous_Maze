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
    Vector3[] route;
    float speed;
    EnemyVariables Variables;

    public bool CalculateRoute()
    {
        Variables = GetComponent<EnemyVariables>();
        StartPoint = VectorMaths.FindPointClosestToEntity(transform, Variables.PointsGrid);
        EndPoint = GetComponent<Patrol>().IntermediateTarget;
        route = GetComponent<AStar>().AStarSearch(StartPoint, EndPoint);
        CurrentTargetIndex = route.Length - 1;
        if (route != null && route.Length != 0)
        {
            CurrentTarget = route[CurrentTargetIndex];
        }
        else
        {
            return false;
        }
        this.speed = Variables.Speed;
        return true;
    }

    public bool Activate(float deltaTime)
    {
        if (route == null || route.Length == 0) CalculateRoute();

        if (Vector3.Distance(transform.position, EndPoint) < 0.5f)
        {
            return false;
        }
        if (Vector3.Distance(transform.position, CurrentTarget) < 0.5f)
        {
            CurrentTargetIndex--;
            CurrentTarget = route[CurrentTargetIndex];
        }
        float step = speed * deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, route[CurrentTargetIndex], step);

        Quaternion targetRotation = Quaternion.LookRotation(CurrentTarget - transform.position);
        // Smoothly rotate towards the target point.
        float turnSpeed = Variables.TurnSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * deltaTime);

        return true;
    }
}
