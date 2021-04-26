using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class Distractable : MonoBehaviour
{
    public delegate void Distracted(Vector3 position);
    public static event Distracted OnDistraction;

    void OnCollisionEnter(Collision collision)
    {

        // check that there are methods that are subscribed to the event
        // also check that the collision wasn't with the player (this can happen when the animation plays)
        if (OnDistraction != null && collision.collider.tag != "Player")
        {
            MapMaintainer m = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
            // find the point on the map that the rock fell near to (so that the enemies can find their way to it)
            Vector3 position = VectorMaths.FindPointClosestToEntity(transform, m.PointsGrid);
            OnDistraction(position);
        }
    }
}
