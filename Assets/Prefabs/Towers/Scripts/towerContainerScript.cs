using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class towerContainerScript : MonoBehaviour
{
    //The difficulty modifiers for the enemies
    [SerializeField] private float buildDifficulty = 3.0f;
    [SerializeField] private float upgradeDifficulty = 0.5f;
    [SerializeField] private float destructionDifficulty = -10.0f;
    [SerializeField] private float damageDifficulty = -2.0f;
	public GameObject damagePopup;

    public SpriteRenderer sprite;
    public GameObject rangeCast;
    public Button interactShop;
    public Button interactUpShop;
    public GameObject tower;
    public towerScript towerScript;
    public TowerStats towerStats;
    public GameObject shop;
    public GameObject UpgradeShop;
    public GameObject healthBar;
    public GameObject notches;
    public bool towerAlive = false;
    // Start is called before the first frame update
    void Start()
    {
        interactShop.onClick.AddListener(openShop);
        interactUpShop.onClick.AddListener(openUpShop);
        interactUpShop.GameObject().SetActive(false);
        if (towerAlive){
            this.setTower(tower);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void openShop(){
        if (GameManager.instance.isMenuOpen){
            GameManager.instance.closeCurrentMenu();
        }
        shop.SetActive(true); 
        GameManager.instance.isMenuOpen = true;
        GameManager.instance.openMenu = shop;
        
    }
    void openUpShop(){
        if (GameManager.instance.isMenuOpen){
            GameManager.instance.closeCurrentMenu();
        }
        GameManager.instance.isMenuOpen = true;
        GameManager.instance.openMenu = UpgradeShop;
        UpgradeShop.SetActive(true);
        rangeCast.SetActive(true);
    }
    public void setTower(GameObject tower){
        this.tower = Instantiate(tower, this.transform);;
        this.towerScript = this.tower.GetComponent<towerScript>();
        this.towerStats = towerScript.GetTowerStats();
        this.towerStats.value -= 20;
        this.towerStats.maxHealth = towerStats.health;
        this.tower.GetComponent<towerScript>().parentContainer = this;
        sprite.color = towerScript.sprite.color;
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().SetUpgradeTree(towerScript.GetUpgradeTree());
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().UpdateTowerStats(towerStats);
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().SetTowerTitle(this.towerScript.TowerTitle);
        shop.SetActive(false);
        rangeCast.SetActive(false);
        interactShop.GameObject().SetActive(false);
        interactUpShop.GameObject().SetActive(true);
        rangeCast.transform.localScale = new Vector3(towerStats.radius, towerStats.radius, 1f);
        if (!towerAlive){
            notches.GetComponent<notches>().makeNotches(towerStats.health, towerStats.maxHealth);
            healthBar.GetComponent<healthBar>().shrinkHealthBar(towerStats.health, towerStats.maxHealth);
            notches.SetActive(true);
            healthBar.SetActive(true);
        }
        
        GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().ChangeDifficulty(buildDifficulty);
        towerAlive = true;
    }
    public void raiseAndSetStats(TowerStats statBoost){
        towerStats.health += statBoost.health;
        towerStats.maxHealth += statBoost.health;
        towerStats.damage += statBoost.damage;
        towerStats.radius += statBoost.radius;
        towerStats.value += statBoost.value;
        towerStats.fireRate -= statBoost.fireRate;
        towerStats.numbProjectiles += statBoost.numbProjectiles;
        towerStats.numbTargets += statBoost.numbTargets;
        towerScript.setTowerStats(towerStats);
        rangeCast.transform.localScale = new Vector3(towerStats.radius, towerStats.radius, 1f);
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().UpdateTowerStats(towerStats);
        notches.GetComponent<notches>().makeNotches(towerStats.health, towerStats.maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(towerStats.health, towerStats.maxHealth);

        GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().ChangeDifficulty(upgradeDifficulty);
    }
    public void sellTower(){
        GameManager.instance.AddCoins((int)towerStats.value);
        towerAlive = false;
        sprite.color = new Color(255,255,255);
        Destroy(tower);
        tower = null;
        UpgradeShop.SetActive(false);
        interactShop.GameObject().SetActive(true);
        interactUpShop.GameObject().SetActive(false);
        notches.SetActive(false);
        healthBar.SetActive(false);
    }
    public void healTower(){
        towerStats.health = towerStats.maxHealth;
        towerScript.setTowerStats(towerStats);
        towerAlive = true;
        tower.SetActive(true);
        notches.GetComponent<notches>().makeNotches(towerStats.health, towerStats.maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(towerStats.health, towerStats.maxHealth);
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().UpdateTowerStats(towerStats);
        notches.SetActive(true);
        healthBar.SetActive(true);
    }
    public void takeDamage(float damage){
        Vector3 damageSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
		GameObject DamagePop = Instantiate(damagePopup, damageSpawnPos, Quaternion.Euler(0,0,0));
		DamagePop.GetComponent<DamagePopup>().setUp((int)damage);
        string beforeDamage = this.towerStats.health.ToString();
        this.towerStats.health -= damage;
        if (towerStats.health < 0){
            towerStats.health = 0;
        }
        if (this.towerStats.health <= 0){
            tower.SetActive(false);
            towerAlive = false;
            notches.SetActive(false);
            healthBar.SetActive(false);

            GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().ChangeDifficulty(destructionDifficulty);
        }
        UpgradeShop.GetComponent<upgradeShopScriptTemp>().UpdateTowerStats(towerStats);
        notches.GetComponent<notches>().makeNotches(towerStats.health, towerStats.maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(towerStats.health, towerStats.maxHealth);

        //Every 100 points of health a tower loses lowers the difficulty according to the damageDifficulty
        string afterDamage = towerStats.health.ToString();
        if(beforeDamage[0] != afterDamage[0]){
            GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().ChangeDifficulty(damageDifficulty);
        }
    }
}
