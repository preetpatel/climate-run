using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This class manages the forest level
public class ForestLevelManager : MonoBehaviour
{
	public static ForestLevelManager Instance { set; get; }

	private bool isGameStarted = false;
	private PlayerMotor playerMotor;
	private CameraMotor cameraMotor;

	public Animator deathMenuAnim;
    public Text deathScoreText, deathSeedText;

	// UI and the UI fields
	public Text scoreText;
	public Text seedCountText;
	public Text informationText;
	public Text modifierText;
	private float score = 0;
	private float seeds = 0;
	private float modifier = 1.0f;

	private void Awake()
	{
		Instance = this;
		// Sets text and motor fields
		informationText.text = "Tap Anywhere To Begin \n\n Remember to collect as many of those young green plants along the way!";
		playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
		cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
		scoreText.text = "Score : " + score.ToString("0");
		seedCountText.text = "Seeds : " + seeds.ToString();
		modifierText.text = "Modifer : x" + modifier.ToString("0.0");

	}

	// Called once per frame
	private void Update()
	{
		// Checks if the game is going to start
		if (Input.anyKey && !isGameStarted)
		{
			// Starts the game, begins moving player and camera
			isGameStarted = true;
			playerMotor.StartRunning();
			cameraMotor.StartFollowing();
			informationText.text = "";
            FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isFollowing = true;
		}

		// Updates score if game has already started
		if (isGameStarted)
		{
			score += (Time.deltaTime * modifier);
			scoreText.text = "Score : " + score.ToString("0");
		}

	}

	// Called when the user picks up a seed, updates the score
	public void getSeeds()
	{
		seeds++;
		seedCountText.text = "Seeds : " + seeds.ToString();
	}

	// Updates the modifier
	public void updateModifer(float modifierAmount)
	{
		modifier = 1.0f + modifierAmount;
		modifierText.text = "Modifer : x" + modifier.ToString("0.0");
	}

	// Retries the level
    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
	}

	// Plays the death animation and displays final score
    public void OnDeath()
	{
        
        deathScoreText.text = "Score: " + score.ToString("0");
        deathSeedText.text = "Seeds Collected: " + seeds.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        SideObjectSpawner.Instance.IsScrolling = false;
    }

	// Exits the level by playing the ending cutscene.
    public void OnExitButtonPress()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("Forest_EndingCutscene");
    }
}
