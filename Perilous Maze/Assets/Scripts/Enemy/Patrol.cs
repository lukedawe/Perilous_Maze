using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    public Vector3 point1 = new Vector3();
    public Vector3 point2 = new Vector3();
    private Vector3 CurrentTarget;
    private int speed;
    List<Vector3> route;
    int currentPoint;

    void Start()
    {
        this.speed = GetComponent<StatePicker>().speed;
        this.Player = GetComponent<StatePicker>().Player;
        currentPoint = 0;
    }

    public void Constructor(Vector3 start, Vector3 end)
    {
        this.point1 = start;
        this.point2 = end;

        route = GetComponent<PathFinder>().FindFastestPath(point1, point2);
    }

    // Update is called once per frame
    public bool Activate()
    {
        Vector3 targetDir = Player.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDir, forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (angle < 5.0F && distance < 9f)
        {
            return false;
        }
        else
        {
            if (transform.position == CurrentTarget)
            {
                if (CurrentTarget == point1) CurrentTarget = point2; else CurrentTarget = point1;
            }

            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, CurrentTarget, step);
            transform.LookAt(CurrentTarget);

            return true;
        }
    }
}
