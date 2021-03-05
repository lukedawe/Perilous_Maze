using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class TurnHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public GameObject self;
    public int collisions{ get; set; }

    public bool WillCollide(Vector3 currentPos)
    {
        return false;
    }
    public bool WillGoOffMap(Vector3 position, GameObject plane, int mapSize)
    {
        Debug.Log("position = " + position + "inside range? = " + VectorMaths.IsPointInsideRange(position, mapSize));
        return !VectorMaths.IsPointInsideRange(position, mapSize);
    }

    public void Constructor(int rotation, int xRotation=0)
    {
        int x = 3;
        int z;
        if(xRotation == 180){
            z = 5;
        }
        else{
            z = -5;
        }

        this.offset = VectorMaths.CalculateOffset(rotation,x,z);
    }
}
