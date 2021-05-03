using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnHedge : IHedge
{
    Vector3 TurningPoint { get; set; }
    GameObject[] NextPieces { get; set; }
    GameObject[] GetConnections(GameObject current);
    void SetNextPiece(GameObject next);
}
