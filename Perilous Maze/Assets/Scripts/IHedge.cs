using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

interface IHedge
{
    // to work out if the hedge will collide with anything if it is placed at a certain point
    Vector3 offset { get; set; }
    bool WillGoOffMap(Vector3 position, int mapSize);
    void Constructor(int rotation, Vector3 position, int xRotation);
    Vector3[] connectorPoints { get; set; }
    Vector3[] collisionPoints { get; set; }

}
