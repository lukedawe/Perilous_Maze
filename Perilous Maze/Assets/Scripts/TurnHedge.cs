using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class TurnHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public GameObject self;
    public int collisions { get; set; }
    public Vector3[] points { get; set; }

    public bool WillCollide(Vector3 currentPos)
    {
        return false;
    }
    public bool WillGoOffMap(Vector3 position, GameObject plane, int mapSize)
    {
        return !VectorMaths.IsPointInsideRange(position, mapSize);
    }

    public void Constructor(int rotation, Vector3 initialPosition, int xRotation = 0)
    {
        this.points = new Vector3[3];

        int x = 4;
        int z;

        if (xRotation == 180)
        {
            z = 5;
        }
        else
        {
            z = -5;
        }


        this.offset = VectorMaths.CalculateOffset(rotation, x, z);

        // This is for the line that appears
        Vector3 intermediatePoint = initialPosition + VectorMaths.CalculateOffset(rotation, 4, 0);

        this.points[0] = initialPosition;
        this.points[1] = intermediatePoint; 
        this.points[2] = initialPosition + this.offset;

        this.GetComponent<LineRenderer>().SetPositions(points);
    }
}
