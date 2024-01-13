using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class DeathText : MonoBehaviour
{

    private float timer = 0f;
    private float maxTimer = 2.5f;
    public TextMeshProUGUI deathMessage;
    public GameObject enemy; // put the prefab in here
    // Start is called before the first frame update
    private bool alive;
    private string[] Messages;
    int index;
    void Start()
    {
        deathMessage.text = "";
        Messages = new string[] {"EVIL!", "GHASTLY!", "HORRIFIC!"};
        Random picker = new Random();
        index = picker.Next(0, Messages.Length);
    }

    // Update is called once per frame
    void Update()
    {
        alive = enemy.GetComponent<Enemy>().isAlive;
        timer += Time.deltaTime;
        if (!alive){
            deathMessage.text = Messages[index];
            timer = 0f;
            alive = true; // not actually alive, just need to stop the timer from resetting to 0
        }
        if (timer >= maxTimer) {
            deathMessage.text = "";
        }
    }
}
