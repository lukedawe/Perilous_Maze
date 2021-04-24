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
    [HideInInspector] public GameObject Player;
    [SerializeField] int MapSize;
    [HideInInspector] public GameObject[,] Maze;
    Vector3Int StartPoint;
    List<Vector3> route = new List<Vector3>();
    [Range(0, 10)]
    [SerializeField] int BranchingChance;
    readonly Vector3Int[] directions = { new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1), new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0) };
    [SerializeField] GameObject HedgeContainer;
    public Vector3 EndPoint;
    [SerializeField] GameObject EndEffect;
    [HideInInspector] GameObject Enemies;
    [SerializeField] GameObject SafeHouse;
    [SerializeField] GameObject playerVariables;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        Maze = new GameObject[MapSize, MapSize];
        StartPoint = new Vector3Int(1, 0, Random.Range(1, MapSize - 2));
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
                // Maze[i, j].transform.localScale = new Vector3(1, 1f, 1);
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
                    Maze[i, j] = null;
                }
            }
        }
    }

    void CreateMonsters()
    {
        // so that I can concentrate on what one monster does
        int counter = 0;
        for (int i = route.Count - 1; i >= 0; i--)
        {
            int random = Random.Range(1, 11);
            int monster = Random.Range(0, Monsters.Count);
            if (random <= SpawnChance && counter < MapSize / 5)
            {
                GameObject newMonster = Instantiate(Monsters[monster], route[i], Quaternion.identity);
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
        GameObject newMonster = Instantiate(Monsters[0], point, Quaternion.identity);
        newMonster.GetComponent<StatePicker>().Constructor(route);
    }

    void CreateEndPoint()
    {
        for (int i = MapSize - 1; i >= 0; i--)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if (Maze[i, j] == null)
                {
                    EndPoint = new Vector3(i, 0, j);
                    // Instantiate(EndEffect, EndPoint, Quaternion.identity);
                    goto end;
                }

            }
        }
    end:;
        float k = EndPoint.x + 1;
        while (k < MapSize)
        {
            DeleteBlockFromGrid(Maze[(int)k, (int)EndPoint.z]);
            k++;
        }
        EndPoint = new Vector3(k - 1, 0, EndPoint.z);
        Instantiate(SafeHouse, EndPoint + new Vector3(6, 0, 1.5f), Quaternion.Euler(0, -90f, 0));
    }
}
