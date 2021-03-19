using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using HedgeMethods;

public class MapCreator : MonoBehaviour
{

    // all the GameObjects here inherit from HedgeInterface.cs
    public List<GameObject> mazePieces;
    public List<GameObject> edgePieces;
    public List<GameObject> environmentAssets;
    // stores the route to the finish
    public List<(Vector3, Vector3)> route { get; private set; }
    private GameObject map;
    // stores all the placed hedges
    private List<GameObject> placedHedges;
    public int mapSize;

    // Start is called before the first frame update
    void Start()
    {
        this.MapStart();
    }

    public void MapStart()
    {
        placedHedges = new List<GameObject>();
        route = new List<(Vector3, Vector3)>();

        // make the plane for the map
        this.map = this.CreatePlane(new Vector3Int(this.mapSize, 0, 0));
        // This means that we have squares that are mapSize/5
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));

        // make a start for the maze and add it to the route
        int startZCoord = Random.Range(-(mapSize - 1), mapSize - 1);
        Vector3 start = new Vector3(1, 0.5f, startZCoord);

        // route.Add(start);
        while (!RouteFinder(start))
        {

            resetMap();
            continue;
        }
    }

    private bool WillCollide(IHedge hedgeComponent)
    {
        int collisions = 0;
        foreach (Vector3 point in hedgeComponent.points)
        {
            foreach ((Vector3, Vector3) line in this.route)
            {
                (Vector3 point1, Vector3 point2) = line;
                if (point == point1 || point == point2)
                {
                    collisions++;
                }
            }
        }

        // if the point has been used more than once (once because it needs to be connected to the last piece)
        if (collisions > 1)
        {
            return true;
        }

        return false;
    }

    // needs a reference to the position of the hedge, the type of hedge and the same hedge's component
    // returns the position the next hedge should be placed, whether the hedge has been placed and whether
    // the end has been reached 
    private (Vector3, bool, bool) CreateHedge(Vector3 position, GameObject hedge, IHedge hedgeComponent, int yRotation, int xRotation = 0)
    {
        bool hedgeCreated = false;
        hedgeComponent.Constructor(yRotation, position, xRotation);
        Vector3 newCoords = position + hedgeComponent.offset;
        GameObject newHedge = null;

        if (!WillCollide(hedgeComponent) && !hedgeComponent.WillGoOffMap(position, this.map, this.mapSize) && !hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.map, this.mapSize))
        {
            newHedge = CreatePrefab(hedge, position, yRotation, xRotation);
            hedgeCreated = true;
        }
        // if the end of the map has been found
        else if (hedgeComponent.WillGoOffMap(position, this.map, this.mapSize) || hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.map, this.mapSize))
        {
            return (position, false, true);
        }

        // this doesn't entirely work for things that have more than 2 points
        if (hedgeCreated)
        {
            this.route.Add((position, newCoords));
        }
        return (newCoords, hedgeCreated, false);
    }

    // returns the new point to add the next piece to, whether the hedge was placed and whether the end has been found 
    private (Vector3, bool, bool) CreateCrossroads(Vector3 position, GameObject hedge, ICrossRoads hedgeComponent, int newAngle, int yRotation, int xRotation = 0)
    {
        bool hedgeCreated = false;
        hedgeComponent.CrossRoadConstructor(position, yRotation, newAngle);
        Vector3 newCoords = position + hedgeComponent.offset;
        GameObject newHedge = null;

        if (!WillCollide(hedgeComponent) && !hedgeComponent.WillGoOffMap(position, this.map, this.mapSize) && !hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.map, this.mapSize))
        {
            newHedge = CreatePrefab(hedge, position, yRotation, xRotation);
            hedgeCreated = true;
        }
        // if the end of the map has been found
        else if (hedgeComponent.WillGoOffMap(position, this.map, this.mapSize) || hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.map, this.mapSize))
        {
            // this isn't right
            return (position, false, true);
        }

        // this doesn't entirely work for things that have more than 2 points
        if (hedgeCreated)
        {
            this.route.Add((position, newCoords));
        }
        // this isn't right (for testing)
        return (newCoords, true, false);
    }

    public GameObject CreatePlane(Vector3Int position)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = position;
        return plane;
    }

    public bool RouteFinder(Vector3 start)
    {
        // add a wall to the starting position
        CreateHedge(start, mazePieces[0], mazePieces[0].GetComponent<StraightHedge>(), 0);
        Vector3 next = new Vector3(3, 0.5f, start.z);

        // we are starting our journey between -19 and 19.
        bool endFound = false;
        int currentAngle = 0;
        int counter = 0;
        // current position to add the maze piece to
        Vector3 currentPos = next;
        int rightCooldown = 0;
        int leftCooldown = 0;
        int crossroadsCooldown = 0;
        bool hedgeCreated = false;
        // next position to add the maze piece to
        Vector3 nextPos;
        // we need to decide which way we are going to send the player.
        while (!endFound)
        {
            // we have a 40/40 plane in which to make the path
            // we have a hedge that goes 2 blocks forward OR one that goes 6 forward and 5 right.
            int random = Random.Range(0, mazePieces.Count);
            GameObject selectedPiece = mazePieces[random];
            switch (random)
            {
                case 0:
                    (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, selectedPiece, selectedPiece.GetComponent<StraightHedge>(), currentAngle);
                    if (hedgeCreated)
                    {
                        rightCooldown -= 1;
                        leftCooldown -= 1;
                        currentPos = nextPos;
                    }
                    break;
                case 1:
                    int randomDirection = Random.Range(0, 2);

                    // if the random direction selected is right
                    if (randomDirection == 0 && rightCooldown <= 0)
                    {
                        (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle);
                        if (hedgeCreated)
                        {
                            currentAngle = (currentAngle + 90) % 360;
                            rightCooldown = 3;
                            leftCooldown -= 1;
                            currentPos = nextPos;
                        }
                    }
                    // otherwise, head left
                    else if (leftCooldown <= 0)
                    {
                        (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle, 180);
                        if (hedgeCreated)
                        {
                            currentAngle = (currentAngle - 90) % 360;
                            leftCooldown = 3;
                            rightCooldown -= 1;
                            currentPos = nextPos;
                        }
                    }
                    break;
                case 2:

                    if (crossroadsCooldown <= 0)
                    {
                        int randomDirection2 = Random.Range(0, 3);
                        switch (randomDirection2)
                        {
                            case 0:
                                // to go right
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, selectedPiece, selectedPiece.GetComponent<FourWayHedge>(), currentAngle, 90);
                                if (hedgeCreated)
                                {
                                    currentAngle = (currentAngle + 90) % 360;
                                    currentPos = nextPos;
                                }
                                break;
                            case 1:
                                // otherwise, head left
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, selectedPiece, selectedPiece.GetComponent<FourWayHedge>(), currentAngle, -90);
                                if (hedgeCreated)
                                {
                                    currentAngle = (currentAngle - 90) % 360;
                                    currentPos = nextPos;
                                }
                                break;
                            case 2:
                                // or go straight on
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, selectedPiece, selectedPiece.GetComponent<FourWayHedge>(), currentAngle, 0);
                                if (hedgeCreated)
                                {
                                    currentPos = nextPos;
                                }
                                break;
                        }
                        crossroadsCooldown = 5;
                    }

                    // if (leftCooldown <= 0 && rightCooldown <= 0)
                    // {
                    //     GameObject newHedge;
                    //     (nextPos, hedgeCreated, endFound, newHedge) = CreateHedge(currentPos, selectedPiece, selectedPiece.GetComponent<FourWayHedge>(), currentAngle);
                    //     if (hedgeCreated)
                    //     {
                    //         Debug.Log("New angle: " + newHedge.GetComponent<FourWayHedge>().NewRotation);
                    //         currentAngle = newHedge.GetComponent<FourWayHedge>().NewRotation;
                    //         currentPos = nextPos;
                    //     }
                    // }
                    break;

            }
            counter++;
            crossroadsCooldown--;
            if (hedgeCreated == false && endFound == false)
            {
                return false;
            }
        }
        if (counter >= mapSize)
        {
            CreatePrefab(environmentAssets[0], currentPos, currentAngle);
            return true;
        }
        else
        {
            return false;
        }

    }

    public GameObject CreatePrefab(GameObject prefab, Vector3 position, int yRotation, int xRotation = 0)
    {
        // make a rotate object to set the rotation
        GameObject rotate = new GameObject("rotate");
        rotate.transform.Rotate(new Vector3(xRotation, yRotation, 0));
        GameObject newMazePiece = Instantiate(prefab, position, rotate.transform.rotation) as GameObject;
        placedHedges.Add(newMazePiece);
        Destroy(rotate);
        return newMazePiece;
    }

    public void resetMap()
    {
        Debug.ClearDeveloperConsole();
        foreach (GameObject g in placedHedges)
        {
            Destroy(g);
        }
        placedHedges.Clear();
        route.Clear();
    }
}
