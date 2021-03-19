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

    // returns whether a point will fall off the map
    public bool WillGoOffMap(Vector3 position, GameObject plane, int mapSize)
    {
        return !VectorMaths.IsPointInsideRange(position, mapSize);
    }

    public void Constructor(int rotation, Vector3 initialPosition, int xRotation = 0)
    {
        int randomRotation = Random.Range(0, 3);
        Debug.Log("Rotation before: " + rotation);
        this.GetComponent<LineRenderer>().SetPositions(points);

    }

    public void CrossRoadConstructor(Vector3 initialPosition, int currentRotation, int newRotation)
    {
        int x = 2;
        int z = 3;
        Vector3 intermediatePoint;
        this.points = new Vector3[5];
        switch (newRotation)
        {
            // to the right
            case 0:
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, z);
                this.points[4] = initialPosition + VectorMaths.CalculateOffset(currentRotation - 90, x, z);

                x = 4;
                z = 5;

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                // This is for the line that appears
                intermediatePoint = initialPosition + VectorMaths.CalculateOffset(currentRotation, 4, 0);

                this.points[0] = initialPosition;
                this.points[1] = intermediatePoint;
                this.points[2] = initialPosition + this.offset;

                this.GetComponent<LineRenderer>().SetPositions(points);
                break;
            // straight on
            case 90:
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation + 90, x, z);
                this.points[4] = initialPosition + VectorMaths.CalculateOffset(currentRotation - 90, x, z);

                x = 2;
                z = 0;

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                this.points[0] = initialPosition;
                this.points[1] = (initialPosition + this.offset);

                this.GetComponent<LineRenderer>().SetPositions(points);

                break;
            // to the left
            case -90:
                this.points[3] = initialPosition + VectorMaths.CalculateOffset(currentRotation, x, z);
                this.points[4] = initialPosition + VectorMaths.CalculateOffset(currentRotation + 90, x, z);

                x = 4;
                z = -5;

                this.offset = VectorMaths.CalculateOffset(currentRotation, x, z);

                // This is for the line that appears
                intermediatePoint = initialPosition + VectorMaths.CalculateOffset(currentRotation, 4, 0);

                this.points[0] = initialPosition;
                this.points[1] = intermediatePoint;
                this.points[2] = initialPosition + this.offset;

                this.GetComponent<LineRenderer>().SetPositions(points);
                break;

        }
    }
}
