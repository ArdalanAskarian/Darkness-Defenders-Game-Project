using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidController : MonoBehaviour
{
    public GameObject raidCanvas;


    public void StartRaid(){
        raidCanvas.SetActive(false);
        GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().StartSpawns();
    }


    public void DisplayButton(){
        Debug.Log("clicked");
        raidCanvas.SetActive(true);
    }
}
