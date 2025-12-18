using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private Renderer playerRenderer;
    
    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private Color invulnerableColor = Color.blue;
    
    [SerializeField] private float colorFlashDuration = 0.5f;
    
    private Material playerMaterial;
    private Color originalColor;
    private Coroutine invulnerabilityCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth);
        UIManager.Instance.UpdateInvulnerability(isInvulnerable);
        playerRenderer = GetComponent<Renderer>();
        playerMaterial = playerRenderer.material;
        originalColor = playerMaterial.color;
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
        playerMaterial.color = invulnerableColor;

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        UIManager.Instance.UpdateInvulnerability(isInvulnerable);
        playerMaterial.color = originalColor;
        invulnerabilityCoroutine = null;
    }

    IEnumerator FlashColor(Color flashColor)
    {
        Color currentColor = playerMaterial.color;
        playerMaterial.color = flashColor;
        yield return new WaitForSeconds(colorFlashDuration);
        playerMaterial.color = currentColor;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        currentHealth -= damage;
        UIManager.Instance.UpdateHealth(currentHealth);
        StartCoroutine(FlashColor(damageColor));
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        currentHealth = Math.Min(currentHealth + heal, maxHealth);
        UIManager.Instance.UpdateHealth(currentHealth);
        StartCoroutine(FlashColor(healColor));
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
