using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buyTowerButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tower;
    public towerScript towerStats;
    public Text title;
    public Text desc;
    public Text cost;
    public Image background;
    public Text titleText;
    public Text towerStatsDisplay;
    public GameObject towerContainer;
    public GameObject rangeCast;
    public Button makeTower;
    // Start is called before the first frame update
    void Start()
    {
            towerStats = tower.GetComponent<towerScript>();
            title.text = towerStats.TowerTitle;
            desc.text = towerStats.TowerDesc;
            cost.text = "Cost: " + towerStats.towerStats.value + " gold";
            titleText.text = "";
            towerStatsDisplay.text = "";
            makeTower.onClick.AddListener(spawnTower);
    }
    void Update() 
    {
        if (GameManager.instance.gold >= towerStats.towerStats.value){
            background.color = new Color(0,255,0, 110);
        }
        else{
            background.color = new Color(255,0,0, 110);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rangeCast.transform.localScale = new Vector3(towerStats.towerStats.radius, towerStats.towerStats.radius, 1f);
        rangeCast.SetActive(true);
        titleText.text = towerStats.TowerTitle;
        towerStatsDisplay.text = "Health: " + math.round(towerStats.towerStats.health) + "\n" +
            "Damage: " + towerStats.towerStats.damage + "\n" + "Range: " + towerStats.towerStats.radius + "\n" +
            "Fire Rate: " + towerStats.towerStats.fireRate + "\n" + "Projectiles: " + towerStats.towerStats.numbProjectiles + "\n" +
            "Targets: " + towerStats.towerStats.numbTargets;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rangeCast.SetActive(false);
        titleText.text = "";
        towerStatsDisplay.text = "";
    }
    void spawnTower(){
        if (towerStats.towerStats.value <= GameManager.instance.gold){
            towerContainer.GetComponent<towerContainerScript>().setTower(tower);
            GameManager.instance.AddCoins((int)-towerStats.towerStats.value);
        }
    }
}
