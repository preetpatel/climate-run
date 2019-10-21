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

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    // UI and the UI fields
    public Text scoreText;
    public Text garbageText;
    public Slider pollutionSlide;
    public Animator LivesAnimator;

    public Image heart1;
    public Image heart2;
    public Image heart3;

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

    private void Awake()
    {
        Instance = this;
        pollutionSlide.value = TrashSpawner.garbageMultiplier;
        //informationText.text = "Press any key to start";
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
        compMotor = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionMotor>();
        scoreText.text = "Score : " + score.ToString("0");
        garbageText.text = "Garbage : " + garbage.ToString();

        isEndless = SceneController.getIsEndless();

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
        if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen")&&!isDead)
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
            FindObjectOfType<CameraMotor>().isFollowing = true;
        }

        if (isGameStarted)
        {
            score += (Time.deltaTime * modifier);
            scoreText.text = "Score : " + score.ToString("0");
            timeSinceGarbageCollected += Time.deltaTime;
            if(timeSinceGarbageCollected > 5.0f)
            {
                garbageCollected = false;
                timeSinceGarbageCollected = 0.0f;
            }

            if (!isEndless)
            {
                if (score > 60)
                {
                    isGameStarted = false;
                    playerMotor.StopRunning();
                    cameraMotor.StopFollowing();
                    endCutscene.Begin();
                    // StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));
                    score = 0;
                }
            }

        }

    }

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

    public void getGarbage()
    {
        garbage++;
        garbageText.text = "Garbage : " + garbage.ToString();
        garbageCollected = true;
        float garbMulti = TrashSpawner.garbageMultiplier;
        TrashSpawner.garbageMultiplier = Mathf.Clamp(garbMulti -= 0.25f, 0.0f, 1.0f);
        pollutionSlide.value = TrashSpawner.garbageMultiplier;
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

    // public void updatemodifer( float modifieramount)
    // {
    //     modifier = 1.0f + modifieramount;
    //     modifiertext.text = "modifer : x" + modifier.tostring("0.0");
    // }

    public void OnDeath()
    {
        deadScoreText.text = "Score: " + score.ToString("0");
        deadGarbageText.text = "Garbage Collected: " + garbage.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        isGameStarted = false;
        isDead = true;
        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
        if (Settings.isMusicOn)
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
}
