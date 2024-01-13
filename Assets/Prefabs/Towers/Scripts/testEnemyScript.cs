using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemyScript : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(float damage){
        Debug.Log(damage);
    }
}
