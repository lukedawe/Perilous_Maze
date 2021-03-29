using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class FourWayHedge : MonoBehaviour, ICrossRoads
{
    public Vector3 offset { get; set; }
    public Vector3[] connectorPoints { get; set; }
    public Vector3[] collisionPoints { get; set; }
    public string word { get; set; }
    public Vector3 TransformCorrection { get; set; }
    public (ICrossRoads, Vector3, int)[] Branches { get; set; } = new (ICrossRoads, Vector3, int)[2];

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        foreach(Vector3 point in this.collisionPoints){
            if(!VectorMaths.IsPointInsideRange(position, mapSize)){
                return true;
            }
        }
        foreach(Vector3 point in this.connectorPoints){
            if(!VectorMaths.IsPointInsideRange(position, mapSize)){
                return true;
            }
        }
        return false;
    }

    public void Constructor(int rotation, Vector3 initialPosition, int xRotation = 0)
    {
        return;
    }

    public void CrossRoadConstructor(Vector3 initialPosition, int currentRotation, int newRotation)
    {
        // because the piece wasn't spawning in the right position
        this.TransformCorrection = VectorMaths.CalculateOffset(currentRotation, (float)-0.5, -1);

        int x = 2;
        int z = 4;

        this.connectorPoints = new Vector3[4];
        this.collisionPoints = new Vector3[16];

        switch (newRotation)
        {
            // to the left
            case -90:

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);
                this.connectorPoints[0] = initialPosition;
                this.connectorPoints[1] = initialPosition + this.offset;
                this.connectorPoints[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z));
                this.connectorPoints[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0);
                this.Branches[0] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0), currentRotation);
                this.Branches[1] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z)), currentRotation + 90);

                break;
            // straight on
            case int n when (n == 0 || n == 180 || n == -180):

                this.offset = VectorMaths.CalculateOffset(currentRotation, 6, 0);
                this.connectorPoints[0] = initialPosition;
                this.connectorPoints[1] = initialPosition + this.offset;
                this.connectorPoints[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z));
                this.connectorPoints[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, z);
                this.Branches[0] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z)), currentRotation + 90);
                this.Branches[1] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, x, z), currentRotation - 90);

                break;
            // to the right
            case 90:

                z = -(z);
                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);
                this.connectorPoints[0] = initialPosition;
                this.connectorPoints[1] = initialPosition + this.offset;
                this.connectorPoints[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z));
                this.connectorPoints[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0);
                this.Branches[0] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0), currentRotation);
                this.Branches[1] = (this, initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z)), currentRotation - 90);


                break;
            // this should never happen
            default:
                this.connectorPoints[0] = initialPosition;

                Debug.Log("Exception: " + newRotation);
                break;

        }
        this.GetComponent<LineRenderer>().SetPositions(this.connectorPoints);

        this.collisionPoints[4] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 1, 0);
        this.collisionPoints[5] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, 0);
        this.collisionPoints[6] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 3, 0);
        this.collisionPoints[7] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 4, 0);
        this.collisionPoints[8] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 5, 0);
        this.collisionPoints[9] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, -3);
        this.collisionPoints[10] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, -2);
        this.collisionPoints[11] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, -1);
        this.collisionPoints[12] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, 1);
        this.collisionPoints[13] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, 2);
        this.collisionPoints[14] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, 3);
        this.collisionPoints[15] = initialPosition + VectorMaths.CalculateOffset(currentRotation, -1, 0);

        collisionPoints = VectorMaths.SetArraysEqual(this.connectorPoints, collisionPoints);

    }
}
