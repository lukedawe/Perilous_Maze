using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class StraightHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public Vector3[] points { get; set; }

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        return !VectorMaths.IsPointInsideRange(position, mapSize);
    }

    public void Constructor(int rotation, Vector3 position, int xRotation = 0)
    {
        this.points = new Vector3[2];

        int x = 2;
        int z = 0;

        this.offset = VectorMaths.CalculateOffset(rotation, x, z);

        this.points[0] = position;
        this.points[1] = (position + this.offset);

        this.GetComponent<LineRenderer>().SetPositions(points);
    }
}
