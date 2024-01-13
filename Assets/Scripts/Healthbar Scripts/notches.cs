using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notches : MonoBehaviour
{
    [SerializeField] public float healthPerNotch = 10f;
    public GameObject notch;
    private GameObject[] curNotches = {};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void makeNotches(float health, float maxHealth){
        int notchesToMake = (int)Math.Floor(health / healthPerNotch);
        if(notchesToMake < 0){
            return;
        }
        for (int i = 0; i < curNotches.Length; i++){
            Destroy(curNotches[i]);
        }
        if (health <= 0){
            return;
        }
        
        if (health == maxHealth){
            notchesToMake -= 1;
        }
        float hpPerUnitDist = maxHealth / 100f;
        curNotches = new GameObject[notchesToMake];
        
        for (int i = 0; i < notchesToMake; i++){
            curNotches[i] = Instantiate(notch, transform.position, transform.rotation);
            curNotches[i].transform.SetParent(this.transform);
            RectTransform rect = curNotches[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(0,0,0);
            rect.localPosition = new Vector3( healthPerNotch / hpPerUnitDist * (i + 1),20f,0);
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}
