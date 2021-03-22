using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICrossRoads : IHedge
{
    Vector3[] ExtraConnectionPoints { get; set; }
    void CrossRoadConstructor(Vector3 initialPosition, int currentrRotation, int newRotation);

    Vector3 TransformCorrection { get; set; }
}
