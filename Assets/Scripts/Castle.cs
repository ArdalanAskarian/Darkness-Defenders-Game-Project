using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Castle : MonoBehaviour
{
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100f;
    public GameObject deathEffect;
    public GameObject healthBar;
    public GameObject notches;
    public Collider playerCollider;

    public Sprite[] CastleImages;
	public GameObject damagePopup;

    public SpriteRenderer Image1;

    public GameObject restartMenu;
    public FadeController fade;
    public GameObject shop;
    public Button openShop;
    public SpriteRenderer castleGround;

    public bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        notches.GetComponent<notches>().makeNotches(health, maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(health, maxHealth);
        openShop.onClick.AddListener(openDragonShop);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        Vector3 damageSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
		GameObject DamagePop = Instantiate(damagePopup, damageSpawnPos, Quaternion.Euler(0,0,0));
		DamagePop.GetComponent<DamagePopup>().setUp((int)damage);

        if (health/maxHealth <= 0.1)
        {
            Image1.sprite = CastleImages[0];
        }
        else if (health / maxHealth <= 0.2)
        {
            Image1.sprite = CastleImages[1];
        }
        else if (health / maxHealth <= 0.3)
        {
            Image1.sprite = CastleImages[2];
        }
        else if (health / maxHealth <= 0.4)
        {
            Image1.sprite = CastleImages[3];
        }
        else if (health / maxHealth <= 0.5)
        {
            Image1.sprite = CastleImages[4];
        }
        else if (health / maxHealth <= 0.6)
        {
            Image1.sprite = CastleImages[5];
        }
        else if (health / maxHealth <= 0.7)
        {
            Image1.sprite = CastleImages[6];
        }
        else if (health / maxHealth <= 0.8)
        {
            Image1.sprite = CastleImages[7];
        }
        else if (health / maxHealth <= 0.9)
        {
            Image1.sprite = CastleImages[8];
        }
        else
        {
            Image1.sprite = CastleImages[9];
        }

        if (health <= 0)
        {
            isAlive = false;
           if(deathEffect != null){
                GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
                Destroy(effect, 1.0f);
            }
           
            restartMenu.SetActive(true);
            fade.StartFade();
             Destroy(this.gameObject);
        }
        else
        {
            notches.GetComponent<notches>().makeNotches(health, maxHealth);
            healthBar.GetComponent<healthBar>().shrinkHealthBar(health, maxHealth);
           

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameManager.instance.playerHidden = true;
            Image1.color = Color.red;
            castleGround.color= Color.red;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameManager.instance.playerHidden = false;
            Image1.color = Color.white;
            castleGround.color = Color.white;
        }
    }
    void openDragonShop(){
        if (GameManager.instance.isMenuOpen){
            GameManager.instance.closeCurrentMenu();
        }
        shop.SetActive(true); 
        GameManager.instance.isMenuOpen = true;
        GameManager.instance.openMenu = shop;
    }
   
}
