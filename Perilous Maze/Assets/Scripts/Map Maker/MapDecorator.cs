using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDecorator : MonoBehaviour
{
    [SerializeField] List<GameObject> DecorationPrefabs;

    public void Constructor(int mapSize)
    {
        for (int i = 0; i < mapSize * 5; i++)
        {
            int random = Random.Range(0, DecorationPrefabs.Count);
            Vector3 position = new Vector3(Random.Range(0, mapSize), -0.5f, Random.Range(0, mapSize));
            Instantiate(DecorationPrefabs[random], position, Quaternion.identity);
        }
    }
}
