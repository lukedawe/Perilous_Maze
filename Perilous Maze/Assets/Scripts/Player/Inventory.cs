using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int stones;

    void Start()
    {
        // start the player off with a full inventory of stones
        this.stones = 3;
    }

    public void ThrowRock()
    {
        if (stones > 0) this.stones--;
    }

    public void PickupRock()
    {
        if (stones < 3) this.stones++;
    }
}
