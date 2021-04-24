using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class Persue : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    EnemyVariables Variables;
    float timeSinceLastSeen;
    Vector3 positionLastSeenIn;
    float timeSinceLastRun;
    Vector3[] FastestPath;
    AStar PathFinder;
    int startIndex;
    Vector3 target;


    public void Constructor()
    {
        Variables = GetComponent<EnemyVariables>();
        this.Player = Variables.Player;
        PathFinder = GetComponent<AStar>();
    }

    // public bool Activate(float deltaTime)
    // {
    //     float distance = Vector3.Distance(Player.transform.position, transform.position);
    //     if (distance < 30)
    //     {
    //         float step = Variables.Speed * Time.deltaTime; // calculate distance to move
    //         transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
    //         transform.LookAt(Player.transform.position);
    //         return true;
    //     }
    //     return false;
    // }

    public bool Activate(float deltaTime)
    {
        timeSinceLastSeen += deltaTime;
        Vector3 targetDir = Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);

        if (angle < Variables.ViewAngle && distance < Variables.ViewDistance)
        {
            Vector3 directionToPlayer = (Variables.Player.transform.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, directionToPlayer, distance, Variables.HedgeMask))
            {
                timeSinceLastSeen = 0;
                positionLastSeenIn = Player.transform.position;
                // float step = Variables.Speed * Time.deltaTime; // calculate distance to move
                // transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
                // transform.LookAt(Player.transform.position);

                // return true;
            }
        }
        if (timeSinceLastSeen < 2)
        {
            Variables.Speed = 1;
            timeSinceLastRun += Time.deltaTime;
            Vector3 ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, Variables.PointsGrid);
            Vector3 ClosestPointToPlayer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().PointClosestToPlayer;

            if (ClosestPointToSelf != ClosestPointToPlayer)
            {
                if (timeSinceLastRun >= 1)
                {
                    // keep track of the index of the target that the enemy needs to travel towards
                    timeSinceLastRun = 0;

                    FastestPath = PathFinder.AStarSearch(ClosestPointToSelf, ClosestPointToPlayer);

                    if (FastestPath != null)
                    {
                        startIndex = FastestPath.Length - 1;
                        target = FastestPath[startIndex];
                    }
                }

                if (FastestPath != null && target != null && FastestPath.Length > 0)
                {
                    // target.y = -0.5f;
                    // Move our position a step closer to the target.
                    float step = Variables.Speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, target, step);

                    Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Variables.TurnSpeed * Time.deltaTime);

                    // Check if the position of the cube and sphere are approximately equal.
                    if (Vector3.Distance(transform.position, target) < 0.5f)
                    {
                        startIndex--;
                        if (startIndex >= 0)
                        {
                            target = FastestPath[startIndex];
                        }
                    }
                }
            }
            else
            {
                float step = Variables.Speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
                transform.LookAt(Player.transform.position);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
