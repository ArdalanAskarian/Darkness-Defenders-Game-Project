using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class shopScript : MonoBehaviour{
    public GameObject rangeCast;
    public void CloseShop(){
        this.gameObject.SetActive(false);
        if (rangeCast){
            rangeCast.SetActive(false);
        }
    }
}

public class upgradeShopScriptTemp : shopScript
{
    public Text TowerTitle;
    public Text towerStats;
    public int goldToHeal;
    public Text healText;
    public Text sellText;
    public TowerStats currentStats;
    public Button upgradeTowerB1;
    public Button upgradeTowerB2;
    public Button closeShop;
    public Button damageTower;
    public Image shopDisplay;
    public Button healTower;
    public Button sellTower;
    public UpgradeBranch[] upgradeTree;
    private int[] branchPos = new int[2];
    public GameObject towerContainer;
    // Start is called before the first frame update
    void Start()
    {
        upgradeTowerB1.onClick.AddListener(delegate{UpgradeTower(0, upgradeTowerB1);});
        upgradeTowerB2.onClick.AddListener(delegate{UpgradeTower(1, upgradeTowerB2);});
        damageTower.onClick.AddListener(DamageTower);
        healTower.onClick.AddListener(HealTower);
        sellTower.onClick.AddListener(SellTower);
        closeShop.onClick.AddListener(CloseShop);
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
        if ( branchPos[0] >= upgradeTree[0].leaves.Length || upgradeTree[0].leaves[branchPos[0]].price > GameManager.instance.gold){
            upgradeTowerB1.image.color = new Color(255,0,0, 110);
        }
        else{
            upgradeTowerB1.image.color = new Color(0,255,0, 110);
        }
        if (branchPos[1] >= upgradeTree[1].leaves.Length || upgradeTree[1].leaves[branchPos[1]].price > GameManager.instance.gold){
            upgradeTowerB2.image.color = new Color(255,0,0, 110);
        }
        else{
            upgradeTowerB2.image.color = new Color(0,255,0, 110);
        }
        if (GameManager.instance.gold > goldToHeal){
            healTower.image.color = new Color(0,255,0, 110);
        }
        else{
            healTower.image.color = new Color(255,0,0, 110);
        }
        if (GameManager.instance.gold + currentStats.value > 0){
            sellTower.image.color = new Color(0,255,0, 110);
        }
        else{
            sellTower.image.color = new Color(255,0,0, 110);
        }
    }
    void UpgradeTower(int branch, Button upButton){
        if (GameManager.instance.gold < upgradeTree[branch].leaves[branchPos[branch]].price){
            return;
        }
        GameManager.instance.AddCoins((int)-upgradeTree[branch].leaves[branchPos[branch]].price);
        towerContainer.GetComponent<towerContainerScript>().raiseAndSetStats(upgradeTree[branch].leaves[branchPos[branch]].upgrade);
        branchPos[branch]++;
        if (branchPos[branch] >= upgradeTree[branch].leaves.Length){
            upButton.interactable = false;
            upButton.GetComponent<UpgradeButtonScript>().SetMaxText();
        }
        else{
            upButton.GetComponent<UpgradeButtonScript>().SetText(upgradeTree[branch].leaves[branchPos[branch]]);
        }
        

    }
    void DamageTower(){
        towerContainer.GetComponent<towerContainerScript>().takeDamage(10);
    }
    void HealTower(){
        if (GameManager.instance.gold > goldToHeal){
            GameManager.instance.AddCoins(-goldToHeal);
            towerContainer.GetComponent<towerContainerScript>().healTower();
            
        }
    }
    void SellTower(){
        if (GameManager.instance.gold + currentStats.value < 0){
            return;
        }
        rangeCast.SetActive(false);
        towerContainer.GetComponent<towerContainerScript>().sellTower();
    }
    public void SetUpgradeTree(UpgradeBranch[] tree){
        this.upgradeTree = tree;
        this.branchPos = new int[2];
        upgradeTowerB1.interactable = true;
        upgradeTowerB1.GetComponent<UpgradeButtonScript>().ResetText();
        upgradeTowerB2.interactable = true;
        upgradeTowerB2.GetComponent<UpgradeButtonScript>().ResetText();
        if (branchPos[0] >= upgradeTree[0].leaves.Length){
            upgradeTowerB1.interactable = false;
            upgradeTowerB1.GetComponent<UpgradeButtonScript>().SetMaxText();
        }
        else{
            upgradeTowerB1.GetComponent<UpgradeButtonScript>().SetText(upgradeTree[0].leaves[branchPos[0]]);
        }
        if (branchPos[1] >= upgradeTree[1].leaves.Length){
            upgradeTowerB2.interactable = false;
            upgradeTowerB2.GetComponent<UpgradeButtonScript>().SetMaxText();
        }
        else{
            upgradeTowerB2.GetComponent<UpgradeButtonScript>().SetText(upgradeTree[1].leaves[branchPos[1]]);
        }
    }
    void snapRight(){
        
        shopDisplay.rectTransform.anchoredPosition = new Vector3(707f, 0f, 0f);
    }
    void snapLeft(){
        shopDisplay.rectTransform.anchoredPosition = new Vector3(93f, 0f,0);
    }
    public void SetTowerTitle(String TowerTitle){
        this.TowerTitle.text = TowerTitle;
    }
    public void UpdateTowerStats(TowerStats newStats){
        this.currentStats = newStats;
        float hpToheal = currentStats.health / currentStats.maxHealth;
        hpToheal -= 1;
        hpToheal *= -1;
        hpToheal = math.round(hpToheal * 100f) * 0.01f;
        if (math.round(currentStats.health) == currentStats.maxHealth){
            healTower.interactable = false;
            healText.text = "Full Health";
            goldToHeal = -1;
        }
        else{
            healTower.interactable = true;
            goldToHeal = (int)math.round(currentStats.value / 3 *  hpToheal);
            healText.text = "Heal Tower: " + goldToHeal + " gold";
        }
        if (currentStats.value > 0){
            sellText.text = "Sell Tower for " + currentStats.value + " gold";
        }
        else{
            sellText.text = "Remove Tree for " + currentStats.value * -1 + " gold";
        }
        if (currentStats.damage == 0){
            towerStats.text = "Health: N/A\nDamage: N/A\nRange: N/A\nFire Rate: N/A\nProjectiles: N/A\nTargets: N/A";
        }
        else{
            towerStats.text = "Health: " + math.round(currentStats.health) + "/" + currentStats.maxHealth + "\n" +
            "Damage: " + currentStats.damage + "\n" + "Range: " + math.round(currentStats.radius * 10f) / 10f + "\n" +
            "Fire Rate: " + math.round(currentStats.fireRate * 10f) / 10f + "\n" + "Projectiles: " + currentStats.numbProjectiles + "\n" +
            "Targets: " + currentStats.numbTargets;
        }
    }
}
