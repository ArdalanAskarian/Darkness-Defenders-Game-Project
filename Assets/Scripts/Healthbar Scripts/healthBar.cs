using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{

    public Image bar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    public void shrinkHealthBar(float health, float maxHealth){
        bar.fillAmount = health / maxHealth;
        if (health > 2 * maxHealth / 3){
            bar.color = new Color(18f/255f, 164f/255f, 31f/255f);
        }
        else if (health > maxHealth / 3){
            bar.color = new Color(253f/255, 255/255, 0);
        }
        else{
            bar.color = new Color(255, 0, 0);
        }
    }
}
