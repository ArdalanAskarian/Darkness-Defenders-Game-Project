using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct Upgrade{
    public int upVal;
    public int cost;
}
[System.Serializable]
public struct Upgrades{
    public Upgrade[] upgrades;
}
public class DragonUpgradeScript : shopScript
{
    public GameObject player;
    public Image shopDisplay;
    public Upgrades[] upgrades;
    public GameObject dragon;
    public Text DText;
    public Text PText;
    public int recruitCost;
    private int dmgIdx = 0;
    private int hpIdx = 0;
    private int costIdx = 0;
    public Button recruit;
    public Image recruitBack;
    public Button upDmg;
    public Image dmgBack;
    public Button lowCost;
    public Image lowerCostBack;
    public Button dismiss;
    public Image dismissBack;
    public Button close;
    public Text dmgText;
    public Button pgLeft;
    public Button pgRight;
    public Button hpButton;
    public Text hpText;
    public Image hpBack;
    public Text playerText;
    public Text recruitText;
    public Text costLowerText;
    public Text statsText;
    public Button raiseSpeed;
    private int spdIdx = 0;
    public Text speedText;
    public Image speedBack;
    int page;
    public Text pageText;
    public Text playerUpgrades;
    public Button sellBone;
    public Image sellBack;
    public Text dragonUpgrades;
    // Start is called before the first frame update
    void Start()
    {
        pageChange(1);
        page = 1;
        resetText();
        recruit.onClick.AddListener(recruitDragon);
        upDmg.onClick.AddListener(upDamage);
        lowCost.onClick.AddListener(redCost);
        dismiss.onClick.AddListener(dismissDragon);
        raiseSpeed.onClick.AddListener(spdRaise);
        close.onClick.AddListener(CloseShop);
        hpButton.onClick.AddListener(raiseHp);
        pgLeft.onClick.AddListener(delegate{pageChange(1);});
        pgRight.onClick.AddListener(delegate{pageChange(2);});
        sellBone.onClick.AddListener(sellBones);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player.transform.position.x > 6.5){
            snapLeft();
        }
        else if (GameManager.instance.player.transform.position.x < -6.5){
            snapRight();
        }
        if (Input.GetMouseButtonDown(1)){
            CloseShop();
        }
        if (page == 1){
            if (GameManager.instance.dragonAlive && costIdx < upgrades[1].upgrades.Length && GameManager.instance.gold >= upgrades[1].upgrades[costIdx].cost){
                lowerCostBack.color = new Color(0,255,0, 110);
            }
            else{
                lowerCostBack.color = new Color(255,0,0, 110);
            }
            if (GameManager.instance.dragonAlive && spdIdx < upgrades[2].upgrades.Length && GameManager.instance.gold >= upgrades[2].upgrades[spdIdx].cost){
                speedBack.color = new Color(0,255,0, 110);
            }
            else{
                speedBack.color = new Color(255,0,0, 110);
            }
            if (!GameManager.instance.dragonAlive && GameManager.instance.gold >= recruitCost){
                recruitBack.color = new Color(0,255,0, 110);
            }
            else{
                recruitBack.color = new Color(255,0,0, 110);
            }
        }
        else{
           if (dmgIdx < upgrades[0].upgrades.Length && GameManager.instance.bones >= upgrades[0].upgrades[dmgIdx].cost){
                dmgBack.color = new Color(0,255,0, 110);
            }
            else{
                dmgBack.color = new Color(255,0,0, 110);
            }

            if (hpIdx < upgrades[3].upgrades.Length && GameManager.instance.bones >= upgrades[3].upgrades[hpIdx].cost){
                hpBack.color = new Color(0,255,0, 110);
            }
            else{
                hpBack.color = new Color(255,0,0, 110);
            } 
            if (GameManager.instance.bones > 0){
                sellBack.color = new Color(0,255,0, 110);
            }
            else{
                sellBack.color = new Color(255,0,0, 110);
            } 
        }
        
        
        if (page == 1 && GameManager.instance.dragonAlive){
            if (costIdx < upgrades[1].upgrades.Length){
                lowCost.interactable = true;
            }
            else{
                lowCost.interactable = false;
            }
            if (spdIdx < upgrades[2].upgrades.Length){
                raiseSpeed.interactable = true;
            }
            else{
                raiseSpeed.interactable = false;
            }
            dismiss.interactable = true;
            dismissBack.color = new Color(0,255,0, 110);
            recruit.interactable = false;
        }
        else if (page == 1){
            recruit.interactable = true;
            dismissBack.color = new Color(255,0,0, 110);
            dismiss.interactable = false;
            lowCost.interactable = false;
            raiseSpeed.interactable = false;
        }
        else if (page == 2){
            if (dmgIdx < upgrades[0].upgrades.Length){
                upDmg.interactable = true;
            }
            else{
                upDmg.interactable = false;
            }
            if (hpIdx < upgrades[3].upgrades.Length){
                hpButton.interactable = true;
            }
            else{
                hpButton.interactable = false;
            }
        }
    }
    void sellBones(){
        if (GameManager.instance.bones > 0){
            GameManager.instance.AddBones(-1);
            GameManager.instance.AddCoins(10);
        }
    }
    void dismissDragon(){
        dragon.GetComponent<Dragon>().resetTimer();
        dragon.GetComponent<Dragon>().DismissDragonEffect();
        GameManager.instance.dragonAlive = false;
        dragon.SetActive(false);
        resetText();
    }
    void recruitDragon(){
        if (GameManager.instance.gold >= recruitCost){
            GameManager.instance.AddCoins(-recruitCost);
            dragon.SetActive(true);
            dragon.GetComponent<Dragon>().resetTimer();
            GameManager.instance.dragonAlive = true;
            resetText();

        }
    }

    void raiseHp(){
        if (GameManager.instance.bones >= upgrades[3].upgrades[hpIdx].cost){
            GameManager.instance.AddBones(-upgrades[3].upgrades[hpIdx].cost);
            player.GetComponent<Player>().Health += upgrades[3].upgrades[hpIdx].upVal;
            player.GetComponent<Player>().maxHealth += upgrades[3].upgrades[hpIdx].upVal;
            hpIdx++;
            resetText();
        }
    }
    void upDamage(){
        if (GameManager.instance.bones >= upgrades[0].upgrades[dmgIdx].cost){
            GameManager.instance.AddBones(-upgrades[0].upgrades[dmgIdx].cost);
            player.GetComponent<PlayerActions>().damage += upgrades[0].upgrades[dmgIdx].upVal;
            dmgIdx++;
            resetText();

            
        }
    }

    void redCost(){
        if (GameManager.instance.gold >= upgrades[1].upgrades[costIdx].cost){
            GameManager.instance.AddCoins(-upgrades[1].upgrades[costIdx].cost);
            dragon.GetComponent<Dragon>().cost -= upgrades[1].upgrades[costIdx].upVal;
            costIdx++;
            resetText();

            
        }
    }
    void spdRaise(){
        if (GameManager.instance.gold >= upgrades[2].upgrades[spdIdx].cost){
            GameManager.instance.AddCoins(-upgrades[2].upgrades[spdIdx].cost);
            dragon.GetComponent<Dragon>().dragonSpeed += upgrades[2].upgrades[spdIdx].upVal/5f;
            spdIdx++;
            resetText();

            
        }
    }
    public void resetText(){
        if (GameManager.instance.dragonAlive){
            recruitText.text = "Dragon Already Recruited";
        }
        else{
            recruitText.text = "Recruit Dragon\nCost " + recruitCost + " gold";
        }
        if (costIdx < upgrades[1].upgrades.Length){
            costLowerText.text = "Lower Upkeep by: " + upgrades[1].upgrades[costIdx].upVal + " gold\nCost: " + upgrades[1].upgrades[costIdx].cost + " gold";
        }
        else{
            costLowerText.text = "Fully Upgraded";
        }
        if (dmgIdx < upgrades[0].upgrades.Length){
            dmgText.text = "Increase Damage by: " + upgrades[0].upgrades[dmgIdx].upVal + "\nCost: " + upgrades[0].upgrades[dmgIdx].cost + " bones";
        }
        else{
            dmgText.text = "Fully Upgraded";
        }

        if (spdIdx < upgrades[2].upgrades.Length){
            speedText.text = "Increase Speed by: " + upgrades[2].upgrades[spdIdx].upVal + "\nCost: " + upgrades[2].upgrades[spdIdx].cost + " gold";
        }
        else{
            speedText.text = "Fully Upgraded";
        }
        if (hpIdx < upgrades[3].upgrades.Length){
            hpText.text = "Increase Health by: " + upgrades[3].upgrades[hpIdx].upVal + "\nCost: " + upgrades[3].upgrades[hpIdx].cost + " bones";
        }
        else{
            hpText.text = "Fully Upgraded";
        }
        statsText.text = /*"Damage: " + dragon.GetComponent<Dragon>().damage +*/ "Upkeep: " + dragon.GetComponent<Dragon>().cost + " gold per 20s\nSpeed: " + dragon.GetComponent<Dragon>().dragonSpeed * 5;
        playerText.text = "Damage: " + player.GetComponent<PlayerActions>().damage + "\nHealth: " + player.GetComponent<Player>().maxHealth;
    }
    void snapRight(){    
        shopDisplay.rectTransform.anchoredPosition = new Vector3(707f, 0f, 0f);
    }
    void snapLeft(){
        shopDisplay.rectTransform.anchoredPosition = new Vector3(93f, 0f,0);
    }
    void pageChange(int newPage){
        page = newPage;
        pageText.text = "Page: " + page;
        upDmg.gameObject.SetActive(page == 2);
        hpButton.gameObject.SetActive(page == 2);
        pgLeft.gameObject.SetActive(page == 2);
        pgRight.gameObject.SetActive(page == 1);
        recruit.gameObject.SetActive(page == 1);
        lowCost.gameObject.SetActive(page == 1);
        raiseSpeed.gameObject.SetActive(page == 1);
        PText.gameObject.SetActive(page == 2);
        DText.gameObject.SetActive(page == 1);
        dismiss.gameObject.SetActive(page == 1);
        statsText.gameObject.SetActive(page == 1);
        playerText.gameObject.SetActive(page == 2);
        playerUpgrades.gameObject.SetActive(page == 2);
        sellBone.gameObject.SetActive(page == 2);
        dragonUpgrades.gameObject.SetActive(page == 1);
    }
}
