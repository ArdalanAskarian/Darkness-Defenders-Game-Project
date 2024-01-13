using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{


    public static bool IsMagnetic = false;
    // public GameObject collectEffect;
    [SerializeField] public float AttractorSpeed = 3.0f;


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            
            other.GetComponent<Coin>().Movement();
        }

    }


}
