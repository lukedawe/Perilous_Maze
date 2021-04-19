using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persue : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    float Speed;
    EnemyVariables Variables;

    public void Constructor()
    {
        Variables = GetComponent<EnemyVariables>();
        this.Player = Variables.Player;
        this.Speed = Variables.Speed;
    }

    public bool Activate(float deltaTime)
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (angle < Variables.ViewAngle && distance < Variables.ViewDistance)
        {
            Vector3 directionToPlayer = (Variables.Player.transform.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, directionToPlayer, distance, Variables.HedgeMask))
            {
                float step = Speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
                transform.LookAt(Player.transform.position);

                return true;
            }
        }

        return false;
    }
}
