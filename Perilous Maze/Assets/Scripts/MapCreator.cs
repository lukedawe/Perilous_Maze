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
        map.transform.localScale = new Vector3Int((this.mapSize / 5), 1, (this.mapSize / 5));

        // make a start for the maze and add it to the route
        int startZCoord = Random.Range(-(mapSize+1), mapSize-1);
        Vector3 start = new Vector3(1, 0.5f, startZCoord);
        // add a wall to the starting position
        CreateHedge(start, mazePieces[0], mazePieces[0].GetComponent<StraightHedge>(), 0);
        
        Vector3 next = new Vector3(3, 0.5f, startZCoord);
        // route.Add(start);
        RouteFinder(next);
    }

    // needs a reference to the position of the hedge, the type of hedge and the same hedge's component
    private Vector3 CreateHedge(Vector3 position, GameObject hedge, IHedge hedgeComponent, int yRotation, int xRotation = 0)
    {
        // we do not want this to be StraightHedge
        if(!hedgeComponent.WillCollide(position) && !hedgeComponent.WillGoOffMap(position, this.map)){
            hedgeComponent.Constructor(yRotation, xRotation);
            // make a rotate object to set the rotation
            GameObject rotate = new GameObject("rotate");
            rotate.transform.Rotate(new Vector3 (xRotation,yRotation,0));

            GameObject newMazePiece = Instantiate(hedge, position, rotate.transform.rotation);

            // destroy the rotation object
            Destroy(rotate);
        }
        Vector3 newCoords = position + hedgeComponent.offset;
        return newCoords;
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
        // we need to decide which way we are going to send the player.
        while (counter <=20)
        {
            // we have a 40/40 plane in which to make the path
            // we have a hedge that goes 2 blocks forward OR one that goes 6 forward and 5 right.
            int random = Random.Range(0, mazePieces.Count);
            GameObject selectedPiece = mazePieces[random];
            switch (random){
                case 0:
                    nextPos = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<StraightHedge>(), currentAngle);
                    break;
                case 1:
                    int randomDirection = Random.Range(0, 2);
                    
                    // if the random direction selected is right
                    if(randomDirection == 0){
                        nextPos = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle);
                        currentAngle=(currentAngle+90)%360;
                    }
                    // otherwise, head left
                    else{
                        nextPos = CreateHedge(nextPos, selectedPiece, selectedPiece.GetComponent<TurnHedge>(), currentAngle, 180);
                        currentAngle=(currentAngle-90)%360;
                    }
                    break;
            }

            endFound = true;
            counter ++;
        }
        return;
    }
}
