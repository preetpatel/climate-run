using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ForestLevelManager : MonoBehaviour
{
    public static ForestLevelManager Instance { set; get; }

    private bool isGameStarted = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;
    private const string HIGHSCOREKEY = "ForestHighScore";
    private GorillaMotor compMotor;

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    public Animator deathMenuAnim;
    public Text deathScoreText, deathSeedText;

    public Animator lifeAnimation;
    public Animator HighScoreAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text seedCountText;
    public Text informationText;
    public Text HighScoreText;
    public GameObject newHighScore;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    private int roundedScore;

    private float score = 0;
    private float seeds = 0;
    private float modifier = 1.0f;
    private AudioSource musicPlayer;
    private GameObject audioPlayer;

    private bool isGameOver = false;

    // Check if in endless mode
    private bool isEndless;
    private bool done = false;

    private void Awake()
    {
        Instance = this;

        informationText.text = "Tap Anywhere To Begin!";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        compMotor = GameObject.FindGameObjectWithTag("Companion").GetComponent<GorillaMotor>();
        scoreText.text = score.ToString("0");
		seedCountText.text = seeds.ToString();

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

        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);

        if (!isEndless)
        {
            startCutscene.Begin();
        }
    }

	private void Update()
	{
        if (!isGameStarted && !DialogueAnimator.GetBool("isOpen") && done && !isEndless)
        {
            SceneManager.LoadScene("Congrats");
        }

        if (!isGameOver)
        {
            if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
            {
                isGameStarted = true;
                playerMotor.StartRunning();
                cameraMotor.StartFollowing();
                compMotor.StartRunning();
                informationText.text = "";
                FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
                FindObjectOfType<CameraMotor>().isFollowing = true;
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
            score += (Time.deltaTime * modifier);
            scoreText.text = "Score : " + score.ToString("0");
        }
    }

    public void StopGameIfNotEndless(GameObject endSegment)
    {
        if (!isEndless)
        {
            DialogueAnimator.SetBool("isOpen", true);

            Transform gorillaPosition = endSegment.transform.Find("GorillaPosition");
            compMotor.DoEndSequence(gorillaPosition);

            isGameStarted = false;
            cameraMotor.MoveToCutscenePos(endSegment.transform.Find("CameraPosition"));
            endCutscene.Begin();
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
        }
    }

	public void getSeeds()
	{
		seeds++;
		seedCountText.text = seeds.ToString();
	}

	public IEnumerator updateLives(float livesAmount)
	{
        lifeAnimation.SetTrigger("LifeLost");
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
        lifeAnimation.SetTrigger("GoBack");
	}

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
	}

    public void OnDeath()
    { 
        isGameStarted = false;
        isGameOver = true;
        deathScoreText.text = "Score: " + score.ToString("0");
        deathSeedText.text = "Seeds Collected: " + seeds.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        SideObjectSpawner.Instance.IsScrolling = false;
        GameObject panel = GameObject.FindGameObjectWithTag("AlivePanel");
        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (Settings.isMusicOn.Value)
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));

        // Save the High Score
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

    }

    public void OnExitButtonPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void saveHighScore()
    {
        string name = SceneController.saveName();
        Debug.Log(name);
        HighscoreTable.AddHighscoreEntry(roundedScore, name, "forest");
        GameObject.FindGameObjectWithTag("HighScore").SetActive(false);
    }
}