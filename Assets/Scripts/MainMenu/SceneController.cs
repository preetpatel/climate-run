using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static bool isEndless = false;

    private void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = -1;
    }
    public void GoToLevelSelector()
    {
        isEndless = false;
        SceneManager.LoadScene("LevelSelector");
    }

    public void GoToAntarctica()
    {
        SceneManager.LoadScene("Antarctica");
    }

    public void GoToForest()
    {
        SceneManager.LoadScene("Forest");
    }

    public void GoToBeach()
    {
        SceneManager.LoadScene("Beach");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToEndless()
    {
        isEndless = true;
        SceneManager.LoadScene("LevelSelector");
    }
}
