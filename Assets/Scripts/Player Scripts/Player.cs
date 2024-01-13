using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float speedX, speedY;
    Animator playerAnimator;

    [SerializeField] public float Health = 0;
    [SerializeField] public float maxHealth = 100.0f;
    public GameObject healthBar;
    public GameObject notches;
    private SpriteRenderer spriteRenderer;
    public GameObject damagePopup;
    public GameObject collectEffect;
    public GameObject deathEffect;
    public AudioClip coinAudio;
    private AudioSource audio;


    Rigidbody rb;

    private float timer = 10f;
    private float maxTimer = 10f;

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        Health = maxHealth;
        notches.GetComponent<notches>().makeNotches(Health, maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(Health, maxHealth);
        audio = GetComponent<AudioSource>();


    }

    void Update()
    {
        if (timer < maxTimer && !GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().raidOnGoing)
        {
            timer = maxTimer;
        }

        if (timer >= maxTimer)
        {
            Movement();
        }
        else
        {
            timer += Time.deltaTime;
            transform.position = new Vector3(0f, 0f, -3f);
            Death();
        }
    }

    public void TakeDamage(float damage)
    {
        Vector3 damageSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
        GameObject DamagePop = Instantiate(damagePopup, damageSpawnPos, Quaternion.Euler(0, 0, 0));
        DamagePop.GetComponent<DamagePopup>().setUp((int)damage);
        Health -= damage;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
        notches.GetComponent<notches>().makeNotches(Health, maxHealth);
        healthBar.GetComponent<healthBar>().shrinkHealthBar(Health, maxHealth);

        if (Health <= 0)
        {



            GameObject deathEffectInstance = Instantiate(deathEffect, transform.position, transform.rotation);
            StartCoroutine(DestroyAfterAnimation(deathEffectInstance, 0.49f));

            transform.position = new Vector3(0f, 0f, -3f);

            Health = maxHealth;
            notches.GetComponent<notches>().makeNotches(Health, maxHealth);
            healthBar.GetComponent<healthBar>().shrinkHealthBar(Health, maxHealth);
            GameManager.instance.playerDied();
            timer = 0f;
            Death();
        }

    }

    private void Movement()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;

        bool isMoving = (speedX != 0 || speedY != 0);
        playerAnimator.SetBool("isMoving", isMoving);

        if (speedX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (speedX > 0)
        {
            spriteRenderer.flipX = false;
        }

        rb.velocity = new Vector3(speedX, speedY);
    }
    public void Death()
    {
        speedX = 0f;
        speedY = 0f;
        rb.velocity = new Vector3(speedX, speedY);
        playerAnimator.SetBool("isDead", true);
    }

    IEnumerator DestroyAfterAnimation(GameObject effectInstance, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(effectInstance);
    }

    public void coinEffect()
    {
        Destroy(Instantiate(collectEffect, transform), 0.3f);
        audio.PlayOneShot(coinAudio, 0.6f);
    }
}
