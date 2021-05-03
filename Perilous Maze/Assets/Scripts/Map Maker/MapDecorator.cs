using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDecorator : MonoBehaviour
{
    [SerializeField] List<GameObject> DecorationPrefabs;
    GameObject DecorationContainer;
    [SerializeField] GameObject tree;
    int mapSize;
    [SerializeField] GameObject dust;
    [SerializeField] int decorationIntensity;
    List<Vector3> cannotPlaceDecoration = new List<Vector3>();


    public void Constructor(int mapSize)
    {
        this.mapSize = mapSize;
        DecorationContainer = GameObject.Find("Decoration Container");

        for (int i = 0; i < mapSize * decorationIntensity; i++)
        {
            int random = Random.Range(0, DecorationPrefabs.Count);
            Vector3 position;

            do
            {
                position = new Vector3(Random.Range(0, mapSize), 0, Random.Range(0, mapSize));
            }
            while (cannotPlaceDecoration.Contains(position) && GetComponent<NewMapCreator>().route.Contains(position));

            GameObject temp = Instantiate(DecorationPrefabs[random], position, Quaternion.identity);
            temp.transform.SetParent(DecorationContainer.transform);
            cannotPlaceDecoration.Add(temp.transform.position);
        }
        SurroundMapWithTrees();
        AtmosphericDust();
    }

    void SurroundMapWithTrees()
    {
        for (int i = -10; i < mapSize + 8; i++)
        {
            for (int j = -10; j < mapSize + 8; j++)
            {
                if (i < -1 || i >= mapSize + 1 || j < -1 || j >= mapSize + 1)
                {
                    GameObject newTree = Instantiate(tree, new Vector3(i, 0, j), Quaternion.identity);
                    newTree.transform.SetParent(DecorationContainer.transform);
                }
            }
        }
    }

    void AtmosphericDust()
    {
        for (int i = 0; i < mapSize; i += 10)
        {
            for (int j = 0; j < mapSize; j += 10)
            {
                GameObject newDust = Instantiate(dust, new Vector3(i, 0, j), Quaternion.identity);
                newDust.transform.SetParent(DecorationContainer.transform);
            }
        }
    }
}
