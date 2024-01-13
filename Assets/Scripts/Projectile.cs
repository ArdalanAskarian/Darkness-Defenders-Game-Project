using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public AudioClip impactSound;
    public AudioSource soundSource;
    public bool freeze;
    public bool explosive;
    public GameObject explosionEffect;
    [SerializeField] private float lifeTime = 5.0f;
    [SerializeField] private float moveSpeed = 300.0f;
    public Collider target;
    public float damage;
    public bool healing= false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileHome();
        Spin();
    }

    private void MoveProjectile() {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    private void ProjectileHome(){
        if (target == null){
            Destroy(this.gameObject);
        }
        Vector3 direction = target.transform.position - this.transform.position;
        float distance = direction.magnitude;
        direction = direction/distance;
        this.transform.position += new Vector3(moveSpeed * direction.x * Time.deltaTime, moveSpeed * direction.y * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other) {
        if (other == target){
            if (healing){
                other.GetComponent<Player>().TakeDamage(-damage); 
                Destroy(this.gameObject);
                return;
            }
            if (explosive){
                Vector3 pos = target.transform.position;
                Quaternion rotation = target.transform.rotation;
                Collider[] targets = Physics.OverlapSphere(this.transform.position, 3f, LayerMask.GetMask("Enemy"));
                soundSource.PlayOneShot(impactSound, 0.05f);
                GameObject effect = Instantiate(explosionEffect, pos, rotation);
                Destroy(effect, 0.4f);
                for (int i = 0; i < targets.Length; i++){
                    targets[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            else{
               other.GetComponent<Enemy>().TakeDamage(damage); 
            }
            if (freeze){
                other.GetComponent<Enemy>().SlowEnemy();
            }
            Destroy(this.gameObject);
        }
    }

    private void Spin() {
        transform.Rotate(0.0f, 0.0f, 2.5f); //z axis
    }

    //void OnTriggerEnter(Collider other) {
    //    if (other.transform.tag == "Enemy") {
    //        other.GetComponent<Enemy>().TakeDamage(damage);
    //        Destroy(this.gameObject);
    //    }
    //}
}