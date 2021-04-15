using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{
    [SerializeField] GameObject RockPrefab;
    [SerializeField] float ThrowForce;
    Vector3 RockHeight = new Vector3(0, 1, 0);
    float TimeSinceLastAction;
    void ThrowRock()
    {
        if (GetComponent<Inventory>().ThrowRock())
        {
            GameObject newRock = Instantiate(RockPrefab, transform.position + transform.forward + RockHeight, Quaternion.identity);
            newRock.GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x, 3, transform.forward.z) * ThrowForce);
        }
    }
    void FixedUpdate()
    {
        TimeSinceLastAction += Time.deltaTime;
        if (Input.GetKey(KeyCode.R) && TimeSinceLastAction > 1)
        {
            TimeSinceLastAction = 0;
            ThrowRock();
        }
    }
}
