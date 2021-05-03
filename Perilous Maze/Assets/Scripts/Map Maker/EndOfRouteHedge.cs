using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class EndOfRouteHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public Vector3[] connectorPoints { get; set; }
    public Vector3[] collisionPoints { get; set; }
    public Vector3 transformCorrection { get; set; }

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
        this.collisionPoints = new Vector3[1];
        this.collisionPoints[0] = position;
        this.transformCorrection = VectorMaths.CalculateOffset(rotation, 0, (float)-1.5);
    }
}
