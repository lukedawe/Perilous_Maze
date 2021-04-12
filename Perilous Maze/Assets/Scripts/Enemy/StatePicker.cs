using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePicker : MonoBehaviour
{
    IState CurrentState;
    [SerializeField] int speed;
    GameObject Player;
    Patrol patrol;
    Persue persue;

    public void Constructor(List<Vector3> points)
    {
        patrol = GetComponent<Patrol>();
        persue = GetComponent<Persue>();

        this.CurrentState = GetComponent<Patrol>();
        GameObject modifier = GameObject.Find("Map Modifier");
        MapMaintainer maintainer = modifier.GetComponent<MapMaintainer>();
        this.Player = maintainer.Player;

        patrol.Constructor(speed, Player, points);
        persue.Constructor(Player, speed);

        patrol.Player = Player;
        persue.Player = Player;
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
