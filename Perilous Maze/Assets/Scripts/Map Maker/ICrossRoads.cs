using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrossRoads : ITurnHedge
{
    // for keeping track of the position and direction that branches must take from the crossroads
    (ICrossRoads, Vector3, int)[] Branches { get; set; }
    void CrossRoadConstructor(Vector3 initialPosition, int currentrRotation, int newRotation);
    Vector3 TransformCorrection { get; set; }
}
