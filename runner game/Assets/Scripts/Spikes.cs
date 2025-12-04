using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int spikesDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(spikesDamage);
        }
    }
}
