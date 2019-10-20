using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AntarcticaLevelManager : MonoBehaviour
{
    public static AntarcticaLevelManager Instance {set; get;}

    private bool isGameStarted = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text informationText;
    public Text livesText;
    private float score = 0;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText;
    public Button pauseButton;

    private void Awake()
    {
        Instance = this;
        scoreText.text = score.ToString();
        informationText.text = "Touch to start";
        livesText.text = "Lives Remaining : 3";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();

        startCutscene.Begin();
    }

    private void Update()
    {

        if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();    
        }

        if (isGameStarted)
        {
            score += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString("0");

            // refactor later
            if (score > 60)
            {
                isGameStarted = false;
                playerMotor.StopRunning();
                cameraMotor.StopFollowing();
                endCutscene.Begin();

                score = 0;
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

        }
    }

    public void OnDeath()
    {
        deadScoreText.text = "Score: " + score.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        isGameStarted = false;
        scoreText.gameObject.SetActive(false);
        informationText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);

        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Antarctica");
    }

    public void OnExitButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void updateLives(float livesAmount)
    {
        livesText.text = "Lives Remaining : " + livesAmount.ToString("0");
    }
}
