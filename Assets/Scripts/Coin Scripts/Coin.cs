using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Value of coins

    [SerializeField] public int coinValue;
    public int bones;
    public bool rotate;
    public float rotationSpeed;

    [SerializeField] public float AttractorSpeed = 3.0f;

    private bool isMagnetic;
    public bool IsMagnetic => isMagnetic;

    public bool alive2 = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && alive2)
        {
            alive2 = false;
            Collect();
        }

    }

    public void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, AttractorSpeed * Time.deltaTime);

    }

    public void Collect()
    {
        GameManager.instance.player.GetComponent<Player>().coinEffect();
        GameManager.instance.AddBones(bones);
        GameManager.instance.AddCoins(coinValue);
        Destroy(gameObject);

    }


}
