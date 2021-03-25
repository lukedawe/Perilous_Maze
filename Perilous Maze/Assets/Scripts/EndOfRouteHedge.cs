using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class EndOfRouteHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public Vector3[] connectorPoints { get; set; }
    public Vector3[] collisionPoints { get; set; }

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        foreach(Vector3 point in this.collisionPoints){
            if(!VectorMaths.IsPointInsideRange(position, mapSize)){
                return true;
            }
        }
        return false;
    }

    public void Constructor(int rotation, Vector3 position, int xRotation = 0)
    {
        this.collisionPoints[0] = position;
    }
}
