using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.PlayerDeath();
        }
    }
}
