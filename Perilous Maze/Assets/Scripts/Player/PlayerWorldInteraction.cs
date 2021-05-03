using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{
    [SerializeField] GameObject RockPrefab;
    [SerializeField] float ThrowForce;
    Vector3 RockHeight = new Vector3(0, 1, 0);
    float TimeSinceLastAction;
    [SerializeField] Animator animator;
    GameObject newRock;

    public delegate void Pickup(Vector3 position);
    public static event Pickup pickup;

    void OnPickup()
    {
        // check that there are methods that are subscribed to the event
        if (pickup != null)
        {
            pickup(transform.position);
        }
    }

    void ThrowRock()
    {
        if (GetComponent<Inventory>().ThrowRock())
        {
            animator.SetTrigger("Throw");
            newRock = Instantiate(RockPrefab, GameObject.Find("Rock position").transform.position, Quaternion.identity);
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

        if (Input.GetKey(KeyCode.E) && !animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
        {
            OnPickup();
            animator.SetTrigger("Pickup");
        }
    }

    void AtRelease()
    {
        newRock.GetComponent<Release>().AtRelease();
    }
}
