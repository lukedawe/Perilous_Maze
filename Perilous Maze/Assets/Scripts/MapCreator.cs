using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{

    // all the GameObjects here inherit from HedgeInterface.cs
    public List<GameObject> mazePieces;
    public List<GameObject> edgePieces;
    public int mapSize;

    private List<Vector3Int> route;
    private GameObject map;
    private List<GameObject> placedHedges;

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
        this.map = this.CreatePlane(new Vector3Int(this.mapSize, 0, 0));
        // This means that we have squares that are mapSize/5
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));

        // make a start for the maze and add it to the route
        int startZCoord = Random.Range(-(mapSize - 1), mapSize - 1);
        Vector3 start = new Vector3(1, 0.5f, startZCoord);
        // add a wall to the starting position
        CreateHedge(start, mazePieces[0], mazePieces[0].GetComponent<StraightHedge>(), 0);

        Vector3 next = new Vector3(3, 0.5f, startZCoord);
        // route.Add(start);
        RouteFinder(next);
    }

    // needs a reference to the position of the hedge, the type of hedge and the same hedge's component
    private (Vector3, bool) CreateHedge(Vector3 position, GameObject hedge, IHedge hedgeComponent, int yRotation, int xRotation = 0)
    {
        hedgeComponent.Constructor(yRotation, position, xRotation);
        bool hedgeCreated = false;

        if (!hedgeComponent.WillCollide(position) && !hedgeComponent.WillGoOffMap(position, this.map, this.mapSize) && !hedgeComponent.WillGoOffMap((position + hedgeComponent.offset), this.map, this.mapSize))
        {
            // make a rotate object to set the rotation
            GameObject rotate = new GameObject("rotate");
            rotate.transform.Rotate(new Vector3(xRotation, yRotation, 0));

            GameObject newMazePiece = Instantiate(hedge, position, rotate.transform.rotation);

            // destroy the rotation object
            Destroy(rotate);
            hedgeCreated = true;
        }
        Vector3 newCoords = position + hedgeComponent.offset;
        return (newCoords, hedgeCreated);
    }

    public GameObject CreatePlane(Vector3Int position)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = position;
        return plane;
    }

    public void RouteFinder(Vector3 start)
    {
        // we are starting our journey between -19 and 19.
        bool endFound = false;
        int currentAngle = 0;
        int counter = 0;
        Vector3 nextPos = start;
        int rightCooldown = 0;
        int leftCooldown = 0;
        bool hedgeCreated = false;
        // we need to decide which way we are going to send the player.
        while (counter <= this.mapSize)
        {
            Vector3 previous = nextPos;
            // we have a 40/40 plane in which to make the path
            // we have a hedge that goes 2 blocks forward OR one that goes 6 forward and 5 right.
            int random = Random.Range(0, mazePieces.Count);
            GameObject selectedPiece = mazePieces[random];
            switch (random)
            {
                case 0:
                    (nextPos, hedgeCreated) = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<StraightHedge>(), currentAngle);
                    if (hedgeCreated == true)
                    {
                        rightCooldown -= 1;
                        leftCooldown -= 1;
                        counter++;
                    }
                    else
                    {
                        nextPos = previous;
                    }

                    break;
                case 1:
                    int randomDirection = Random.Range(0, 2);

                    // if the random direction selected is right
                    if (randomDirection == 0 && rightCooldown <= 0)
                    {
                        (nextPos, hedgeCreated) = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle);
                        if (hedgeCreated == true)
                        {
                            currentAngle = (currentAngle + 90) % 360;
                            rightCooldown = 3;
                            counter++;
                        }
                        else
                        {
                            nextPos = previous;
                        }
                    }
                    // otherwise, head left
                    else if (leftCooldown <= 0)
                    {
                        (nextPos, hedgeCreated) = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle, 180);
                        if (hedgeCreated == true)
                        {
                            currentAngle = (currentAngle - 90) % 360;
                            leftCooldown = 3;
                            counter++;
                        }
                        else
                        {
                            nextPos = previous;
                        }
                    }
                    break;
            }

            endFound = true;

        }
        return;
    }
}
