using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int rewardAmount;
    [SerializeField] int rewardAmountRocks;
    [SerializeField] AudioSource audioPlayer;
    bool opened = false;

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
        if (Vector3.Distance(transform.position, position) < 0.8f && !opened)
        {
            int random = Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().variables.addPoints(rewardAmount);
                    break;
                case 1:
                    // if we can add the stones then do it, otherwise give the player points
                    // this way the player is always being rewarded for opening chests
                    if (!GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().Player.GetComponent<Inventory>().PickupRock(rewardAmountRocks))
                    {
                        GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().variables.addPoints(rewardAmount);
                    }
                    break;
                default:
                    GameObject.Find("Map Modifier").GetComponent<MapMaintainer>().variables.addPoints(rewardAmount);
                    break;
            }

            // add the score to the player
            animator.SetTrigger("OpenChest");
            audioPlayer.Play();
            opened = true;
        }
    }
}