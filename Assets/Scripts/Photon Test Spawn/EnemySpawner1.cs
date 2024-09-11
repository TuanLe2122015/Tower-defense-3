using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner1 : MonoBehaviourPun
{
    float CDSpawnEnemy = 0.3f;
    float CDSpawNextWave = 3;

    [System.NonSerialized]
    public GameObject gameManager;

    [SerializeField]
    public GameObject enePrefab;

    //public class WaveComponent
    //{
    //    public GameObject enemyRefabs;
    //    public int numberOfEnemy;
    //    [System.NonSerialized]
    //    public int spawned = 0;
    //}
    //[SerializeReference]
    //public WaveComponent[] numberOfWave;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        PhotonNetwork.Instantiate(enePrefab.name, transform.position,transform.rotation);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Hết thời gian tạo Wave mới
    //    CDSpawNextWave -= Time.deltaTime;

    //    if (CDSpawNextWave <= 0)
    //    {
    //        // Mỗi thời gian ngắn tạo 1 enemy
    //        CDSpawNextWave = CDSpawnEnemy;

    //        bool didSpawn = false;
    //        foreach(WaveComponent com in numberOfWave)
    //        {
    //            if (com.spawned < com.numberOfEnemy)
    //            {
    //                Vector3 posEnemy = transform.position;
    //                PhotonNetwork.Instantiate(com.enemyRefabs.name, posEnemy, Quaternion.identity);
    //                //NewEnemy.GetComponent<Enemy>().addOnMap = true;

    //                //Sau mỗi wave máu tối đa của enemy tăng thêm 15%
    //                //NewEnemy.GetComponent<Enemy>().GetPower(gameManager.GetComponent<GameManager1>().multiPowerEnemy);

    //                com.spawned++;
    //                didSpawn = true;
    //                break;
    //            }
    //        }

    //        // Hủy wave đã xong, tạo wave mới, tang suc manh cua enemy
    //        if (didSpawn == false)
    //        {
    //            //gameManager.GetComponent<GameManager1>().multiPowerEnemy *= 1.15f;
    //            //gameManager.GetComponent<GameManager1>().curentWave++;
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}