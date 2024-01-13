using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;
    // Start is called before the first frame update
    float t = 0;
    Color32 startFaceColor = new Color32(255,0,0,255);
    Color32 endFaceColor = new Color32(255,0,0,0);
    Color32 startOutlineColor = new Color32(0,0,0,255);
    Color32 endOutlineColor = new Color32(0,0,0,0);
    void Awake()
    {
        text = this.GetComponent<TextMeshPro>();
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(this.transform.position.x , this.transform.position.y + (0.7f * Time.deltaTime));        
        if (t < 1){
            text.faceColor = Color32.Lerp(startFaceColor, endFaceColor, t);
            text.outlineColor = Color32.Lerp(startOutlineColor, endOutlineColor, t);
            t += Time.deltaTime / 1f;
        }
        

    }
    public void setUp(int Damage){
        if (Damage < 0){
            startFaceColor = new Color(0,255,0,255);
            endFaceColor = new Color32(0,255,0,0);
        }
        Damage = Math.Abs(Damage);
        text.text = "" + Damage;
        
    }
    IEnumerator Die(){
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
