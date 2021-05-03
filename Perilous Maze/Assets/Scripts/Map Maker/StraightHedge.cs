using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class StraightHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public Vector3[] connectorPoints { get; set; }
    public Vector3[] collisionPoints { get; set; }


    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        foreach (Vector3 point in this.collisionPoints)
        {
            if (!VectorMaths.IsPointInsideRange(position, mapSize))
            {
                return true;
            }
        }
        return false;
    }

    public void Constructor(int rotation, Vector3 position, int xRotation = 0)
    {
        this.connectorPoints = new Vector3[2];
        this.collisionPoints = new Vector3[4];


        int x = 2;
        int z = 0;

        this.offset = VectorMaths.CalculateOffset(rotation, x, z);

        this.connectorPoints[0] = position;
        this.connectorPoints[1] = position + this.offset;
        
        this.collisionPoints[0] = position - (this.offset / 2);
        this.collisionPoints[1] = position;
        this.collisionPoints[2] = position + (this.offset / 2);
        this.collisionPoints[3] = position + this.offset;

        this.GetComponent<LineRenderer>().SetPositions(connectorPoints);

    }
}
