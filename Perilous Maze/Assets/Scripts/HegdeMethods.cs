using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HedgeMethods
{
    public class VectorMaths
    {
        public const int BUFFER = 4;

        public static Vector3 CalculateOffset(int rotation, int x, int z)
        {
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
    }
}