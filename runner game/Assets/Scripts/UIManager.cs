using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private int score = 0;
    [SerializeField] private int bestScore = 0;
    [SerializeField] private int health = 3;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private TMP_Text tMPro;

    private void Start()
    {
        score = 0;
        UpdateText();
    }

    public void UpdateScore()
    {
        score += 1;
        if (score > bestScore)
        {
            bestScore = score;
        }
        UpdateText();
    }

    public void NullifyScore()
    {
        score = 0;
        UpdateText();
    }

    public void UpdateHealth(int newHealth)
    {
        health = newHealth;
        UpdateText();
    }

    public void UpdateInvulnerability(bool newIsInvulnerable)
    {
        isInvulnerable = newIsInvulnerable;
        UpdateText();
    }

    private void UpdateText()
    {
        tMPro.text = "Best Score: " + bestScore +  "\nScore: " + score + "\nHealth: " + health + "\nInvulnerability " + isInvulnerable;
    }
}
