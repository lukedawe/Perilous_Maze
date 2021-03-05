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
            double radianRotation = -1*((Math.PI / 180) * rotation);

            // translate the vector by the requred radians
            double x2 = (x * Math.Cos(radianRotation)) - (z * Math.Sin(radianRotation));
            double z2 = (x * Math.Sin(radianRotation)) + (z * Math.Cos(radianRotation));

            Vector3 offset = new Vector3((float)x2, 0, (float)z2);
            return offset;
        }

        public static bool IsPointInsideRange(Vector3 point, int mapSize){
           int z1 = -mapSize;
           int z2 = mapSize;
           int x1 = 0;
           int x2 = mapSize*2;

           if(z1 <= point.z && point.z <= z2 && x1 <= point.x && point.x <= x2){
               return true;
           }
           else{
               return false;
           }
        }
    }
}