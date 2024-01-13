using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restartbutton : MonoBehaviour
{
    public Button restartButt;

    // Start is called before the first frame update
    void Start()
    {
        restartButt.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartGame()
    {
        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
