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

    // Cutscenes
    public DialogueTrigger startCutscene;
    public DialogueTrigger endCutscene;
    public Animator DialogueAnimator;

    public Animator deathMenuAnim;
    public Text deathScoreText, deathSeedText;

	// UI and the UI fields
	public Text scoreText;
	public Text seedCountText;
	public Text informationText;
	public Text livesText;
	private float score = 0;
	private float seeds = 0;
	private float modifier = 1.0f;
    private AudioSource audioPlayer;


    private void Awake()
	{
        Instance = this;

		informationText.text = "Tap Anywhere To Begin!";
		playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
		cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
		scoreText.text = "Score : " + score.ToString("0");
		seedCountText.text = "Seeds : " + seeds.ToString();
		livesText.text = "Lives Remaining : 3";

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.CompareTag("Music"))
            {
                audioPlayer = audio;
            }
        }

        StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));

        startCutscene.Begin();
	}

	private void Update()
	{
		if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
		{
			isGameStarted = true;
			playerMotor.StartRunning();
			cameraMotor.StartFollowing();
			informationText.text = "";
            FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isFollowing = true;
            GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
            Music music = musicPlayer.GetComponent<Music>();
            music.changeMusic(SceneManager.GetActiveScene());

        }

		if (isGameStarted)
		{
			score += (Time.deltaTime * modifier);
			scoreText.text = "Score : " + score.ToString("0");

            if (score > 60)
            {
                isGameStarted = false;
                playerMotor.StopRunning();
                cameraMotor.StopFollowing();
                endCutscene.Begin();
                StartCoroutine(AudioController.FadeOut(audioPlayer, 0.5f));
                score = 0;
            }
        }

	}

	public void getSeeds()
	{
		seeds++;
		seedCountText.text = "Seeds : " + seeds.ToString();
	}

	public void updateLives(float livesAmount)
	{
		livesText.text = "Lives Remaining : " + livesAmount.ToString("0");
	}

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
	}

    public void OnDeath()
	{
        isGameStarted = false;   
        deathScoreText.text = "Score: " + score.ToString("0");
        deathSeedText.text = "Seeds Collected: " + seeds.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        SideObjectSpawner.Instance.IsScrolling = false;
        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
    }

    public void OnExitButtonPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
