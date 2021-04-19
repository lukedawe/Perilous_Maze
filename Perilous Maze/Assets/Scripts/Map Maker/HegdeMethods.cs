using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HedgeMethods
{
    public class VectorMaths
    {

        public static Vector3 FindPointClosestToEntity(Transform t, List<Vector3> PointsGrid)
        {
            Vector3 closestPoint = new Vector3();
            if (PointsGrid.Count == 0)
            {
                Debug.LogError("No valid points supplied for calculations");
            }
            float min = (PointsGrid[0] - t.position).magnitude;

            foreach (Vector3 point in PointsGrid)
            {
                float distance = (point - t.position).magnitude;
                if (distance < min)
                {
                    closestPoint = point;
                    min = distance;
                }
            }

            return closestPoint;
        }

        public static Vector3 FindPointClosestToEntity(Vector3 position, List<Vector3> PointsGrid)
        {
            Vector3 closestPoint = new Vector3();
            if (PointsGrid.Count == 0)
            {
                Debug.LogError("No valid points supplied for calculations");
            }
            float min = (PointsGrid[0] - position).magnitude;

            foreach (Vector3 point in PointsGrid)
            {
                float distance = (point - position).magnitude;
                if (distance < min)
                {
                    closestPoint = point;
                    min = distance;
                }
            }

            return closestPoint;
        }

        public static Vector3 CalculateOffset(int rotation, float x, float z)
        {
            rotation %= 360;

            // calculate the rotation in radians
            double radianRotation = -1 * ((Math.PI / 180) * rotation);

            // translate the vector by the requred radians
            double x2 = (x * Math.Cos(radianRotation)) - (z * Math.Sin(radianRotation));
            double z2 = (x * Math.Sin(radianRotation)) + (z * Math.Cos(radianRotation));

            Vector3 offset = new Vector3((float)x2, 0, (float)z2);
            return offset;
        }

        public static bool IsPointInsideRange(Vector3 point, int planeSize)
        {
            int z1 = -planeSize;
            int z2 = planeSize;
            int x1 = 0;
            int x2 = planeSize * 2;

            if (z1 <= point.z && point.z <= z2 && x1 <= point.x && point.x <= x2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TwoVectorsIntersect((Vector3, Vector3) line1, (Vector3, Vector3) line2)
        {
            float m1;
            float c1;
            float m2;
            float c2;

            (m1, c1) = CreateLineEquation(line1);
            (m2, c2) = CreateLineEquation(line1);

            return true;
        }

        public static (float, float) CreateLineEquation((Vector3, Vector3) line)
        {
            Vector3 point1;
            Vector3 point2;
            (point1, point2) = line;
            float m = (point2.x - point1.x) / (point2.z - point1.z);
            float c = point1.x - m * (point1.z);

            return (m, c);
        }

        // from: https://stackoverflow.com/questions/59449628/check-when-two-vector3-lines-intersect-unity3d#59449849
        public static bool LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public static Vector3[] SetArraysEqual(Vector3[] array1, Vector3[] array2)
        {
            for (int i = 0; i < array1.Length; i++)
            {
                {
                    array2[i] = array1[i];
                }
            }
            return array2;
        }
    }
}