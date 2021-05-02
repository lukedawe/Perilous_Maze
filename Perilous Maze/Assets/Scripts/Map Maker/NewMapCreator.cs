using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapCreator : MonoBehaviour
{
    [SerializeField] Material CubeMaterial;
    [SerializeField] Material PlaneMaterial;
    [Range(0, 10)]
    [SerializeField] int SpawnChance;
    [Range(0, 10)]
    [SerializeField] int ChestSpawnChance;
    [SerializeField] List<GameObject> Monsters;
    [SerializeField] GameObject PlayerPrefab;
    [HideInInspector] public GameObject Player;
    int MapSize;
    [HideInInspector] public GameObject[,] Maze;
    Vector3 StartPoint;
    [HideInInspector] public List<Vector3> route = new List<Vector3>();
    [Range(0, 10)]
    [SerializeField] int BranchingChance;
    readonly Vector3Int[] directions = { new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1), new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0) };
    [SerializeField] GameObject HedgeContainer;
    public Vector3 EndPoint;
    [SerializeField] GameObject EndEffect;
    [HideInInspector] GameObject Enemies;
    [SerializeField] GameObject SafeHouse;
    [SerializeField] GameObject playerVariables;
    [SerializeField] List<GameObject> Chests;

    // Start is called before the first frame update
    void Awake()
    {
        chooseDifficulty();

        Time.timeScale = 1f;
        Maze = new GameObject[MapSize, MapSize];
        StartPoint = new Vector3(1, 0, Random.Range(1, MapSize - 2));
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(MapSize / 2, 1, MapSize / 2);
        plane.GetComponent<Renderer>().material = PlaneMaterial;
        plane.layer = 3;

        do
        {
            ResetMap();
            BeginMap();
            StartRoute();
        }
        while (route.Count < MapSize * 5);

        Player = Instantiate(PlayerPrefab, StartPoint, Quaternion.identity);

        GameObject cameraReference = GameObject.Find("Main Camera");
        cameraReference.GetComponent<CameraBehaviour>().CameraStart(Player);

        CreateEndPoint();

        GetComponent<MapMaintainer>().PointsGrid = route;
        GetComponent<MapMaintainer>().Player = Player;

        GetComponent<MapDecorator>().Constructor(MapSize);

        CreateMonsters();
        spawnChests();

    }

    void chooseDifficulty()
    {
        string difficulty;

        if (PlayerPrefs.HasKey("Difficulty"))
        {
            difficulty = PlayerPrefs.GetString("Difficulty");
        }
        else
        {
            difficulty = "Medium";
        }

        Debug.Log("Difficulty: " + difficulty);

        switch (difficulty)
        {
            case "Easy":
                MapSize = 20;
                break;
            case "Medium":
                MapSize = 30;
                break;
            case "Hard":
                MapSize = 40;
                break;
            default:
                MapSize = 30;
                break;
        }
    }

    void BeginMap()
    {
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                Vector3 position = new Vector3(i, 0.5f, j);
                Maze[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Maze[i, j].transform.position = position;
                Maze[i, j].GetComponent<Renderer>().material = CubeMaterial;
                Maze[i, j].transform.SetParent(HedgeContainer.transform);
                Maze[i, j].layer = 3;
            }
        }
    }

    void StartRoute()
    {
        GameObject start = Maze[(int)StartPoint.x, (int)StartPoint.z];
        DeleteBlockFromGrid(start);
        MakeRoute(start.transform.position);
    }

    void MakeRoute(Vector3 position)
    {
        foreach (Vector3Int direction in directions)
        {
            // x1, z1 and x2, z2 are the blocks that will be deleted, x3, z3 is the block that will not be deleted
            // but x1, z1 and x2, z2 will not be deleted if x3 is already deleted

            int x1 = (int)(position.x + direction.x);
            int z1 = (int)(position.z + direction.z);
            int x2 = (int)(position.x + 2 * direction.x);
            int z2 = (int)(position.z + 2 * direction.z);
            int x3 = (int)(position.x + 3 * direction.x);
            int z3 = (int)(position.z + 3 * direction.z);

            bool x1InRange = x1 > 0 && x1 < MapSize - 1;
            bool z1InRange = z1 > 0 && z1 < MapSize - 1;
            bool x2InRange = x2 > 0 && x2 < MapSize - 1;
            bool z2InRange = z2 > 0 && z2 < MapSize - 1;
            bool x3InRange = x3 > 0 && x3 < MapSize - 1;
            bool z3InRange = z3 > 0 && z3 < MapSize - 1;

            // make sure that everything is within the maze
            if (x1InRange && z1InRange && x2InRange && z2InRange && x3InRange && z3InRange)
            {
                GameObject[] candidateBlocks = { Maze[x1, z1], Maze[x2, z2] };

                // if all the blocks are still there...
                if (candidateBlocks[0] != null && candidateBlocks[1] != null && Maze[x3, z3] != null)
                {

                    int random = Random.Range(1, 11);
                    if (random <= BranchingChance)
                    {
                        foreach (GameObject block in candidateBlocks)
                        {
                            // delete the blocks and continue from x2, z2
                            DeleteBlockFromGrid(block);
                            MakeRoute(candidateBlocks[1].transform.position);
                        }
                    }
                }
            }
        }
    }

    void DeleteBlockFromGrid(GameObject block)
    {
        Vector3 temp = new Vector3(block.transform.position.x, 0, block.transform.position.z);
        // add the position of the block to the route, so that the enemies know where they can walk etc.
        route.Add(temp);
        Maze[(int)block.transform.position.x, (int)block.transform.position.z] = null;
        Destroy(block);
    }

    void ResetMap()
    {
        route.Clear();
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if (Maze[i, j] != null)
                {
                    Destroy(Maze[i, j]);
                    Maze[i, j] = null;
                }
            }
        }
    }

    void CreateMonsters()
    {

        int counter = 0;
        for (int i = route.Count - 1; i >= 0; i--)
        {
            int random = Random.Range(1, 11);
            int monster = Random.Range(0, Monsters.Count);
            // limit the number of enemies per level
            if (random <= SpawnChance && counter < MapSize / 7)
            {
                // if the point is too close to the player
                if (Vector3.Distance(route[i], StartPoint) < 10)
                {
                    continue;
                }

                GameObject newMonster = Instantiate(Monsters[monster], route[i], Quaternion.identity);
                // for the plague doctor enemy
                if (newMonster.GetComponent<RouteToPlayer>() != null)
                {
                    newMonster.GetComponent<RouteToPlayer>().Constructor(route, Player);
                }
                // for the blob and bear enemies
                if (newMonster.GetComponent<StatePicker>() != null)
                {
                    newMonster.GetComponent<StatePicker>().Constructor(route);
                }
                counter++;
            }
        }
    }

    void spawnChests()
    {

        int chestCounter = 0;

        foreach (Vector3 position in route)
        {
            Vector3 directionToFace = new Vector3(0, 0, 0);
            // keeps track of the number of hedges that surround a block
            int surroundedByCounter = 0;
            foreach (Vector3 d in directions)
            {

                Vector3 newPosition = new Vector3(position.x + d.x, 0, position.z + d.z);

                bool xInRange = newPosition.x > 0 && newPosition.x < MapSize - 1;
                bool zInRange = newPosition.z > 0 && newPosition.z < MapSize - 1;

                if (!xInRange || !zInRange)
                {
                    continue;
                }
                if (Maze[(int)(newPosition.x), (int)(newPosition.z)])
                {
                    surroundedByCounter++;
                }
                else
                {
                    directionToFace = newPosition;
                }

                // if it is the last direction to check AND the position is surrounded by 3 hedges (aka it's a dead end)
                if (d == directions[3] && surroundedByCounter == 3)
                {
                    int random = Random.Range(1, 11);
                    int chestType = Random.Range(0, Chests.Count - 1);
                    // limit the number of enemies per level
                    if (random <= ChestSpawnChance && chestCounter < MapSize / 10)
                    {
                        float angle = Vector3.Angle(directionToFace, position);
                        GameObject g = Instantiate(Chests[chestType], position, Quaternion.identity);
                        g.transform.LookAt(directionToFace, Vector3.up);
                        chestCounter++;
                    }
                }
            }
        }
    }

    // for testing, spawn a monster of a certain type
    void CreateTestMonster()
    {
        int random = Random.Range(0, route.Count);
        Vector3 point = route[random];
        GameObject newMonster = Instantiate(Monsters[0], point, Quaternion.identity);
        newMonster.GetComponent<StatePicker>().Constructor(route);
    }

    // find a suitable end point
    void CreateEndPoint()
    {
        for (int i = MapSize - 1; i >= 0; i--)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if (Maze[i, j] == null)
                {
                    // this is the point that is the closest to the other side of the maze
                    EndPoint = new Vector3(i, 0, j);
                    goto end;
                }

            }
        }
    end:;
        float k = EndPoint.x + 1;

        // delete blocks from the previous end point until you reach the actual edge of the maze
        while (k < MapSize)
        {
            DeleteBlockFromGrid(Maze[(int)k, (int)EndPoint.z]);
            k++;
        }
        // save the end point and spawn the house
        EndPoint = new Vector3(k - 1, 0, EndPoint.z);
        Instantiate(SafeHouse, EndPoint + new Vector3(6, 0, 1.5f), Quaternion.Euler(0, -90f, 0));
    }
}
