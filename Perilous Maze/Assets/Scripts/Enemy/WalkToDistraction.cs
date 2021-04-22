using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class WalkToDistraction : MonoBehaviour, IState
{
    Vector3 Destination;
    AStar PathFinder;
    EnemyVariables Variables;
    Vector3[] FastestPath;
    int StartIndex;
    Vector3 target;
    public GameObject Player { get; set; }

    public void Constructor(Vector3 destination)
    {
        Variables = GetComponent<EnemyVariables>();
        this.Destination = destination;
        Vector3 ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, Variables.PointsGrid);

        PathFinder = GetComponent<AStar>();
        Player = Variables.Player;
        FastestPath = PathFinder.AStarSearch(ClosestPointToSelf, Destination);

        if (FastestPath != null)
        {
            StartIndex = FastestPath.Length - 1;
            target = FastestPath[StartIndex];
        }
    }
    public bool Activate(float deltaTime)
    {
        if (StartIndex > 0)
        {
            if (FastestPath != null && target != null && FastestPath.Length > 0)
            {
                // Move our position a step closer to the target.
                float step = Variables.Speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);

                Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Variables.TurnSpeed * Time.deltaTime);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target) < 0.5f)
                {
                    StartIndex--;
                    if (StartIndex > 0)
                    {
                        target = FastestPath[StartIndex];
                    }
                }
            }
            return true;
        }
        return false;

    }
}
