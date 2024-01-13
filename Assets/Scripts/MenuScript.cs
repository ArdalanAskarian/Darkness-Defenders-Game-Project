using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    

    public Button two;

    public Button three;
  

    public Image menu;
    public bool isPaused = false;

    public GameObject tutorial;
     

    
    // Start is called before the first frame update
    void Start()
    {
        // pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        
    
        
        two.onClick.AddListener(Build);
        
        three.onClick.AddListener(Resume);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void Build()
    {
       tutorial.SetActive(true);
    }

    void Resume()
    {
        menu.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
}
