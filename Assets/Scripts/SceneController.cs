using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void GoToLevelSelector()
    {
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
}
