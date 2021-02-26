using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StraightHedge : MonoBehaviour, IHedge
{
    public Vector3[] nextPoints { get; set; }
    public Vector3 offset { get; set; }
    public GameObject self;

    public bool WillCollide(Vector3 currentPos)
    {
        return false;
    }
    public bool WillGoOffMap(Vector3 position, GameObject plane)
    {
        return false;
    }

    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.GetComponent<StraightHedge>() != null){
            Destroy(self);
        }
    }

    public void Constructor(int rotation){
        switch (rotation)
        {
            case 0:
                this.offset = new Vector3(2, 0, 0);
                break;
            case 90:
                this.offset = new Vector3(0, 0, -2);
                break;
            case 180:
                break;
            case 270:
                break;
            case -180:
                this.offset = new Vector3(-2, 0, 0);
                break;
        }
    }
}
