using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AntarcticaLevelManager : MonoBehaviour
{
    public static AntarcticaLevelManager Instance { set; get; }

    private bool isGameStarted = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;
    private CompanionMotor compMotor;
    private const string HIGHSCOREKEY = "AntarcticaHighScore";

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text informationText;
    public Text HighScoreText;
    public GameObject newHighScore;
    private float score = 0;
    private AudioSource musicPlayer;
    private GameObject audioPlayer;

    public Animator HighScoreAnimator;
    public Animator LivesAnimator;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText;
    public Button pauseButton;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    private int roundedScore;
    private bool isGameOver = false;

    // Check if in endless mode
    private bool isEndless;

    // ends the level when this score is reached
    private float scoreOnFinish = 50.0f;

    private void Awake()
    {
        Instance = this;
        scoreText.text = score.ToString();
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        compMotor = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionMotor>();

        isEndless = SceneController.getIsEndless();

        if (Settings.isMusicOn.Value)
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

        if (!isEndless)
        {
            startCutscene.Begin();
        }
        else
        {
            informationText.text = "Touch to start";
        }
    }

    private void Update()
    {
        if (!isGameStarted && !DialogueAnimator.GetBool("isOpen") && score > scoreOnFinish && !isEndless)
        {
            SceneManager.LoadScene("Beach");
        }

        if (!isGameOver)
        {
            if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
            {
                isGameStarted = true;
                playerMotor.StartRunning();
                cameraMotor.StartFollowing();
                compMotor.StartRunning();

                if (Settings.isMusicOn.Value)
                {
                    audioPlayer = GameObject.FindGameObjectWithTag("SoundController");
                    Music music = audioPlayer.GetComponent<Music>();
                    music.changeMusic(SceneManager.GetActiveScene());
                }
            }
        }



        if (isGameStarted)
        {
            score += Time.deltaTime;
            scoreText.text = score.ToString("0");

            // refactor later
            if (!isEndless)
            {

                if (score > scoreOnFinish)
                {
                    isGameStarted = false;
                    playerMotor.StopRunning();
                    cameraMotor.StopFollowing();
                    compMotor.StopRunning();
                    DialogueAnimator.SetBool("isOpen", true);
                    endCutscene.Begin();
                    if (Settings.isMusicOn.Value)
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
        isGameOver = true;

        if (Settings.isMusicOn.Value)
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));

        roundedScore = (int)Mathf.Round(score);
        bool isNewHighScore = SaveState.saveHighScore(roundedScore, HIGHSCOREKEY);

        if (isEndless)
        {
            if (isNewHighScore)
            {
                newHighScore.SetActive(true);
                HighScoreAnimator.SetTrigger("IsHighScore");
            }

            HighScoreText.text = "HighScore : " + PlayerPrefs.GetInt(HIGHSCOREKEY);
        }
        else
        {
            HighScoreText.gameObject.SetActive(false);
        }



        GameObject panel = GameObject.FindGameObjectWithTag("AlivePanel");

        if (panel != null)
        {
            panel.SetActive(false);
        }
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

    public void saveHighScore()
    {
        string name = SceneController.saveName();
        Debug.Log(name);
        HighscoreTable.AddHighscoreEntry(roundedScore, name, "antarctica");
        GameObject.FindGameObjectWithTag("HighScore").SetActive(false);
    }

    public void SkipLevel()
    {
        score = scoreOnFinish + 1;
    }
}