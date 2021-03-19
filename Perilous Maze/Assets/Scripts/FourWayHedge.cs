using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgeMethods;

public class FourWayHedge : MonoBehaviour, ICrossRoads
{
    public Vector3 offset { get; set; }
    public Vector3[] points { get; set; }
    public Vector3[] OtherPoints { get; set; }
    public string word { get; set; }
    public Vector3 TransformCorrection { get; set; }

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, int mapSize)
    {
        return !VectorMaths.IsPointInsideRange(position, mapSize);
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
        Vector3 intermediatePoint;
        this.points = new Vector3[4];
        switch (newRotation)
        {
            // to the right
            case -90:
                // this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, z);
                // this.points[4] = initialPosition + VectorMaths.CalculateOffset(currentRotation + 90, x, z);

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                // This is for the line that appears
                intermediatePoint = initialPosition + VectorMaths.CalculateOffset(currentRotation, 4, 0);

                this.points[0] = initialPosition;
                this.points[1] = initialPosition + this.offset;
                this.points[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z));
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0);

                break;
            // straight on
            case int n when (n == 0 || n == 180 || n == -180):

                x = 6;
                z = 0;

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                this.points[0] = initialPosition;
                this.points[1] = initialPosition + this.offset;
                this.points[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, -(4));
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 2, 4);


                break;
            // to the left
            case 90:
                z = -(z);

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                // This is for the line that appears
                intermediatePoint = initialPosition + VectorMaths.CalculateOffset(currentRotation, 4, 0);

                this.points[0] = initialPosition;
                this.points[1] = initialPosition + this.offset;
                this.points[2] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, -(z));
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, 6, 0);

                break;
            // this should never happen
            default:
                this.points[0] = initialPosition;

                Debug.Log("Exception: " + newRotation);
                break;


        }
        this.GetComponent<LineRenderer>().SetPositions(this.points);
    }
}
