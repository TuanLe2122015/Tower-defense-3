using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager1 : MonoBehaviour
{
    //public GameObject spot;
    //List<GameObject> enemyOnMap = new List<GameObject>();

    //List<bool> eligibleCreateWave;
    Transform repawnPoint;

    //public GameObject [] listTower;

    //[System.NonSerialized]
    //private GameObject towerSelected;
    //[System.NonSerialized]
    //public GameObject spotSelected;

    public GameObject activeWave;

    public float money = 20;
    public float lives = 20;
    //public Text textMoney;
    //public Text textLives;

    //public GameObject bottomButton;
    public int curentWave = 0;
    public float multiPowerEnemy = 1f;

    [SerializeField] private GameObject enemyObj = null;

    // Start is called before the first frame update
    void Start()
    {
        //repawnPoint = GameObject.Find("PathOfEnemy").transform.GetChild(0);
        //textMoney.text += money.ToString();
        //textLives.text += lives.ToString();
        //towerSelected = null;

        PhotonNetwork.Instantiate(enemyObj.name, Vector3.zero, Quaternion.identity);
    }

    public void LoseLives()
    {
        lives--;
        //textLives.text = "Lives :"+ lives.ToString();
    }
    public void EarnMoney(float mon)
    {
        money += mon;
        //textMoney.text = "Money: " + money.ToString();
    }
    //public void DeleteEnemy(GameObject enemyDie)
    //{
    //    //enemyOnMap.Remove(enemyDie);
    //    //Debug.Log("Enemy dead!!!");
    //}

    //public List<GameObject> GetEnemyOnMap()
    //{
    //   // return enemyOnMap;
    //}
    //public void AddEnenemyToMap(GameObject ene)//  dùng để xử lý tìm đường đi ngắn nhất cho enemy
    //{
    //    //enemyOnMap.Add(ene);
    //}

    //void ActiveButton()
    //{
    //    bottomButton.transform.GetChild(0).gameObject.SetActive(false);
    //    bottomButton.transform.GetChild(1).gameObject.SetActive(false);
    //    bottomButton.transform.GetChild(2).gameObject.SetActive(false);
    //    bottomButton.transform.GetChild(3).gameObject.SetActive(true);
    //}
    //public void ButtonBuyTowerSingleActtack()
    //{
    //    towerSelected = listTower[0];
    //    //ActiveButton();
    //}
    //public void ButtonBuyTowerAoeActtack()
    //{
    //    towerSelected = listTower[1];
    //    //ActiveButton();
    //}

    //public void ButtonBuyTowerSuperAoeActtack()
    //{
    //    towerSelected = listTower[2];
    //    //ActiveButton();
    //}
    //public bool SuccessBuyTower() // Kiểm tra có đủ tiền mua tower hay không
    //{
    //    if (towerSelected == null)
    //        return false;
    //    if (money >= towerSelected.GetComponent<Tower>().cost)
    //    {
    //        money -= towerSelected.GetComponent<Tower>().cost;
    //        //textMoney.text = "Money: " + money.ToString();
    //        return true;
    //    }
    //    return false;
    //}
    //public void ButtonBuildTower()
    //{
    //    if (spotSelected != null)
    //    {
    //        if (SuccessBuyTower())
    //        {
    //            spotSelected.GetComponent<TowerSpot>().BuildTower(towerSelected);
    //        }
    //        else
    //        {
    //            Debug.Log("We need more gold!!!");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Not yet selected SPOT!!!");
    //    }
    //}

    //public void FinishBuildingTower()
    //{
    //    towerSelected = null;
    //}

    //public void ActiveWave()
    //{
    //    activeWave.gameObject.SetActive(true);
    //}
}
