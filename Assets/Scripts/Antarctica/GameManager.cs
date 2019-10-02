using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {set; get;}

    private bool isGameStarted = false;
    private bool startedShaking = false;
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

            if (score > 10)
            {
                informationText.text = "The ice is melting!";

                if (!startedShaking)
                {
                    cameraMotor.shakeDuration = 3f;
                    startedShaking = true;
                }
            } else
            {
                informationText.text = "";
            }

        }

    }

}
