using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHedge : MonoBehaviour, IHedge
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<StraightHedge>() != null)
        {
            Destroy(self);
        }
    }

    public void Constructor(int rotation)
    {
        switch (rotation)
        {
            case 0:
                this.offset = new Vector3(3, 0, -5);
                break;
            case 90:
                this.offset = new Vector3(-5, 0, -3);
                break;
            case -180:
                this.offset = new Vector3(-3,0,5);
                break;
        }
    }
}
