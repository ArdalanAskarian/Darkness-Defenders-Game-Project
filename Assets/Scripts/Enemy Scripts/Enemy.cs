using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class CoinDropParameters
{
    public GameObject coinPrefab;
    public int minCoins;
    public int maxCoins;
    public float dropRadius;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] public float health = 0.0f;
	[SerializeField] public float maxHealth = 0.0f;
    [SerializeField] public float damage = 0.0f;
    [SerializeField] private float damageRate = 5.0f;
	[SerializeField] private float damageTime = 0.0f;
	[SerializeField] private float dropPercent = 100.0f;
	[SerializeField] private float knockBackDist = 2.0f;
	[SerializeField] private float knockBackRate = 20.0f;
	[SerializeField] private float knockBackTime = 0.0f;
	public GameObject damagePopup;

    public GameObject drop;
	public GameObject healthBar;
    public GameObject notches;
	public Animator enemyAnimator;
	public NavMeshAgent navAgent;
	public AudioSource attack1;
	public AudioSource attack2;
	public AudioSource attack3;
	public AudioSource attack4;

	//Determines the current target
	private GameObject target;

	//Acts as a boolean
	private bool isMoving = true;
	private bool pushedBack = false;
	public bool isAlive = true;
	public bool slowed = false;

	private Vector3 lastPosition;
	private float slowedTime = 0;

    public CoinDropParameters[] coinDropOptions;



    void Start(){
		notches.GetComponent<notches>().healthPerNotch = maxHealth / 5.0f;
		notches.GetComponent<notches>().makeNotches(health, maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(health, maxHealth);

		enemyAnimator = GetComponentInChildren<Animator>();
		target = ClosestTower();

		lastPosition = transform.position;
		navAgent.updateRotation = false;
		navAgent.updateUpAxis = false;

		attack1.playOnAwake = false;
		attack2.playOnAwake = false;
		attack3.playOnAwake = false;
		attack4.playOnAwake = false;
	}


	/**
    * Sets the stats of the enemy when spawning
    */
    public void SetStats(float startHealth, float startDamage){
        health = startHealth;
        damage = startDamage;
		maxHealth = startHealth;
    }


    // Determines whether the enemy is attacking or moving
    void Update(){
		if (!GameManager.instance.castle.GetComponent<Castle>().isAlive){
			Destroy(this.gameObject);
		}

		if ((slowedTime <= Time.time) && slowed){
			this.navAgent.speed = 3.0f;
			slowed = false;
		}

		//Switching the enemy target if the player is in the castle
		if (target.transform.tag == "Player" && GameManager.instance.playerHidden){
			target = ClosestTower();
			isMoving = true;
		}

		if (pushedBack && (knockBackTime < Time.time) && isAlive){
			pushedBack = false;
			isMoving = true;
		}
		else if (isMoving && isAlive && !pushedBack){
        	Movement();
		}
		else if (!isMoving && (damageTime < Time.time) && isAlive && !pushedBack){
			Attack();
		}
    }
    

    /**
     *Moves the enemy towards the closest tower unless it is in the player's sphere collider
     */
    public void Movement(){
		Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
		Vector3 targetPos = new Vector3(closestPoint.x, closestPoint.y, -1);
		this.navAgent.SetDestination(targetPos);

		//Trying to help navmesh when it sucks
		if (lastPosition == transform.position){
			//this.navAgent.SetDestination(Vector3.MoveTowards(transform.position, navAgent.destination, navAgent.speed*Time.deltaTime));
			transform.position = Vector3.MoveTowards(transform.position, navAgent.destination, navAgent.speed*Time.deltaTime);
		}

		//Making the animation play in the right direction
		Vector3 posDifference = navAgent.destination - transform.position;
		if (posDifference.x < 0){
			transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
		}
		else if (posDifference.x >= 0){
			transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
		}
		enemyAnimator.Play("walk");

		if (CloseToTarget()){
			this.navAgent.SetDestination(transform.position);
			isMoving = false;
		}

		lastPosition = transform.position;
    }


	/**
	*Checks if the enemy has reached their destination
	*/
	public bool ReachedDestination(){
		if (Vector3.Distance(transform.position, navAgent.destination) <= 1.5f){
			return true;
		}
		return false;
	}



	//Checks if the enemy is close to it's target
	public bool CloseToTarget(){
			Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
		if (Vector3.Distance(transform.position, closestPoint) <= 1.5f){
			return true;
		}
		return false;
	}


    /**
     * Finds the closest tower with health and returns it as a GameObject
     * If there are no more towers to target the castle is returned
     */
    public GameObject ClosestTower(){
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        float closestDistance = Vector3.Distance(GameManager.instance.castle.transform.position, transform.position);
        GameObject closest = GameManager.instance.castle;
        
        foreach (GameObject tower in towers){
            float newDistance = Vector3.Distance(tower.transform.position, transform.position);
            
            if (newDistance < closestDistance && tower.GetComponent<towerScript>().towerStats.health > 0){
        		closestDistance = newDistance;
            	closest = tower;
            }
        }
        return closest;
    }


	/*
	*Knocking back the enemy
	*/
	public void KnockBack(Vector3 attackPosition){
		if (!isAlive) {return;}

		pushedBack = true;

		Vector3 knockBackPos = ((transform.position - attackPosition) * knockBackDist) + transform.position;
		knockBackPos.z = -1;
		navAgent.SetDestination(knockBackPos);

		knockBackTime = knockBackRate + Time.time;
	}


	/**
	*Subtracks the damage given from the enemy's health
	*If health drops below zero the enemy is destroyed
	*/
	public void TakeDamage(float damage){
		if (isAlive){
			Vector3 damageSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
			GameObject DamagePop = Instantiate(damagePopup, damageSpawnPos, Quaternion.Euler(0,0,0));
			DamagePop.GetComponent<DamagePopup>().setUp((int)damage);
			health -= damage;
			if (health <= 0){
				notches.GetComponent<notches>().makeNotches(0.0f, maxHealth);
            	healthBar.GetComponent<healthBar>().shrinkHealthBar(0.0f, maxHealth);

				navAgent.SetDestination(transform.position);

				isAlive = false;
				enemyAnimator.Play("die");

                foreach (CoinDropParameters dropOption in coinDropOptions)
                {
                    int numCoins = Random.Range(dropOption.minCoins, dropOption.maxCoins + 1);

                    for (int i = 0; i < numCoins; i++)
                    {
						int spawnChance = Random.Range(0, 2);

						if (spawnChance == 1 || dropOption == coinDropOptions[3]){
                    		Vector3 randomOffset = new Vector3(Random.Range(-dropOption.dropRadius, dropOption.dropRadius), Random.Range(-dropOption.dropRadius, dropOption.dropRadius), 0f);
                        	Vector3 coinSpawnPosition = transform.position + randomOffset;

                        	Instantiate(dropOption.coinPrefab, coinSpawnPosition, Quaternion.identity);
						}
                    }
                }

                //Making sure that all projectiles hit the enemy before it's gone, avoiding error messages
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
				StartCoroutine(Dead());
			}
			else{
				notches.GetComponent<notches>().makeNotches(health, maxHealth);
            	healthBar.GetComponent<healthBar>().shrinkHealthBar(health, maxHealth);
			}
		}
	}
	IEnumerator Dead(){
		yield return new WaitForSeconds(5.0f);
		Destroy(this.gameObject);
	}


	/**
	*Attacks the current target
	*Will only be called when the enemy is stopped and when damageTime is less than Time.time
	*/
	public void Attack(){
		if(!CloseToTarget()){
			isMoving = true;
			return;
		}

		//Checks what object the target is and damages it. If that target's health goes below 0 then a new target is found
		if (target.transform.tag == "Player" && target.GetComponent<Player>().Health <= 0){
			target = ClosestTower();
			isMoving = true;
			return;
		}
		if (target.transform.tag == "Player" && (damage >= target.GetComponent<Player>().Health)){
			GameManager.instance.player.GetComponent<Player>().TakeDamage(damage);
			target = ClosestTower();
			WaitForAnimation();
		}
		else if (target.transform.tag == "Player"){
			GameManager.instance.player.GetComponent<Player>().TakeDamage(damage);
		}
		else if (target.transform.tag == "Tower" && target.GetComponent<towerScript>().towerStats.health <= 0){
			target = ClosestTower();
			isMoving = true;
			return;
		}
		else if (target.transform.tag == "Tower" && (damage >= target.GetComponent<towerScript>().towerStats.health)){
			target.GetComponent<towerScript>().TakeDamage(damage);
			target = ClosestTower();
			WaitForAnimation();
		}
		else if (target.transform.tag == "Tower"){
			target.GetComponent<towerScript>().TakeDamage(damage);
		}
		else if (target.transform.tag == "Castle"){
			target.GetComponent<Castle>().TakeDamage(damage);
		}
		else{
			Debug.LogWarning("In Enemy Script: Attack(): Enemy currently has no appropriate target");
		}
		enemyAnimator.Play("attack");

		int attackSound = Random.Range((int)1, (int)5);
		if(attackSound == 1){
			attack1.Play();
		}
		else if(attackSound == 2){
			attack2.Play();
		}
		else if(attackSound == 3){
			attack3.Play();
		}
		else if(attackSound == 4){
			attack4.Play();
		}

		damageTime = Time.time + damageRate;

		//Making sure that the attack animation finishes before moving again
		if (!CloseToTarget()){
			WaitForAnimation();
		}
	}
	IEnumerator WaitForAnimation(){
		yield return new WaitForSeconds(1.0f);
		isMoving = true;
	}


	/*
	*Sets slowed time to slow the enemy for 3 seconds
	*/
	public void SlowEnemy(){
		slowedTime = Time.time + 3f;
		this.navAgent.speed = 1.5f;
		slowed = true;
	}


	void OnTriggerEnter(Collider other){
		if (other.transform.tag == "Player Collider" && GameManager.instance.playerHidden == false){
			target = GameManager.instance.player;
			isMoving = true;
		}
	}


	void OnTriggerExit(Collider other){
		if (other.transform.tag == "Player Collider" && target.transform.tag == "Player"){
			target = ClosestTower();
			isMoving = true;
		}
	}


	void OnTriggerStay(Collider other){
		if ((target.transform.tag == "Castle" || target.transform.tag == "Tower") && GameManager.instance.playerHidden == false && other.transform.tag == "Player Collider"){
			target = GameManager.instance.player;
			isMoving = true;
		}
	}
}
