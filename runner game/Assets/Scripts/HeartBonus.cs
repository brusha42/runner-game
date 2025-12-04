using UnityEngine;

public class HeartBonus : MonoBehaviour
{
    [SerializeField] private int heal = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.Heal(heal);
            Destroy(gameObject);
        }
    }
}
