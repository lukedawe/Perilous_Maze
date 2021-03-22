using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HedgeMethods
{
    public class VectorMaths
    {

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

    public class Visualiser : MonoBehaviour
    {
        private List<GameObject> spheres;

        public void Constructor(){
            spheres = new List<GameObject>();
        }

        public void VisualisePoints(Vector3[] points)
        {
            
            foreach (Vector3 point in points)
            {
                CreateSphere(point);
            }
        }

        public void CreateSphere(Vector3 position)
        {
            GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newSphere.transform.position = position;
            newSphere.transform.localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
            spheres.Add(newSphere);
        }

        public void CreateBigSphere(Vector3 position)
        {
            GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newSphere.transform.position = position;
            newSphere.transform.localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
            newSphere.GetComponent<Renderer>().material.color = new Color(0, 204, 102);
            spheres.Add(newSphere);
        }
        public void CreateBigBlackSphere(Vector3 position)
        {
            GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newSphere.transform.position = position;
            newSphere.transform.localScale = new Vector3(1, 1, 1);
            newSphere.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            spheres.Add(newSphere);
        }

        public void DeleteAllSpheres()
        {
            if (this.spheres != null)
            {
                foreach (GameObject sphere in this.spheres)
                {
                    Destroy(sphere);
                }
            }
        }
    }
}