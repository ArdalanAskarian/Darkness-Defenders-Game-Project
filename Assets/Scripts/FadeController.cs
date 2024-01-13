using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeController : MonoBehaviour
{
    public GameObject blackOutImage;
    public Text appearingText;
    private AudioSource audioSource;
    public GameObject player;
    public Text raidNumText;


    public Buttons Option_1;
    void Start()
    {
        player.GetComponent<AudioSource>().Stop();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        raidNumText.text = "Raids Survived: " + GameManager.instance.EnemySpawner.GetComponent<EnemySpawner>().completedRaids;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void StartFade()
    {
        StartCoroutine(FadeBlackOutImage());
    }
    public IEnumerator FadeBlackOutImage(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        Color objectColor = blackOutImage.GetComponent<Image>().color;
        float fadeAmount;
        StartCoroutine(Option_1.tryagain());

        if (fadeToBlack)
        {
            while(blackOutImage.GetComponent<Image>().color.a <1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else 
        {
            while(blackOutImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
       
        Color textColor = appearingText.color;
        textColor.a = 0;

        while (textColor.a < 1)
        {
            fadeAmount = textColor.a + (fadeSpeed * Time.deltaTime);
            textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
            appearingText.color = textColor;
            raidNumText.color = textColor;
            yield return null;
        }

        
    }
    
}
