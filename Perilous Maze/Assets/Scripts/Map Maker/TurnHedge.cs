using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;
using System;

public class TurnHedge : MonoBehaviour, ITurnHedge
{
    public Vector3 offset { get; set; }
    public Vector3[] connectorPoints { get; set; }
    public Vector3[] collisionPoints { get; set; }
    public Vector3 TurningPoint { get; set; }
    public GameObject[] NextPieces { get; set; }


    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        foreach (Vector3 point in this.collisionPoints)
        {
            if (!VectorMaths.IsPointInsideRange(position, mapSize))
            {
                return true;
            }
        }
        return false;
    }

    public void Constructor(int rotation, Vector3 initialPosition, int xRotation = 0)
    {
        this.connectorPoints = new Vector3[3];
        this.collisionPoints = new Vector3[9];

        int x = 4;
        int z = 4;

        if (xRotation != 180)
        {
            z = -(z);
        }

        this.offset = VectorMaths.CalculateOffset(rotation, x, z);

        // This is for the line that appears
        Vector3 intermediatePoint = initialPosition + VectorMaths.CalculateOffset(rotation, 4, 0);

        this.connectorPoints[0] = initialPosition;
        this.connectorPoints[1] = intermediatePoint;
        this.connectorPoints[2] = initialPosition + this.offset;

        this.GetComponent<LineRenderer>().SetPositions(connectorPoints);
        this.collisionPoints = VectorMaths.SetArraysEqual(this.connectorPoints, this.collisionPoints);

        this.collisionPoints[3] = initialPosition + VectorMaths.CalculateOffset(rotation, 1, 0);
        this.collisionPoints[4] = initialPosition + VectorMaths.CalculateOffset(rotation, 2, 0);

        this.collisionPoints[5] = this.TurningPoint = initialPosition + VectorMaths.CalculateOffset(rotation, 3, 0);

        // if z is negative, z/4=-1, if z is positive, z/4=1
        this.collisionPoints[6] = initialPosition + VectorMaths.CalculateOffset(rotation, 4, (z / 4) * 1);
        this.collisionPoints[7] = initialPosition + VectorMaths.CalculateOffset(rotation, 4, (z / 4) * 2);
        this.collisionPoints[8] = initialPosition + VectorMaths.CalculateOffset(rotation, 4, (z / 4) * 3);

    }

    public void SetNextPiece(GameObject next)
    {
        if (this.NextPieces == null)
        {
            this.NextPieces = new GameObject[2];
        }

        for (int i = 0; i < this.NextPieces.Length; i++)
        {
            if (this.NextPieces[i] == null)
            {
                this.NextPieces[i] = next;
            }
        }
    }

    public GameObject[] GetConnections(GameObject current)
    {
        GameObject[] connection = new GameObject[1];
        foreach (GameObject piece in this.NextPieces)
        {
            if (piece != current)
            {
                connection[0] = piece;
            }
        }
        return connection;
    }
}
