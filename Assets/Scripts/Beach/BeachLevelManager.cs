using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Text informationText;
    public Text livesText;
    public Slider pollutionSlide;
    public Image infoBox;
    private float score = 0;
    private float garbage = 0;
    private float modifier = 1.0f;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadGarbageText;

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
        livesText.text = "Lives Remaining : 3";

        //modifierText.text = "Modifer : x" + modifier.ToString("0.0");

        startCutscene.Begin();

    }

    private void Update()
    {
        if (Input.anyKey && !isGameStarted && !DialogueAnimator.GetBool("isOpen"))
        {
            isGameStarted = true;
            playerMotor.StartRunning();
            cameraMotor.StartFollowing();
            compMotor.StartRunning();
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
    
    public void updateLives(float livesAmount)
    {
        livesText.text = "Lives Remaining : " + livesAmount.ToString("0");
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
        GameObject.FindGameObjectWithTag("AlivePanel").SetActive(false);
           
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
