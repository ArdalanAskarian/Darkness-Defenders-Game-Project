using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject commonEnemy;
    public GameObject uncommonEnemy;
    public GameObject bossEnemy;
    public Camera mainCamera;

    [SerializeField] private float spawnRate = 1.0f;
    [SerializeField] private float rateDecreasePercent = 0.05f;
    [SerializeField] private float spawnTimer = 0.0f;

    //Amount of enemies that will spawn in the next raid
    [SerializeField] private float increasePercent = 0.2f;

    //Counts the number of enemies spawned, and is reset when a boss is spawned
    [SerializeField] public int enemyCounter = 0;
    
    //This variable will spawn cause a boss to spawn when the indicated number of enemies have spawned
    [SerializeField] public int bossSpawn = 20;


    [SerializeField] private float commonMinHealth = 10.0f;
    [SerializeField] private float commonMaxHealth = 15.0f;
    [SerializeField] private float uncommonMinHealth = 20.0f;
    [SerializeField] private float uncommonMaxHealth = 25.0f;
    [SerializeField] private float bossMinHealth = 40.0f;
    [SerializeField] private float bossMaxHealth = 50.0f;

    [SerializeField] private float commonMinDamage = 1.0f;
    [SerializeField] private float commonMaxDamage = 2.0f;
    [SerializeField] private float uncommonMinDamage= 2.0f;
    [SerializeField] private float uncommonMaxDamage = 3.0f;
    [SerializeField] private float bossMinDamage = 10.0f;
    [SerializeField] private float bossMaxDamage = 15.0f;
    
    //This variable is to keep track of how hard the game should be a certain time
    private float difficulty = 0.0f;

    public int completedRaids = 0;

    public bool spawning = false;
    public bool raidOnGoing = false;
    
    
    void Update(){
        if (spawning) {SpawnEnemy();}
        else if (raidOnGoing && RaidDefeated()) {RaidEnded();}
    }
    
    
    /**
     * This function spawns enemies at random positions at the edge of the camera
     * It will randomly decide whether to spawn a common or uncommon enemy and once
     * The enemy counter matches the bossSpawn variable a boss will be spawned instead
     * Enemy health and damage is randomly generated and scaled up with time and difficulty
     */
    private void SpawnEnemy(){
        if (Time.time > spawnTimer && enemyCounter < bossSpawn){
            float difficultyScaling = completedRaids*2.0f + difficulty;
            Vector3 spawnSpot = SpawnPosition();
            
            //Getting a random number, and using that to determine which enemy will spawn
            //Random.Range will exclude the highest value in the range, as it does with integers
            int enemyType = (int)Random.Range((int)0, (int)7);
            if (enemyType < 5){
                float commonHealth = Random.Range(commonMinHealth + difficultyScaling, commonMaxHealth + difficultyScaling);
                float commonDamage = Random.Range(commonMinDamage + difficultyScaling/1.5f, commonMaxDamage + difficultyScaling/1.5f);

                if (commonHealth <= 0.0f){
                    commonHealth = 1.0f;
                }
                if (commonDamage <= 0.0f){
                    commonDamage = 0.01f;
                }

                GameObject newCommon = Instantiate(commonEnemy, spawnSpot, transform.rotation);
                newCommon.GetComponent<Enemy>().SetStats(commonHealth, commonDamage);
            }
            else if (enemyType >= 5){
                float uncommonHealth = Random.Range(uncommonMinHealth + difficultyScaling, uncommonMaxHealth + difficultyScaling);
                float uncommonDamage = Random.Range(uncommonMinDamage + difficultyScaling/1.5f, uncommonMaxDamage + difficultyScaling/1.5f);

                if (uncommonHealth <= 0.0f){
                    uncommonHealth = 1.0f;
                }
                if (uncommonDamage <= 0.0f){
                    uncommonDamage = 0.01f;
                }

                GameObject newUncommon = Instantiate(uncommonEnemy, spawnSpot, transform.rotation);
                newUncommon.GetComponent<Enemy>().SetStats(uncommonHealth, uncommonDamage);
            }
            else{
                Debug.LogWarning("In function SpawnEnemy: Enemy Type needs to be between the variables indicated in the Random.Range function");
            }
            spawnTimer = Time.time + spawnRate;
            enemyCounter += 1;
        }
        else if (Time.time > spawnTimer && enemyCounter >= bossSpawn){
            float difficultyScaling = completedRaids*6.0f + difficulty;
            Vector3 spawnSpot = SpawnPosition();
            float bossHealth = Random.Range(bossMinHealth + difficultyScaling, bossMaxHealth + difficultyScaling);
            float bossDamage = Random.Range(bossMinDamage + difficultyScaling, bossMaxDamage + difficultyScaling);

            GameObject newBoss = Instantiate(bossEnemy, spawnSpot, transform.rotation);
            newBoss.GetComponent<Enemy>().SetStats(bossHealth, bossDamage);
            
            spawning = false;
            enemyCounter = 0;
            bossSpawn += completedRaids*2;
            spawnRate = spawnRate - (spawnRate*rateDecreasePercent);

            //decreasing the change in spawnrate when spawning is getting wild
            if (spawnRate < 1.0f){
                rateDecreasePercent = rateDecreasePercent/3.0f;
            }
        }
    }


    /**
     * This function creates a random position at the edge of the camera
     * returns: type: Vector3, the position that was randomly generated
     */
    private Vector3 SpawnPosition(){
        //Randomly determining whether the position will be on a vertical or horizontal edge of the screen
        //Getting a boolean as an answer but random range excludes the highest value when working with ints
        int edge = (int)Random.Range((int)0, (int)2);

        //Determing the position of the spawn on the camera
        if (edge == 0){
            int xAxis = (int) Random.Range((int)0, (int)2);
            float yAxis = Random.Range(0.0f, 1.0f);
            
            //Converting the point to a space in the scene
            return mainCamera.ViewportToWorldPoint(new Vector3((float)xAxis, yAxis, 0));
        }
        else if (edge == 1){
            int yAxis = (int) Random.Range((int)0, (int)2);
            float xAxis = Random.Range(0.0f, 1.0f);
            
            //Converting the point to a space in the scene
            return mainCamera.ViewportToWorldPoint(new Vector3(xAxis, (float)yAxis, 0));
        }
        else{
            Debug.LogWarning("In function SpawnPosition: the edge of the screen needs to be a boolean number.");
            return new Vector3(0, 0, 0);
        }
    }


    /**
     * Changes the difficulty of the enemies spawned and ensures that the
     * difficulty does not fall below zero
     */
    public void ChangeDifficulty(float change){
        difficulty += change;

        if (difficulty <= 0){
            difficulty = 0.0f;
        }
    }


    /**
     * This function returns the current difficulty of the enemies
     */
    public float CurrentDifficulty(){
        return difficulty;
    }


    public void RaidEnded(){
        completedRaids += 1;
        raidOnGoing = false;
        GameManager.instance.raidCanvas.GetComponent<RaidController>().DisplayButton();
    }


    public void StartSpawns(){
        spawning = true;
        raidOnGoing = true;
    }


    /*
    *Checking if there are still enemies alive in the game
    */
    public bool RaidDefeated(){
        GameObject[] common = GameObject.FindGameObjectsWithTag("Common Enemy");
        GameObject[] uncommon = GameObject.FindGameObjectsWithTag("Uncommon Enemy");
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss Enemy");

        if ((common.Length == 0) && (uncommon.Length == 0) && (boss.Length == 0)){
            return true;
        }
        return false;
    }
}
