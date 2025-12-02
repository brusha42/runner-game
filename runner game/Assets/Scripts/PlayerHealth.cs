using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log($"Столкнулся с: {collision.gameObject.name}");
            Die();
        }
    }

    private void Die()
    {
        // Debug.Log("YOUR SCORE": );
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
