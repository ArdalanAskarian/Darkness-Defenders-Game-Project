using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public GameObject Option_1;
     
    public IEnumerator tryagain()
    {
        Option_1.SetActive(false);
        yield return new WaitForSeconds(5f);
        Option_1.SetActive(true);
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

