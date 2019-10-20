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
    private AudioSource audioPlayer;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText;
    public Button pauseButton;

    private bool isDead = false;

    private void Awake()
    {
        Instance = this;
        scoreText.text = "Score : " + score.ToString();
        livesText.text = "Lives Remaining : 3";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();

        if (Settings.isMusicOn)
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Music"))
                {
                    audioPlayer = audio;
                }
            }

            StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));
        }

        startCutscene.Begin();
    }

    private void Update()
    {
        if (!isGameStarted && !DialogueAnimator.GetBool("isOpen") && score > 50)
        {
            SceneManager.LoadScene("Forest");
        }

        if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen")&&!isDead)
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();

            if (Settings.isMusicOn)
            {
                GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
                Music music = musicPlayer.GetComponent<Music>();
                music.changeMusic(SceneManager.GetActiveScene());
            }
        }

        if (isGameStarted)
        {
            score += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString("0");

            // refactor later
            if (score > 50)
            {
                isGameStarted = false;
                playerMotor.StopRunning();
                cameraMotor.StopFollowing();
                DialogueAnimator.SetBool("isOpen", true);
                endCutscene.Begin();
                if (Settings.isMusicOn)
                    StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));
            }
            else if (score > 40)
            {
                informationText.text = "The ice has all melted away!";
            }
            else if (score > 30)
            {
                informationText.text = "The mountains are collapsing!";
            }
            else if (score > 12)
            {
                informationText.text = "Careful! The water is toxic.";
            }
            else if (score > 8)
            {
                informationText.text = "Swipe up to jump";
            }
            else if (score > 3)
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
        deadScoreText.text = "Score : " + score.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        isGameStarted = false;
        isDead = true;
        scoreText.gameObject.SetActive(false);
        informationText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);

        if (Settings.isMusicOn)
            StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));

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
