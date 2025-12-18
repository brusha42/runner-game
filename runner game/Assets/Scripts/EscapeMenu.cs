using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    private bool GameIsPaused = false;
    [SerializeField] private GameObject menu;
    private Keyboard keyboard;

    void Start()
    {
        menu = transform.GetChild(0).gameObject;
        menu.SetActive(false);
        keyboard = Keyboard.current;
    }

    void Update()
    {
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
