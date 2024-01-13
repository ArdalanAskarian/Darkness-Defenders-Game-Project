using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Transform firePoint;
    public GameObject slashEffect;
    private Animator animator; 

    public AudioClip swingSound; 
    public AudioSource audioSource; 
    private void Start()
    {
        animator = GetComponent<Animator>(); 
        audioSource = GetComponent<AudioSource>(); 

    }

   public float damage = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.instance.playerDead)
        {
            GameObject slash = Instantiate(slashEffect, firePoint.position, firePoint.rotation);
            slash.GetComponent<SlashEffect>().damage = damage;
            // Instantiate(slashEffect, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(swingSound, Random.Range(0.8f, 1f));
            // animator.SetBool("isAttacking", true); 
            animator.Play("PlayerAttack");
        }
        else
        {
            animator.SetBool("isAttacking", false); 
        }
    }
}