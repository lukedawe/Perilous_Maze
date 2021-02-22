using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameObject straightHedge;
    public GameObject hedgeTurn;
    public GameObject hedgeWallCorner;
    public GameObject straightHedgeWall;

    public int mapSize;
    private List<Vector3Int> route;
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

        // make the plane for the map
        GameObject map = this.CreatePlane(new Vector3Int(this.mapSize, 0, 0));
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));

        // make a start for the maze and add it to the route
        int startZCoord = Random.Range(-mapSize+1, mapSize-1);
        Vector3 start = new Vector3(1, 0.5f, startZCoord);
        // add a wall to the starting position
        this.CreateStraightHedge(start);
        
        Vector3 next = new Vector3(2, 0.5f, startZCoord);
        // route.Add(start);
        RouteFinder(next);
        

        // now we have the start, we always want to head straight into the maze
        // Vector3Int entryPoint = new Vector3Int(-1, 0, 0);
        // route.Add(entryPoint);
        
    }

    public void CreateStraightHedge(Vector3 position)
    {
        // make a cube for testing purposes
        Instantiate(this.straightHedge, position, Quaternion.identity);
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

    public void RouteFinder(Vector3 start)
    {
        // we are starting our journey between -19 and 19.
        bool endFound = false;
        int currentAngle = 0;
        // we need to decide which way we are going to send the player.
        while (!endFound)
        {
            // we have a 40/40 plane in which to make the path
            // we have a hedge that goes 2 blocks forward OR one that goes 6 forward and 5 right.
        }
    }
}
