using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;


public class Patrol : MonoBehaviour, IState
{
    public GameObject Player { get; set; }
    public Vector3 point1;
    Vector3 point2;
    Vector3 CurrentTarget;
    int speed;
    List<Vector3> route;
    int currentPoint;
    List<Vector3> ReturnRoute;

    public void Constructor(int speed, GameObject player, List<Vector3> PointsGrid)
    {
        this.point1 = VectorMaths.FindPointClosestToEntity(transform, PointsGrid);
        this.speed = speed;
        this.Player = player;
        GetComponent<PathFinder>().PointsGrid = PointsGrid;

        do
        {
            this.point2 = PointsGrid[Random.Range(0, PointsGrid.Count)];
            route = GetComponent<PathFinder>().FindFastestPath(point1, point2);
        }
        while (route == null || route.Count == 0);

        // GetComponent<LineRenderer>().SetPositions(route.ToArray());

        if (route.Count == 0)
        {
            Debug.Log("Point1: " + point1.ToString());
            Debug.Log("Point2: " + point2.ToString());
            Debug.Log("Points grid size: " + PointsGrid.Count);
            Debug.Log("Failed to find route");
        }
        currentPoint = 0;
        this.CurrentTarget = route[currentPoint];

    }

    // Update is called once per frame
    public bool Activate()
    {
        if (route == null) return true;
        if (route.Count == 0) return true;

        Vector3 targetDir = Player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (angle < 18f && distance < 5f)
        {
            return false;
        }
        else
        {
            if (Vector3.Distance(transform.position, route[route.Count - 1]) < 0.5f || Vector3.Distance(transform.position, route[0]) < 0.5f)
            {
                if (CurrentTarget == point1) CurrentTarget = point2;
                else CurrentTarget = point1;
            }
            if (Vector3.Distance(transform.position, route[currentPoint]) < 0.5f)
            {
                if (CurrentTarget == point1) currentPoint++;
                else currentPoint--;
            }
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, route[currentPoint], step);
            transform.LookAt(route[currentPoint]);

        }
        return true;
    }
}
