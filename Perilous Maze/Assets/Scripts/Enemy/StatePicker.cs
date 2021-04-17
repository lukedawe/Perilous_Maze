using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePicker : MonoBehaviour
{
    IState CurrentState;
    Patrol patrol;
    Persue persue;

    public void Constructor(List<Vector3> points)
    {
        patrol = GetComponent<Patrol>();
        persue = GetComponent<Persue>();
        this.CurrentState = patrol;

        EnemyVariables variables = GetComponent<EnemyVariables>();

        patrol.Player = variables.Player;
        persue.Player = variables.Player;

        variables.Constructor();
        patrol.Constructor();
        persue.Constructor();

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.CurrentState.Activate(Time.deltaTime))
        {
            if ((Object)this.CurrentState == GetComponent<Patrol>())
            {
                this.CurrentState = GetComponent<Persue>();
            }
            // if the enemy was persuing but lost the player, return it to the patrolling state
            else if ((Object)this.CurrentState == GetComponent<Persue>())
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
}
