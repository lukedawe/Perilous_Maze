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
    // stores the Route to the finish
    private List<(Vector3, Vector3)> Route = new List<(Vector3, Vector3)>();
    private GameObject map;
    // stores all the placed hedges
    private List<GameObject> PlacedHedges = new List<GameObject>();
    // stores all the data about the hedges
    private List<IHedge> IHedgeList = new List<IHedge>();
    public int mapSize;
    public int Cooldown;
    public int CrossRoadsCooldown;
    private List<(ICrossRoads, Vector3, int)> Branches = new List<(ICrossRoads, Vector3, int)>();
    private ICrossRoads currentBranch;
    private bool routeFound;
    public Material Material;
    public GameObject MainCharacter;
    private Vector3 SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        this.MapStart();
    }

    public void MapStart()
    {
        Visualiser visualiser = GetComponent<Visualiser>();

        // make the plane for the map
        this.map = this.CreatePlane(new Vector3Int(this.mapSize, 0, 0));
        // This means that we have squares that are mapSize/5
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));
        map.GetComponent<MeshRenderer>().material = this.Material;

        int counter = 0;
        visualiser.Constructor();

        do
        {
            int startZCoord = Random.Range(-(mapSize - 1), mapSize - 1);
            SpawnPoint = new Vector3(1, 0.5f, startZCoord);
            ResetMap();
            routeFound = RouteFinder(SpawnPoint, 0, false);
            counter++;

            if (counter == 100)
            {
                Debug.Log("<color='red'> counter reached 100 </color>");
            }
        }
        while (!routeFound && counter < 100);
        routeFound = true;

        foreach ((ICrossRoads, Vector3, int) branch in this.Branches)
        {
            (ICrossRoads branchHedge, Vector3 position, int angle) = branch;
            currentBranch = branchHedge;
            Debug.Log(position + "  " + angle);
            RouteFinder(position, angle, true);
        }

        // foreach (GameObject hedge in this.PlacedHedges)
        // {
        //     visualiser.VisualisePoints(hedge.GetComponent<IHedge>().collisionPoints);
        // }

        MakeEdgeHedges();

        GameObject newMainCharacter = CreatePrefab(MainCharacter, SpawnPoint, 90);
        GameObject cameraReference = GameObject.Find("Main Camera");
        cameraReference.GetComponent<CameraBehaviour>().CameraStart(MainCharacter);
    }

    private bool WillCollide(IHedge hedgeComponent, IHedge allowedCollisionPiece)
    {
        int collisions = 0;

        // for the initial route to the finish
        foreach (Vector3 point in hedgeComponent.collisionPoints)
        {
            foreach (IHedge hedge in this.IHedgeList)
            {
                foreach (Vector3 collisionPoint in hedge.collisionPoints)
                {
                    // checks if it is colliding with the last piece that has been placed down (this does not matter)
                    if (point == collisionPoint && hedge != allowedCollisionPiece)
                    {
                        collisions++;
                        // GetComponent<Visualiser>().CreateBigSphere(point);
                    }
                }
            }
        }

        // if the point has been used more than once (once because it needs to be connected to the last piece)
        if (collisions > 0)
        {
            // this is for testing purposes
            return true;
        }

        return false;
    }

    // needs a reference to the position of the hedge, the type of hedge and the same hedge's component
    // returns the position the next hedge should be placed, whether the hedge has been placed and whether
    // the end has been reached 
    private (Vector3, bool, bool) CreateHedge(Vector3 position, GameObject hedge, IHedge hedgeComponent, int yRotation, IHedge allowedCollisionPiece, int xRotation = 0)
    {
        bool hedgeCreated = false;
        hedgeComponent.Constructor(yRotation, position, xRotation);
        Vector3 newCoords = position + hedgeComponent.offset;
        bool edgeFound = false;

        if (!WillCollide(hedgeComponent, allowedCollisionPiece) && !hedgeComponent.WillGoOffMap(position, this.mapSize) && !hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.mapSize))
        {
            hedgeCreated = true;
        }
        // if the end of the map has been found
        else if (hedgeComponent.WillGoOffMap(position, this.mapSize) || hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.mapSize))
        {
            edgeFound = true;
        }
        // else if (WillCollide(hedgeComponent, allowedCollisionPiece) && routeFound)
        // {
        //     GameObject endHedge = CreatePrefab(this.mazePieces[mazePieces.Count - 1], position, yRotation);
        //     CreateHedge(position, endHedge, endHedge.GetComponent<EndOfRouteHedge>(), yRotation, allowedCollisionPiece);
        // }

        if (hedgeCreated)
        {
            UpdateLists(position, newCoords, hedgeComponent, hedge);
        }
        else
        {
            Destroy(hedge);
        }
        // if the hedge won't go off the map, return
        return (newCoords, hedgeCreated, edgeFound);
    }

    // returns the new point to add the next piece to, whether the hedge was placed and whether the end has been found 
    private (Vector3, bool, bool) CreateCrossroads(Vector3 position, GameObject hedge, ICrossRoads hedgeComponent, int newAngle, int yRotation, IHedge allowedCollisionPiece, bool addCrossroadsToList = true, int xRotation = 0)
    {
        bool hedgeCreated = false;
        hedgeComponent.CrossRoadConstructor(position, yRotation, newAngle);
        hedge.transform.position += hedgeComponent.TransformCorrection;
        bool edgeFound = false;
        Vector3 newCoords = position + hedgeComponent.offset;

        // WillCollide does not work for hedges with lots of points yet (this might be because this.PlacedHedges does not account for all points)
        if (!WillCollide(hedgeComponent, allowedCollisionPiece) && !hedgeComponent.WillGoOffMap(position, this.mapSize) && !hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.mapSize))
        {
            // the angle does not matter for these prefabs
            hedgeCreated = true;
        }
        // if the end of the map has been found
        else if (hedgeComponent.WillGoOffMap(position, this.mapSize) || hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.mapSize))
        {
            edgeFound = true;
        }
        // else if (WillCollide(hedgeComponent, allowedCollisionPiece) && routeFound)
        // {
        //     GameObject endHedge = CreatePrefab(this.mazePieces[mazePieces.Count - 1], position, yRotation);
        //     CreateHedge(position, endHedge, endHedge.GetComponent<EndOfRouteHedge>(), yRotation, allowedCollisionPiece);
        // }

        // this doesn't entirely work for things that have more than 2 points
        if (hedgeCreated)
        {
            UpdateLists(position, newCoords, hedgeComponent, hedge);
            if (addCrossroadsToList)
            {
                this.Branches.AddRange(hedgeComponent.Branches);
            }
        }
        else
        {
            Destroy(hedge);
        }
        // this isn't right (for testing)
        return (newCoords, hedgeCreated, edgeFound);
    }

    private void UpdateLists(Vector3 position, Vector3 newCoords, IHedge hedgeComponent, GameObject newHedge)
    {
        this.Route.Add((position, newCoords));
        this.PlacedHedges.Add(newHedge);
        this.IHedgeList.Add(hedgeComponent);
    }

    public GameObject CreatePlane(Vector3Int position)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = position;
        return plane;
    }

    public bool RouteFinder(Vector3 start, int angle, bool firstRouteOfBranch)
    {
        Vector3 next = start;

        if (!routeFound)
        {
            // add a wall to the starting position
            GameObject firstPiece = CreatePrefab(mazePieces[0], start, 0);
            CreateHedge(start, firstPiece, firstPiece.GetComponent<StraightHedge>(), 0, null);
            next = new Vector3(3, 0.5f, start.z);
        }

        // we are starting our journey between -19 and 19.
        bool endFound = false;
        int currentAngle = angle;
        int counter = 0;
        // current position to add the maze piece to
        Vector3 currentPos = next;
        int rightCooldown = 0;
        int leftCooldown = 0;
        int crossroadsCooldown = 0;
        bool hedgeCreated = false;
        // next position to add the maze piece to
        Vector3 nextPos = new Vector3();

        // we need to decide which way we are going to send the player.
        while (!endFound)
        {

            // we have a 40/40 plane in which to make the path
            // we have a hedge that goes 2 blocks forward OR one that goes 6 forward and 5 right.
            int random = Random.Range(0, mazePieces.Count - 1);
            GameObject selectedPiece = mazePieces[random];
            GameObject newHedge;
            IHedge allowedCollisionHedge;

            if (firstRouteOfBranch == true)
            {
                allowedCollisionHedge = this.currentBranch;
            }
            else if (IHedgeList.Count > 0)
            {
                allowedCollisionHedge = this.IHedgeList[IHedgeList.Count - 1];
            }
            else allowedCollisionHedge = null;

            switch (random)
            {
                case 0:
                    newHedge = CreatePrefab(selectedPiece, currentPos, currentAngle);
                    (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, newHedge, newHedge.GetComponent<StraightHedge>(), currentAngle, allowedCollisionHedge);
                    if (hedgeCreated)
                    {
                        rightCooldown--;
                        leftCooldown--;
                        crossroadsCooldown--;
                    }
                    break;
                case 1:
                    int randomDirection = Random.Range(0, 2);

                    // if the random direction selected is right
                    if (randomDirection == 0 && rightCooldown <= 0)
                    {
                        newHedge = CreatePrefab(selectedPiece, currentPos, currentAngle);
                        (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, newHedge, newHedge.GetComponent<TurnHedge>(), currentAngle, allowedCollisionHedge);
                        if (hedgeCreated)
                        {
                            currentAngle = (currentAngle + 90) % 360;
                            rightCooldown = this.Cooldown;
                            leftCooldown--;
                            crossroadsCooldown--;
                        }
                    }
                    // otherwise, head left
                    else if (leftCooldown <= 0)
                    {
                        newHedge = CreatePrefab(selectedPiece, currentPos, currentAngle, 180);
                        (nextPos, hedgeCreated, endFound) = CreateHedge(currentPos, newHedge, newHedge.GetComponent<TurnHedge>(), currentAngle, allowedCollisionHedge, 180);
                        if (hedgeCreated)
                        {
                            currentAngle = (currentAngle - 90) % 360;
                            leftCooldown = this.Cooldown;
                            rightCooldown--;
                            crossroadsCooldown--;
                        }
                    }
                    break;
                case 2:

                    if (crossroadsCooldown <= 0)
                    {
                        int randomCrossRoadsDirection = Random.Range(0, 3);
                        newHedge = CreatePrefab(selectedPiece, currentPos, currentAngle);
                        switch (randomCrossRoadsDirection)
                        {
                            case 0:
                                // to go right
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, newHedge, newHedge.GetComponent<FourWayHedge>(), 90, currentAngle, allowedCollisionHedge, !routeFound);
                                if (hedgeCreated)
                                {
                                    currentAngle = (currentAngle + 90) % 360;
                                }
                                break;
                            case 1:
                                // otherwise, head left
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, newHedge, newHedge.GetComponent<FourWayHedge>(), -90, currentAngle, allowedCollisionHedge, !routeFound);
                                if (hedgeCreated)
                                {
                                    currentAngle = (currentAngle - 90) % 360;
                                }
                                break;
                            case 2:
                                // or go straight on
                                (nextPos, hedgeCreated, endFound) = CreateCrossroads(currentPos, newHedge, newHedge.GetComponent<FourWayHedge>(), 0, currentAngle, allowedCollisionHedge, !routeFound);
                                break;
                        }
                        crossroadsCooldown = this.CrossRoadsCooldown;
                        rightCooldown--;
                        leftCooldown--;
                        if (hedgeCreated && routeFound)
                        {
                            foreach ((ICrossRoads, Vector3, int) branch in newHedge.GetComponent<FourWayHedge>().Branches)
                            {
                                (ICrossRoads branchHedge, Vector3 position, int branchAngle) = branch;
                                currentBranch = branchHedge;
                                Debug.Log(position + "  " + branchAngle);
                                RouteFinder(position, branchAngle, true);
                            }
                        }
                    }

                    break;
            }
            counter++;
            if (hedgeCreated == false && endFound == false)
            {
                return false;
            }
            if (hedgeCreated)
            {
                currentPos = nextPos;
                firstRouteOfBranch = false;
            }
        }
        if (counter >= mapSize && !routeFound)
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
        rotate.transform.Rotate(new Vector3(xRotation, yRotation, 0), Space.World);
        GameObject newMazePiece = Instantiate(prefab, position, rotate.transform.rotation) as GameObject;
        Destroy(rotate);
        return newMazePiece;
    }

    public void ResetMap()
    {
        Debug.ClearDeveloperConsole();
        foreach (GameObject g in PlacedHedges)
        {
            Destroy(g);
        }
        this.PlacedHedges.Clear();
        this.IHedgeList.Clear();
        this.Route.Clear();
        this.Branches.Clear();
        // GetComponent<Visualiser>().DeleteAllSpheres();
    }

    public void MakeEdgeHedges()
    {
        for (int z = -mapSize; z <= mapSize; z += 2)
        {
            CreatePrefab(edgePieces[0], new Vector3(0, (float)0.5, z), 90);
            CreatePrefab(edgePieces[0], new Vector3(mapSize * 2, (float)0.5, z), 90);
        }
        for (int x = 0; x <= mapSize * 2; x += 2)
        {
            CreatePrefab(edgePieces[0], new Vector3(x, (float)0.5, -mapSize), 0);
            CreatePrefab(edgePieces[0], new Vector3(x, (float)0.5, mapSize), 0);
        }
    }
}
