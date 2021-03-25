using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
