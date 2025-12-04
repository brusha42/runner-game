using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private bool isInvulnerable = false;
    private Coroutine invulnerabilityCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth);
        UIManager.Instance.UpdateInvulnerability(isInvulnerable);
    }

    public void MakeInvulnerable(float duration)
    {
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
        }
        invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine(duration));
    }

    IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;
        UIManager.Instance.UpdateInvulnerability(isInvulnerable);
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
        UIManager.Instance.UpdateInvulnerability(isInvulnerable);
        invulnerabilityCoroutine = null;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        currentHealth -= damage;
        UIManager.Instance.UpdateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        currentHealth = Math.Min(currentHealth + heal, maxHealth);
        UIManager.Instance.UpdateHealth(currentHealth);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isInvulnerable)
            {
                BoxCollider boxCollider = collision.gameObject.GetComponent<BoxCollider>();
                boxCollider.isTrigger = true;
            }
            else
            {
                Die();
            }
        }
    }

    private void Die()
    {
        UIManager.Instance.NullifyScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
