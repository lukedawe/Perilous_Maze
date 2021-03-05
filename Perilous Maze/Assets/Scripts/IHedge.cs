using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

interface IHedge
{
    // to work out if the hedge will collide with anything if it is placed at a certain point
    bool WillCollide(Vector3 currentPos);
    Vector3 offset { get; set; }
    bool WillGoOffMap(Vector3 position, GameObject plane, int mapSize);
    void Constructor(int rotation, Vector3 position, int xRotation);
    int collisions { get; set; }
    List <Vector3> Lines{get; set;}
    
}
