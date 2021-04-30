using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int rewardAmount;

    // Start is called before the first frame update
    void Start()
    {
        PlayerWorldInteraction.pickup += Open;
    }

    void OnDisable()
    {
        PlayerWorldInteraction.pickup -= Open;
    }

    void Open(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) < 0.8f)
        {
            // add the score to the player
            GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().variables.addPoints(rewardAmount);
            Debug.Log("IT WORKED");
            animator.SetTrigger("OpenChest");
        }
    }
}