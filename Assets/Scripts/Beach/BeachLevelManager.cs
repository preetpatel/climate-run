using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeachLevelManager : MonoBehaviour
{

    public static BeachLevelManager Instance { set; get; }
    private bool isGameStarted = false;
    private bool startedShaking = false;
    private PlayerMotor playerMotor;
    private CameraMotor cameraMotor;
    private CompanionMotor compMotor;
    private bool garbageCollected = false;
    private float timeSinceGarbageCollected = 0.0f;
    private const string HIGHSCOREKEY = "BeachHighScore";
    private bool isGameOver = false;
    public GameObject newHighScore;

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text garbageText;
    public Text HighScoreText;
    public Slider pollutionSlide;
    public Animator LivesAnimator;
    public Animator HighScoreAnimator;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    private int roundedScore;

    private float score = 0;
    private float garbage = 0;
    private float modifier = 1.0f;
    private AudioSource musicPlayer;
    private GameObject audioPlayer;

    private bool isDead = false;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadGarbageText;

    // Check if in endless mode
    private bool isEndless;

    /* This method is run before the first frame update, it intialises all necessary variables */
    private void Awake()
    {
        Instance = this;
        pollutionSlide.value = TrashSpawner.garbageMultiplier;
        //informationText.text = "Press any key to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        compMotor = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionMotor>();
        scoreText.text = score.ToString("0");
        garbageText.text = garbage.ToString();

        isEndless = SceneController.getIsEndless();

        // Initialise music
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

        // If its endless you skip all cutscenes
        if (!isEndless)
        {
            startCutscene.Begin();
        }

    }

    /* This method is run every frame */
    private void Update()
    {
        // Goes to the next level in the story if the user has met the conditions
        if (!isGameStarted && !DialogueAnimator.GetBool("isOpen") && score > 60 && !isEndless)
        {
            SceneManager.LoadScene("Forest");
        }

        // Starts the game after the user has run through the initial text.
        if (!isGameOver)
        {
            if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen") && !isDead)
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
                FindObjectOfType<CameraMotor>().isFollowing = true;
            }
        }

        // Updates score and other variables if the game is running
        if (isGameStarted)
        {
            score += (Time.deltaTime * modifier);
            scoreText.text = score.ToString("0");
            timeSinceGarbageCollected += Time.deltaTime;
            if(timeSinceGarbageCollected > 3.5f)
            {
                garbageCollected = false;
                timeSinceGarbageCollected = 0.0f;
            }

            // Ends the game when the user has reached the end.
            if (!isEndless)
            {
                if (score > 60)
                {
                    isGameStarted = false;
                    playerMotor.StopRunning();
                    cameraMotor.StopFollowing();
                    DialogueAnimator.SetBool("isOpen", true);
                    endCutscene.Begin();
                    // StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));
                }
            }

        }

    }

    /* Updates at fixed intervals, used to ensure the garbage multiplier increases at a constant rate */
    private void FixedUpdate() 
    {
        if (isGameStarted)
        {
            if (!garbageCollected)
            {
                float garbMulti = TrashSpawner.garbageMultiplier;
                TrashSpawner.garbageMultiplier = Mathf.Clamp(garbMulti += 0.0005f, 0.0f, 1.0f);
                pollutionSlide.value = TrashSpawner.garbageMultiplier;
            }
        }
    }

    /* Called when a piece of garbage is collected, updates the pollution level and other variables */
    public void getGarbage()
    {
        garbage++;
        garbageText.text = garbage.ToString();
        garbageCollected = true;
        float garbMulti = TrashSpawner.garbageMultiplier;
        TrashSpawner.garbageMultiplier = Mathf.Clamp(garbMulti -= 0.2f, 0.0f, 1.0f);
        pollutionSlide.value = TrashSpawner.garbageMultiplier;
    }
    
    /* Called when the user hits an obstacle, updates the lives UI indicators. */
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

    // public void updatemodifer( float modifieramount)
    // {
    //     modifier = 1.0f + modifieramount;
    //     modifiertext.text = "modifer : x" + modifier.tostring("0.0");
    // }

    /* Called when the user dies, brings up the UI and stops the game */
    public void OnDeath()
    {
        deadScoreText.text = "Score: " + score.ToString("0");
        deadGarbageText.text = "Garbage Collected: " + garbage.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        isGameStarted = false;
        isGameOver = true;
        isDead = true;

        // Save the High Score
        roundedScore = (int)Mathf.Round(score);
        bool isNewHighScore = SaveState.saveHighScore(roundedScore, HIGHSCOREKEY);

        // If its endless allow the user to save their score
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

        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
        if (Settings.isMusicOn.Value)
            StartCoroutine(AudioController.FadeOut(musicPlayer, 0.5f));
    }

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Beach");
	}

    public void OnExitButtonPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Saves the Users high score
    public void saveHighScore()
    {
        string name = SceneController.saveName();
        HighscoreTable.AddHighscoreEntry(roundedScore, name, "beach");
        GameObject.FindGameObjectWithTag("HighScore").SetActive(false);
    }
}
