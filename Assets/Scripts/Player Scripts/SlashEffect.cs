using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody rb;

    private Animator anim;
    public float damage= 1.0f;
    void Start()
    {
        playerStats= GameObject.FindObjectOfType<PlayerStats>().GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>(); 

        anim.speed = playerStats.animationSpeed;
    }

   public void DestroySelf(){
        Destroy(gameObject);
   }

   void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 3) {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<Enemy>().KnockBack(transform.position);
        }
    }
}
