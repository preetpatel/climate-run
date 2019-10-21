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
    private CompanionMotor compMotor;

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text informationText;
    private float score = 0;
    private AudioSource musicPlayer;
    private GameObject audioPlayer;

    public Animator LivesAnimator;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText;
    public Button pauseButton;

    public Image heart1;
    public Image heart2;
    public Image heart3;
    
    // Check if in endless mode
    private bool isEndless;    

    private void Awake()
    {
        Instance = this;
        scoreText.text = "Score : " + score.ToString();
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        isEndless = playerMotor.isEndless;
        compMotor = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionMotor>();

        if (Settings.isMusicOn)
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Music"))
                {
                    musicPlayer = audio;
                }
            }

            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
        }

        startCutscene.Begin();
    }

    private void Update()
    {
        if (!isGameStarted && !DialogueAnimator.GetBool("isOpen") && score > 50)
        {
            SceneManager.LoadScene("Forest");
        }

        if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();
            compMotor.StartRunning();

            if (Settings.isMusicOn)
            {
                audioPlayer = GameObject.FindGameObjectWithTag("SoundController");
                Music music = audioPlayer.GetComponent<Music>();
                music.changeMusic(SceneManager.GetActiveScene());
            }
        }


        if (isGameStarted)
        {
            score += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString("0");

            // refactor later
            if (!isEndless)
            {
                if (score > 50)
                {
                    isGameStarted = false;
                    playerMotor.StopRunning();
                    cameraMotor.StopFollowing();
                    DialogueAnimator.SetBool("isOpen", true);
                    endCutscene.Begin();
                    if (Settings.isMusicOn)
                        StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
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
            else
            {
                informationText.text = null;
            }
        }
    }

    public void OnDeath()
    {
        deadScoreText.text = "Score : " + score.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        isGameStarted = false;

        if (Settings.isMusicOn)
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));

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

    public IEnumerator updateLives(float livesAmount)
    {
        LivesAnimator.SetTrigger("LifeLost");
        switch (livesAmount)
        {
            case 2f:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1f:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 0f:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
        }

        yield return new WaitForSeconds(1f);
        LivesAnimator.SetTrigger("GoBack");
    }
}
