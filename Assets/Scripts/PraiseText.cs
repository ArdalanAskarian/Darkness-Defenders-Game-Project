using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PraiseText : MonoBehaviour
{

    public string textValue;
    public Text currentPraise;
    public GameObject spawner;
    private int currentFinishedRaids; // tracker to compare to
    private int finishedRaids; // live updated number of finished raids from the enermy spawner script
    private float timer = 0f;
    private float maxTimer = 15f;
    private string[] Messages;

    // Start is called before the first frame update
    void Start()
    {
        finishedRaids = spawner.GetComponent<EnemySpawner>().completedRaids;
        currentFinishedRaids = 0;
        Messages = new string[] {"The world trembles at your diabolical feet!", "Yet another wave of disgusting goodness defeated!", "Your evil deeds are reaching legendary status!",
                                             "You are truly the bane of the worlds existence!", "Evil looks good on you!", "Your wickedness knows no bounds!",
                                              "What a masterpiece of malevolence!", "Keep up the chaos!", "Keep embracing your inner imp!", "Darkness prevails yet again!"};
    }

    void Update() {
        finishedRaids = spawner.GetComponent<EnemySpawner>().completedRaids;
        timer += Time.deltaTime;
        if (currentFinishedRaids != finishedRaids) { // they will be unequal when a raid finishs, which will trigger a new message showing
            string message = pickNewMessage();
            currentPraise.text = message;
            currentFinishedRaids += 1;
            timer = 0f;
        }
        if (timer >= maxTimer) {
            currentPraise.text = "";
        }
    }

    string pickNewMessage(){
        // longish, evil evil messages
        Random picker = new Random();
        int index = picker.Next(0, Messages.Length);
        return Messages[index];
    }


}
