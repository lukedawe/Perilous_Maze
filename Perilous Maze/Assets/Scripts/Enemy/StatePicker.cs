using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePicker : MonoBehaviour
{
    IState CurrentState;
    public int speed;
    public GameObject Player;

    void Awake()
    {
        this.CurrentState = GetComponent<Patrol>();
        GameObject modifier = GameObject.Find("Map Modifier");
        MapMaintainer maintainer = modifier.GetComponent<MapMaintainer>();
        this.Player = maintainer.Player;

        Vector3 point1 = maintainer.PointsGrid[Random.Range(0, maintainer.PointsGrid.Count)];
        Vector3 point2 = maintainer.PointsGrid[Random.Range(0, maintainer.PointsGrid.Count)];

        GetComponent<Patrol>().Constructor(point1, point2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.CurrentState.Activate())
        {
            if ((Object)this.CurrentState == GetComponent<Patrol>())
            {
                this.CurrentState = GetComponent<Persue>();
            }
            else
            {
                this.CurrentState = GetComponent<Patrol>();
            }
        }
    }
}
