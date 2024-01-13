using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] private float commonPercent = 0.25f;
    [SerializeField] private float uncommonPercent = 0.20f;
    [SerializeField] private float bossPercent = 0.10f;
    public GameObject upgradeShop;
    public float dragonSpeed = 1.0f;
    public float dragonRadius = 5.0f;
    private float angle;
    public int cost = 20;
    public float costTime;
    private float costInterval = 20.0f;
    public GameObject enemySpawner;
    public GameObject smokeEffect;
    Animator animator;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        GameObject effect = Instantiate(smokeEffect, transform.position, transform.rotation);
        Destroy(effect, 1.0f);

        animator.SetTrigger("fly");

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * dragonSpeed;
        transform.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * dragonRadius; 
        transform.rotation = Quaternion.Euler(0,0,angle * Mathf.Rad2Deg);


        if (!enemySpawner.GetComponent<EnemySpawner>().raidOnGoing){
            costTime = Time.time + costInterval;
        }

        if (costTime < Time.time){
            if (GameManager.instance.gold >= cost){
                GameManager.instance.AddCoins(-cost);
                costTime = Time.time + costInterval;
                audio.Play();
                Debug.Log("Dragon ate coins.");
            }
            else{
                audio.Play();
                smokeEffect.transform.localScale += new Vector3(1, 1, 1);
                GameObject effect = Instantiate(smokeEffect, transform.position, transform.rotation);
                Destroy(effect, 1.0f);
                smokeEffect.transform.localScale -= new Vector3(1, 1, 1);
                GameManager.instance.dragonAlive = false;
                upgradeShop.GetComponent<DragonUpgradeScript>().resetText();
                this.gameObject.SetActive(false);
            }
            
        }
    }

    public void DismissDragonEffect(){
        smokeEffect.transform.localScale += new Vector3(1, 1, 1);
        GameObject effect = Instantiate(smokeEffect, transform.position, transform.rotation);
        Destroy(effect, 1.0f);
        smokeEffect.transform.localScale -= new Vector3(1, 1, 1);
        audio.Play();
    }

    public void resetTimer(){
        costTime = Time.time + costInterval;
    }
    void OnTriggerEnter(Collider other)
{
   if (other.CompareTag("Common Enemy"))
    {
        float damage = other.GetComponent<Enemy>().maxHealth *commonPercent;
        other.GetComponent<Enemy>().TakeDamage(damage);
    }
    else if (other.CompareTag("Uncommon Enemy")){
        float damage = other.GetComponent<Enemy>().maxHealth *uncommonPercent;
        other.GetComponent<Enemy>().TakeDamage(damage);
    }
    else if (other.CompareTag("Boss Enemy")){
        float damage = other.GetComponent<Enemy>().maxHealth *bossPercent;
        other.GetComponent<Enemy>().TakeDamage(damage);
    }
}

}
