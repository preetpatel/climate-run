using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private static bool isEndless = false;

    public static bool getIsEndless()
    {
        return isEndless;
    }

    public static string saveName()
    {
        InputField txt_Input = GameObject.Find("InputField").GetComponent<InputField>();

        string name = txt_Input.text;
        if(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            name = "XXX";
        }
        return name;
    }

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

    public void GoToHighScore()
    {
        SceneManager.LoadScene("High Scores");
    }
}
