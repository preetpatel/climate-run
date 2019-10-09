using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeachLevelManager : MonoBehaviour
{

    public static BeachLevelManager Instance { set; get; }

    private bool isGameStarted = false;
    private bool startedShaking = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;

    // UI and the UI fields
    public Text scoreText;
    public Text garbageText;
    public Text informationText;
    public Text livesText;
    private float score = 0;
    private float garbage = 0;
    private float modifier = 1.0f;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadGarbageText;

    private void Awake()
    {
        Instance = this;

        informationText.text = "Press any key to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        scoreText.text = "Score : " + score.ToString("0");
        garbageText.text = "Garbage : " + garbage.ToString();
        //livesText.text = "Lives Remaining : 3";
        //modifierText.text = "Modifer : x" + modifier.ToString("0.0");

    }

    private void Update()
    {
        if (Input.anyKey && !isGameStarted)
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();
            informationText.text = "";
        }

        if (isGameStarted)
        {
            score += (Time.deltaTime * modifier);
            scoreText.text = "Score : " + score.ToString("0");
        }

    }

    public void getGarbage()
    {
        garbage++;
        garbageText.text = "Garbage : " + garbage.ToString();
    }
    
    public void updateLives(float livesAmount)
    {
        livesText.text = "Lives Remaining : " + livesAmount.ToString("0");
    }

    // public void updateModifer( float modifierAmount)
    // {
    //     modifier = 1.0f + modifierAmount;
    //     modifierText.text = "Modifer : x" + modifier.ToString("0.0");
    // }

    public void OnDeath()
    {
        deadScoreText.text = "Score: " + score.ToString("0");
        deadGarbageText.text = "Garbage Collected: " + garbage.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        //completely pause the game
        //Time.timeScale = 0;
    }

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Beach");
	}

    public void OnExitButtonPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
