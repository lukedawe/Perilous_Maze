using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persue : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    int speed;

    void Start()
    {
        this.speed = GetComponent<StatePicker>().speed;
    }

    public bool Activate()
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDir, forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (angle < 5.0F && distance < 9f)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
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
