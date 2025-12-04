using UnityEngine;

public class ShieldBonus : MonoBehaviour
{
    [SerializeField] private float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.MakeInvulnerable(duration);
            Destroy(gameObject);
        }
    }
}
