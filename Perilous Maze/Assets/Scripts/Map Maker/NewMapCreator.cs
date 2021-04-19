using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapCreator : MonoBehaviour
{
    [SerializeField] Material CubeMaterial;
    [SerializeField] Material PlaneMaterial;
    [SerializeField] int SpawnChance;
    [SerializeField] List<GameObject> Monsters;
    [SerializeField] GameObject PlayerPrefab;
    public GameObject Player;
    [SerializeField] int MapSize;
    public GameObject[,] Maze;
    Vector3Int StartPoint;
    List<Vector3> route = new List<Vector3>();
    [SerializeField] int BranchingChance;
    readonly Vector3Int[] directions = { new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1), new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0) };
    [SerializeField] GameObject HedgeContainer;

    // Start is called before the first frame update
    void Awake()
    {
        Maze = new GameObject[MapSize, MapSize];
        StartPoint = new Vector3Int(1, 0, Random.Range(0, MapSize));
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, -0.5f, 0);
        plane.transform.localScale = new Vector3(MapSize / 2, 1, MapSize / 2);
        plane.GetComponent<Renderer>().material = PlaneMaterial;

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

        GetComponent<MapMaintainer>().PointsGrid = route;
        GetComponent<MapMaintainer>().Player = Player;
        GetComponent<MapDecorator>().Constructor(MapSize);

        CreateMonsters();
    }

    void BeginMap()
    {
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                Vector3Int position = new Vector3Int(i, 0, j);
                Maze[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Maze[i, j].transform.position = position;
                Maze[i, j].GetComponent<Renderer>().material = CubeMaterial;
                Maze[i, j].transform.SetParent(HedgeContainer.transform);
            }
        }
    }

    void StartRoute()
    {
        GameObject start = Maze[1, StartPoint.z];
        DeleteBlockFromGrid(start);
        MakeRoute(start.transform.position);
    }

    void MakeRoute(Vector3 position)
    {
        foreach (Vector3Int direction in directions)
        {
            int x1 = (int)(position.x + direction.x);
            int z1 = (int)(position.z + direction.z);
            int x2 = (int)(position.x + 2 * direction.x);
            int z2 = (int)(position.z + 2 * direction.z);

            bool x1InRange = x1 > 0 && x1 < MapSize - 1;
            bool z1InRange = z1 > 0 && z1 < MapSize - 1;
            bool x2InRange = x2 > 0 && x2 < MapSize - 1;
            bool z2InRange = z2 > 0 && z2 < MapSize - 1;

            if (x1InRange && z1InRange && x2InRange && z2InRange)
            {
                GameObject[] candidateBlocks = { Maze[x1, z1], Maze[x2, z2] };
                int[,] candidateBlocksCoords = { { x1, z1 }, { x2, z2 } };

                if (candidateBlocks[0] != null && candidateBlocks[1] != null)
                {

                    int random = Random.Range(1, 11);
                    if (random <= BranchingChance)
                    {
                        foreach (GameObject block in candidateBlocks)
                        {
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
                }
            }
        }
    }

    void CreateMonsters()
    {
        // so that I can concentrate on what one monster does
        int counter = 0;
        foreach (Vector3 point in route)
        {
            int random = Random.Range(1, 11);
            int monster = Random.Range(0, Monsters.Count);
            if (random <= SpawnChance && counter < MapSize / 10)
            {
                GameObject newMonster = Instantiate(Monsters[monster], point - new Vector3(0, 0.5f, 0), Quaternion.identity);
                if (newMonster.GetComponent<RouteToPlayer>() != null)
                {
                    newMonster.GetComponent<RouteToPlayer>().Constructor(route, Player);
                }
                if (newMonster.GetComponent<StatePicker>() != null)
                {
                    newMonster.GetComponent<StatePicker>().Constructor(route);
                }
                counter++;
            }
        }
    }

    void CreateTestMonster()
    {
        int random = Random.Range(0, route.Count);
        Vector3 point = route[random];
        GameObject newMonster = Instantiate(Monsters[0], point - new Vector3(0, 0.5f, 0), Quaternion.identity);
        newMonster.GetComponent<StatePicker>().Constructor(route);
    }
}
