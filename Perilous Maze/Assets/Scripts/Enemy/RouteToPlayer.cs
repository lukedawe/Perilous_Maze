using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class RouteToPlayer : MonoBehaviour
{
    Vector3 ClosestPointToSelf;
    Vector3 ClosestPointToPlayer;
    float TimeSinceLastRun;
    Vector3 target;
    Vector3[] FastestPath;
    int startIndex;
    public AStar PathFinder;
    [SerializeField] EnemyVariables Variables;
    [SerializeField] Animator animator;
    bool isWalking = false;

    void FixedUpdate()
    {
        TimeSinceLastRun += Time.deltaTime;

        if (Variables.Player.GetComponent<PlayerMovement>().walkSoundPlaying)
        {
            ClosestPointToPlayer = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().PointClosestToPlayer;
        }

        ClosestPointToSelf = VectorMaths.FindPointClosestToEntity(transform, Variables.PointsGrid);

        if (ClosestPointToSelf != ClosestPointToPlayer)
        {
            if (!isWalking)
            {
                animator.SetTrigger("Walking");
                isWalking = true;
            }

            if (TimeSinceLastRun >= 1)
            {

                // keep track of the index of the target that the enemy needs to travel towards
                TimeSinceLastRun = 0;

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
        else if (CanSeePlayer())
        {
            // if the enemy is not currently moving, then trigger the animation to walk to the player
            if (!isWalking)
            {
                animator.SetTrigger("Walking");
                isWalking = true;
            }
            // walk towards the enemy
            float step = Variables.Speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Variables.Player.transform.position, step);
            transform.LookAt(Variables.Player.transform.position);
        }
        else
        {
            if (isWalking)
            {
                animator.SetTrigger("Idle");
                isWalking = false;
            }
        }
    }

    public void Constructor(List<Vector3> points, GameObject player)
    {
        Variables.Constructor();
        Debug.Log("running constructor");
        PathFinder.maze = GameObject.Find("Map Modifier").GetComponent<NewMapCreator>().Maze;
    }

    bool CanSeePlayer()
    {
        Vector3 targetDir = Variables.Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(Variables.Player.transform.position, transform.position);
        if (angle < Variables.ViewAngle && distance < Variables.ViewDistance)
        {
            Vector3 directionToPlayer = (Variables.Player.transform.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, directionToPlayer, distance, Variables.HedgeMask))
            {
                return true;
            }
        }
        return false;
    }
}