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
            Debug.Log("Story Mode");
        }
        if (isEndless)
        {
            Debug.Log("Endless Mode");
        }
    }
}
