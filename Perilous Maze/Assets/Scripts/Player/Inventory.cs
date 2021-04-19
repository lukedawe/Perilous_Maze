using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int stones;
    [SerializeField] Text stoneDisplay;

    void Start()
    {
        // start the player off with a full inventory of stones
        this.stones = 3;
        stoneDisplay = GameObject.Find("Stone Count").GetComponent<Text>();
        UpdateStoneCount();
    }

    public bool ThrowRock()
    {
        if (stones > 0)
        {
            this.stones--;
            UpdateStoneCount();
            return true;
        }
        return false;
    }

    public bool PickupRock()
    {
        if (stones < 3)
        {
            this.stones++;
            UpdateStoneCount();
            return true;
        }
        return false;
    }

    void UpdateStoneCount()
    {
        stoneDisplay.GetComponent<Text>().text = "Stones: " + stones;
    }
}
