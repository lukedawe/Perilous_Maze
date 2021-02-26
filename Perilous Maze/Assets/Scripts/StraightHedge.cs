using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
