using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface HedgeInterface
{
    bool WillCollide(Vector3 currentPos);
    Vector3[] NextPoints();
}
