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
        // start the player off with an inventory of 3 stones
        this.stones = 3;
        stoneDisplay = GameObject.Find("Stone Count").GetComponent<Text>();
        UpdateStoneCount();
    }

    // this is called when the player is throwing a rock
    // returns false if the player cannot throw a rock
    // returns true if the player can throw a rock
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

    public bool PickupRock(int numberOfRocks = 1)
    {
        // we want to have a limit of 5 stones in the inventory at one time
        if (this.stones >= 5)
        {
            this.stones = 5;
            // returning false means that the rocks cannot be added to the player's inventory
            return false;
        }

        this.stones += numberOfRocks;
        if (this.stones > 5)
        {
            this.stones = 5;
        }
        UpdateStoneCount();

        return true;
    }

    // display the number of stones that the player currently has
    void UpdateStoneCount()
    {
        stoneDisplay.GetComponent<Text>().text = "Stones: " + stones + " / 5";
    }
}
