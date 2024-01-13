using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempShopScript : shopScript
{
    public Button makeTower;
    public Button makeTower2;
    public Button makeTower3;
    public Button makeTower4;
    public Button makeTower5;
    public GameObject towerContainer;
    public Button pgRight;
    public Button pgLeft;
    public Text pageText;
    public Image shopDisplay;
    public Button closeShop;
    // Start is called before the first frame update
    void Start()
    {
        pgLeft.onClick.AddListener(delegate{pageChange(1);});
        pgRight.onClick.AddListener(delegate{pageChange(2);});
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
    }
    void pageChange(int page){
        makeTower.gameObject.SetActive(page == 1);
        makeTower2.gameObject.SetActive(page == 1);
        makeTower3.gameObject.SetActive(page == 1);
        makeTower4.gameObject.SetActive(page == 2);
        makeTower5.gameObject.SetActive(page == 2);
        pgLeft.gameObject.SetActive(page == 2);
        pgRight.gameObject.SetActive(page == 1);
        pageText.text = "Page: " + page + "/2";
    }
    void snapRight(){
        
        shopDisplay.rectTransform.anchoredPosition = new Vector3(707f, 0f, 0f);
    }
    void snapLeft(){
        shopDisplay.rectTransform.anchoredPosition = new Vector3(93f, 0f,0);
    }
}
