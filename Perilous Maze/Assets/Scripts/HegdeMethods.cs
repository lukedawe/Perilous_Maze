using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HedgeMethods
{
    public class Converter
    {
        public static Vector3 CalculateOffset(int rotation, int x, int z)
        {
            // calculate the rotation in radians
            double radianRotation = -1*((Math.PI / 180) * rotation);

            // translate the vector by the requred radians
            double x2 = (x * Math.Cos(radianRotation)) - (z * Math.Sin(radianRotation));
            double z2 = (x * Math.Sin(radianRotation)) + (z * Math.Cos(radianRotation));

            Vector3 offset = new Vector3((float)x2, 0, (float)z2);
            return offset;
        }
    }
}