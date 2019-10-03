using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {set; get;}

    private bool isGameStarted = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;

    // UI and the UI fields
    public Text scoreText;
    public Text informationText;
    private float score = 0;

    private void Awake()
    {
        Instance = this;
        scoreText.text = score.ToString();
        informationText.text = "Press any key to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
    }

    private void Update()
    {
        if (Input.anyKey && !isGameStarted)
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();
        }

        if (isGameStarted)
        {
            score += Time.deltaTime;
            scoreText.text = score.ToString("0");

            // refactor later
            if (score > 20)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (score > 15)
            {
                informationText.text = "The ice is melting!";
            }
            else if (score > 10)
            {
                informationText.text = "Use W to jump over obstacles";
            }
            else if (score > 5)
            {
                informationText.text = "Use S to slide under obstacles";
            }
            else if (score > 0)
            {
                informationText.text = "Use A and D to move";
            }

            if (score.ToString("0").Equals("15"))
            {
                cameraMotor.shakeDuration = 2f;
            }

        }
    }

}
