using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class StraightHedge : MonoBehaviour, IHedge
{
    public Vector3 offset { get; set; }
    public GameObject self;
    public int collisions{ get; set; }

    public bool WillCollide(Vector3 currentPos)
    {
        return false;
    }
    public bool WillGoOffMap(Vector3 position, GameObject plane)
    {
        return false;
    }

    public void Constructor(int rotation, int xRotation = 0){
        int x = 2;
        int z = 0;

        this.offset = Converter.CalculateOffset(rotation,x,z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger called");
    }
}
