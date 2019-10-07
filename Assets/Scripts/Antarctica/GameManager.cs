using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the different mechanics in the game
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
    
    // Sets what happens when game starts
    private void Awake()
    {
        // Sets initial text and motoes
        Instance = this;
        scoreText.text = score.ToString();
        informationText.text = "Touch to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
    }

    // Method called on every frame
    private void Update()
    {
        // Start camera and player
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

            // // Gives commands in floating text or moves to next cut scene
            if (score > 60)
            {
                SceneManager.LoadScene("Antarctica_EndingCutscene");
            }
            else if (score > 15)
            {
                informationText.text = "The ice is melting!";
            }
            else if (score > 10)
            {
                informationText.text = "Swipe up to jump";
            }
            else if (score > 5)
            {
                informationText.text = "Swipe down to slide";
            }
            else if (score > 0)
            {
                informationText.text = "Swipe to move";
            }

            // Shakes game to represent melting
            if (score.ToString("0").Equals("15"))
            {
                cameraMotor.shakeDuration = 2f;
            }

        }
    }

}
