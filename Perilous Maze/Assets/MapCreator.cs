using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public int mapSize;
    public List<Vector3Int> route;
    public void AddLine(Vector3Int line)
    {
        route.Add(line);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mapStart();
    }

    public void mapStart()
    {
        bool endFound = false;
        char[] axis = { 'x', 'z' };

        // make the plane ofr the map
        GameObject map = this.CreatePlane(new Vector3Int(this.mapSize, 0, 0));
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));

        // make a start for the maze and add it to the route
        Vector3Int start = new Vector3Int(0, 1, Random.Range(-mapSize, mapSize));
        route.Add(start);

        // add a cube to the starting position
        this.CreateCube(start);

        // now we have the start, we always want to head straight into the maze
        Vector3Int entryPoint = new Vector3Int(-1, 0, 0);
        route.Add(entryPoint);



        // now we have the starting point of the route, we can head in a random direction
        //while (!endFound)
        //{
        //    Vector3Int newLine = new Vector3Int();
        //    char changedAxis = axis[Random.Range(0, 1)];
        //    switch (changedAxis)
        //    {
        //        case 'x':
        //            newLine.x = Random.Range(-10, 10);
        //            break;
        //        case 'z':
        //            newLine.z = Random.Range(-10, 10);
        //            break;
        //    }
        //}
    }

    public void CreateCube(Vector3Int position)
    {
        // make a cube for testing purposes
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
    }

    public GameObject CreatePlane(Vector3Int position)
    {
        // make a cube for testing purposes
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = position;
        return plane;
    }

    public void createHedge(Vector3Int position)
    {
        return;
    }
}
