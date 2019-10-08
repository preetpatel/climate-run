using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void GoToAntarctica()
    {
        SceneManager.LoadScene("Antarctica_StartingCutscene");
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
