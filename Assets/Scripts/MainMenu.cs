using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public bool isStory;
    public bool isEndless;

    // Trigger when mouse is clicked
    void OnMouseUp()
    {
        if (isStory)
        {
            SceneManager.LoadScene("Beach");
        }
        if (isEndless)
        {
            Debug.Log("Endless Mode");
        }
    }
}
