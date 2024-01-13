using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject player;
    public GameObject castle;
    public GameObject pauseMenu;
    public GameObject openMenu;
    public GameObject raidCanvas;
    public GameObject EnemySpawner;
    public GameObject startMenu;
    public GameObject introScreen;

    public Text coinsText;
    public Text timerText;
    public Text spawnsText;

    public float gold;
    public float elapsedTimeInSeconds;
    public int bones = 0;
    public bool playerHidden = false;
    public bool isMenuOpen = false;
    public bool playerDead=false;

    //responsible for respawn of player 
    public GameObject RespawnCanvas;
    public Text respawnText; 
    public float respawnTime = 10f;
    private bool respawnTimerActive = false;
    public bool dragonAlive = false;
    public Text bonesText;

    public void closeCurrentMenu(){
        openMenu.GetComponent<shopScript>().CloseShop();
    }


    void Awake(){
        if (instance == null){
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SetCoinText();
        SetBoneText();
        RespawnCanvas.SetActive(false);
    }


    void Start()
    {
        SetCoinText();
    }


    void SetCoinText()
    {
        coinsText.text = "Gold of the Innocent: " + gold.ToString();
    }
    void SetBoneText()
    {
        bonesText.text = "Bones of the Forsaken: " + bones.ToString();
    }

    public void AddCoins(int coinToAdd)
    {
        gold += coinToAdd;
        SetCoinText();
    }

    public void AddBones(int bonesToAdd){
        bones += bonesToAdd;
        SetBoneText();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }


        if (respawnTimerActive)
        {
            if (respawnTime > 0 && EnemySpawner.GetComponent<EnemySpawner>().raidOnGoing)
            {
                respawnTime -= Time.deltaTime;
                int respawnTimeLeft = Mathf.FloorToInt(respawnTime);
                respawnText.text = "Respawn in: " + respawnTimeLeft.ToString();
            }
            else
            {
                respawnTime = 0;
                respawnTimerActive = false;
                respawnPlayer();
            }
        }

        // Update the timer
        //if (EnemySpawner.active) {elapsedTimeInSeconds += Time.deltaTime;}
        //int minutes = Mathf.FloorToInt(elapsedTimeInSeconds / 60);
        //int seconds = Mathf.FloorToInt(elapsedTimeInSeconds % 60);
        //timerText.text = "Time spent being evil: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = "Waves of Goodness Survived: " + EnemySpawner.GetComponent<EnemySpawner>().completedRaids;

        //Making sure that this does not update enemy numbers until after the boss is defeated (don't try to comprehend my though process, I can't even do that lol)
        if ((EnemySpawner.GetComponent<EnemySpawner>().spawning && EnemySpawner.GetComponent<EnemySpawner>().raidOnGoing) || (!EnemySpawner.GetComponent<EnemySpawner>().raidOnGoing)){
            spawnsText.text = "Enemies until Boss Spawns: " + (EnemySpawner.GetComponent<EnemySpawner>().bossSpawn - EnemySpawner.GetComponent<EnemySpawner>().enemyCounter);
        }
    }


    public void RestartGame()
    {
        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void playerDied(){
        RespawnCanvas.SetActive(true);
        respawnTime = 10f;
        respawnText.text = respawnTime.ToString();
        respawnTimerActive= true; 
        playerDead=true;
    }
    public void respawnPlayer(){
        respawnTimerActive=false;
        RespawnCanvas.SetActive(false);
        playerDead=false;
    }

}

