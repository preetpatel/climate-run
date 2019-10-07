using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerBeach : MonoBehaviour
{

    public static LevelManagerBeach Instance { set; get; }

    public static bool IsDead { set; get; }

    private bool isGameStarted = false;
    private bool startedShaking = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;

    // UI and the UI fields
    public Text scoreText;
    public Text garbageText;
    public Text informationText;
    public Text modifierText;
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
            FindObjectOfType<BeachPalmTreeSpawner>().IsScrolling = true;
        }

        if (isGameStarted&&!IsDead)
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

    public void updateModifer( float modifierAmount)
    {
        modifier = 1.0f + modifierAmount;
        modifierText.text = "Modifer : x" + modifier.ToString("0.0");
    }

    public void OnDeath()
    {
        IsDead = true;
        deadScoreText.text = score.ToString("0");
        deadGarbageText.text = garbage.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        //completely pause the game
        //Time.timeScale = 0;
    }
}
