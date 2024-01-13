using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{

    public Button startButton;
    public Button tutorialButton;
    public Image backgroundImage;
    public bool isActive = true;
    public GameObject intro;
    public GameObject tutorial;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(startGame);
        tutorialButton.onClick.AddListener(openTutorial);
        Time.timeScale = 0.0f;
        animator = GetComponent<Animator>();
        animator.Play("castleinthedark_0");
    }


    void startGame() {
        backgroundImage.gameObject.SetActive(false);
        isActive = false;
        intro.GetComponent<IntroScreen>().isActive = true;
    }

    void openTutorial() {
        // insert code to open tutorial here
        tutorial.SetActive(true);
        
    }

}
