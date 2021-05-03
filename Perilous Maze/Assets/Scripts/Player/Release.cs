using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : MonoBehaviour
{
    GameObject hand;
    [SerializeField] float throwForce;

    void Start()
    {
        // make the rock stick to the hand and don't use gravity
        hand = GameObject.Find("Rock position");
        transform.parent = hand.transform;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void AtRelease()
    {
        // do not stick to the hand anymore, use gravity and add some force so the rock flies
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody>().AddForce(new Vector3(player.transform.forward.x, 2, player.transform.forward.z) * throwForce);
    }
}
