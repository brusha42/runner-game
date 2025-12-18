using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text tMPro;
    void Start()
    {
        tMPro.text = "Your Best Score: " + PlayerPrefs.GetInt("Best Score").ToString();
    }

    public void PlayGame()
    {
        Debug.Log("LOL");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
