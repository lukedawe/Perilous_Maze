using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class StraightHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public GameObject self;
    public int collisions{ get; set; }

    public bool WillCollide(Vector3 currentPos)
    {
        return false;
    }

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, GameObject plane, int mapSize)
    {
        return !VectorMaths.IsPointInsideRange(position, mapSize);
    }

    public void Constructor(int rotation, int xRotation = 0){
        int x = 2;
        int z = 0;

        this.offset = VectorMaths.CalculateOffset(rotation,x,z);
    }
}
