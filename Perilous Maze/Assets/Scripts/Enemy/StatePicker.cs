using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePicker : MonoBehaviour
{
    IState CurrentState;
    Patrol patrol;
    Persue persue;
    EnemyVariables Variables;

    void WalkTowardsDistraction(Vector3 destination)
    {
        Debug.Log("Rock thrown");
        if (Vector3.Distance(transform.position, destination) < 30)
        {
            GetComponent<WalkToDistraction>().Constructor(destination);
            this.CurrentState = GetComponent<WalkToDistraction>();
        }
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
            this.CurrentState = persue;

        if (!this.CurrentState.Activate(Time.deltaTime))
        {
            if ((Object)this.CurrentState == GetComponent<Patrol>())
            {
                this.CurrentState = GetComponent<Persue>();
            }
            // if the enemy was persuing but lost the player, return it to the patrolling state
            else if ((Object)this.CurrentState == GetComponent<Persue>() || (Object)this.CurrentState == GetComponent<WalkToDistraction>())
            {
                bool success = GetComponent<ReturnToPatrol>().CalculateRoute();

                if (success) this.CurrentState = GetComponent<ReturnToPatrol>();
                else this.CurrentState = GetComponent<Patrol>();
            }
            else if ((Object)this.CurrentState == GetComponent<ReturnToPatrol>())
            {
                this.CurrentState = GetComponent<Patrol>();
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
            Debug.Log("Is within angle");
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
