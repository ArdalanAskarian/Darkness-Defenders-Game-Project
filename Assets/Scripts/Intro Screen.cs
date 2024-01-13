using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{

    public Button skipButton;
    public bool alreadySkipped = false;
    public bool isActive = false;
    public Image grey;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        skipButton.onClick.AddListener(closeIntro);
        audio = GetComponent<AudioSource>();
    }

    void Update() 
    {
        if (isActive == true)
        {
            audio.Play();
            isActive = false;
        }
    }
    void closeIntro() 
    {
        grey.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        audio.Stop();
    }
}
