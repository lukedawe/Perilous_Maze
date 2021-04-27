using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePicker : MonoBehaviour
{
    [SerializeField] string stateText;
    IState CurrentState;
    Patrol patrol;
    Persue persue;
    EnemyVariables Variables;
    [SerializeField] Animator animator;

    void WalkTowardsDistraction(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < 30 && (Object)CurrentState != GetComponent<Persue>())
        {
            GetComponent<WalkToDistraction>().Constructor(destination);
            this.CurrentState = GetComponent<WalkToDistraction>();
            animator.SetTrigger("Distracted");
        }
    }

    void OnDisable()
    {
        Distractable.OnDistraction -= WalkTowardsDistraction;
    }

    public void Constructor(List<Vector3> points)
    {
        Distractable.OnDistraction += WalkTowardsDistraction;

        patrol = GetComponent<Patrol>();
        persue = GetComponent<Persue>();
        this.CurrentState = patrol;

        Variables = GetComponent<EnemyVariables>();

        patrol.Player = Variables.Player;
        persue.Player = Variables.Player;

        Variables.Constructor();
        patrol.Constructor();
        persue.Constructor();

    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
        {
            this.CurrentState = persue;
            stateText = "Persue";
        }

        if (!this.CurrentState.Activate(Time.deltaTime))
        {
            // if the enemy was persuing but lost the player, return it to the patrolling state
            if ((Object)this.CurrentState == GetComponent<Persue>() || (Object)this.CurrentState == GetComponent<WalkToDistraction>())
            {
                // something with this causes an error
                bool success = GetComponent<ReturnToPatrol>().CalculateRoute();
                animator.SetTrigger("ReturnToPatrol");
                stateText = "Return to patrol";

                // if there is success in calculating the route...
                if (success)
                {
                    this.CurrentState = GetComponent<ReturnToPatrol>();
                    stateText = "Return to patrol";
                }
                // this will cause the enemy to run into walls 
                else
                {
                    Debug.LogError("Could not return to patrol");
                    this.CurrentState = GetComponent<Patrol>();
                    stateText = "Patrol";
                }
            }
            else if ((Object)this.CurrentState == GetComponent<ReturnToPatrol>())
            {
                animator.SetTrigger("ReturnToPatrol");
                this.CurrentState = GetComponent<Patrol>();
                stateText = "Patrol";
            }
        }
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
                Debug.Log("Chasing the player");
                return true;
            }
        }
        return false;
    }
}
