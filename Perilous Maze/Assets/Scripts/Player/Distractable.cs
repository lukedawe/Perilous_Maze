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
        if (OnDistraction != null)
        {
            MapMaintainer m = GameObject.Find("Map Modifier").GetComponent<MapMaintainer>();
            Vector3 position = VectorMaths.FindPointClosestToEntity(transform, m.PointsGrid);
            OnDistraction(position);
        }
    }
}
