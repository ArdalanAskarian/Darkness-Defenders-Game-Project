using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonScript : MonoBehaviour
{
    public Text price;
    public Text upgrades;
    public Text maxed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(TowerUpgrade stats){
        String improve  = "";
        if (stats.upgrade.health != 0){
            improve += " - Increases health by " + stats.upgrade.health + "\n";
        }
        if (stats.upgrade.damage != 0){
            improve += " - Increases damage by " + stats.upgrade.damage + "\n";
        }
        if (stats.upgrade.radius != 0){
            improve += " - Increases range by " + stats.upgrade.radius * 1 + "\n";
        }
        if (stats.upgrade.numbTargets != 0){
            improve += " - Fire at " + stats.upgrade.numbTargets + " additional targets\n";
        }
        if (stats.upgrade.numbProjectiles != 0){
            improve += " - Fire " + stats.upgrade.numbProjectiles + " additional projectiles\n";
        }
        if (stats.upgrade.fireRate != 0){
            improve += " - Increases fire rate by " + stats.upgrade.fireRate + "\n";
        }
        upgrades.text = improve;
        price.text = "Cost: " + stats.price + " gold";
    }
    public void SetMaxText(){
        price.text = "";
        upgrades.text = "";
        maxed.text = "Fully Upgraded";
    }
    public void ResetText(){
        price.text = "";
        upgrades.text = "";
        maxed.text = "";
    }
}
