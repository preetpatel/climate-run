using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AntarcticaGameManager : MonoBehaviour
{
    public static AntarcticaGameManager Instance {set; get;}

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
        Instance = this;
        scoreText.text = score.ToString();
        informationText.text = "Touch to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
    }

    // Gets called every frame
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
            // Keeps score
            score += Time.deltaTime;
            scoreText.text = score.ToString("0");

            // Gives commands or moves to next cutscene
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

            if (score.ToString("0").Equals("15"))
            {
                cameraMotor.shakeDuration = 2f;
            }

        }
    }

}
