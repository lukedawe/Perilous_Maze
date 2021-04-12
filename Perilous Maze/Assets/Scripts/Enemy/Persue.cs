using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persue : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    int Speed;

    public void Constructor(GameObject player, int speed){
        this.Player = player;
        this.Speed = speed;
    }

    public bool Activate()
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (angle < 18.0F && distance < 5f)
        {
            float step = Speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
            transform.LookAt(Player.transform.position);
            return true;
        }
        else
        {
            return false;
        }
    }
}
