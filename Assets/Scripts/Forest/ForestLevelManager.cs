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

		informationText.text = "Press any key to start";
		playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
		cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
		scoreText.text = "Score : " + score.ToString("0");
		seedCountText.text = "Seeds : " + seeds.ToString();
		modifierText.text = "Modifer : x" + modifier.ToString("0.0");

	}

	private void Update()
	{
		if (Input.anyKey && !isGameStarted)
		{
			isGameStarted = true;
			playerMotor.StartRunning();
			cameraMotor.StartFollowing();
			informationText.text = "";
            FindObjectOfType<SideObjectSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isFollowing = true;
		}

		if (isGameStarted)
		{
			score += (Time.deltaTime * modifier);
			scoreText.text = "Score : " + score.ToString("0");
		}

	}

	public void getSeeds()
	{
		seeds++;
		seedCountText.text = "Seeds : " + seeds.ToString();
	}

	public void updateModifer(float modifierAmount)
	{
		modifier = 1.0f + modifierAmount;
		modifierText.text = "Modifer : x" + modifier.ToString("0.0");
	}

    public void OnRetryButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
	}

    public void OnDeath()
	{
        
        deathScoreText.text = score.ToString("0");
        deathSeedText.text = seeds.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        SideObjectSpawner.Instance.IsScrolling = false;
    }
}
