using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tower : Photon.Pun.MonoBehaviourPun
{
    float dist = Mathf.Infinity;
    public float AttackRange= 10f;
    public float AttacDame = 1f;
    public float AttacSpeed = 1f;
    public GameObject BulletFrefab;
    float fireCoolDownStan = 0.4f;
    float fireCoolDown = 0.4f;
    float fireCoolDownLeft = 0;
    public float bulletExplosionRadius = 0f;
    
    List<GameObject> enemyOnMap;
    GameObject targetEnemy;

    // Model của Tower
    //public Transform TowerModelTran;
    private Quaternion preRotionToSmoth = new Quaternion();

    public float cost = 6;
    public GameObject PosCreateBullet;

    public GameObject Model_Tower;
    Animator Anima_Model_Tower;

    public bool IsSuperTower;
    public bool HasAnimation;

    // Start is called before the first frame update

    void Awake()
    {
        // Dùng cho Tower có hình đơn giản
        //TShootTran = transform.Find("Tower 1");

        //Dùng cho tower có Model Animation
        if (HasAnimation)
            Anima_Model_Tower = Model_Tower.GetComponent<Animator>();
    }

    void Start()
    {
        fireCoolDown = fireCoolDownStan / AttacSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        // thời gian giảm đến 0 mới được phép bắn đạn
        fireCoolDownLeft -= Time.deltaTime;

        //Hướng mặt về kẻ thù trong tầm bắn gần nhất, cho đến khi ra khỏi tầm bắn, mới hướng tới kẻ thù gần nhất khác
        if (targetEnemy == null)// Xác định kể thù gần nhất, nếu chưa biết
        {
            enemyOnMap = GameObject.FindObjectOfType<GameManager>().GetEnemyOnMap();
            targetEnemy = GetEnemyNearest();
        }
        else// xoay ( thay đổi Rotion ) theo hướng kẻ thù
        {

            if (Vector3.Distance(targetEnemy.transform.position, transform.position) > AttackRange)
            // Enemy vượt ngoài AttackRange ( nghĩa là enemy từ trong tầm bắn ra khỏi tầm bắn )
            {
                targetEnemy = null;

                if (HasAnimation)
                {
                    Anima_Model_Tower.SetBool("Attack", false);
                }

                SmoothNetRotation();
            }
            else// Xoay (Rotion) trụ
            {
                // Bắn đạn
                if (fireCoolDownLeft <= 0)
                {
                    fireCoolDownLeft = fireCoolDown;
                    ShootAt(targetEnemy.transform);

                    // Kích hoạt Animation của hành động Attack
                    if(HasAnimation)
                        Anima_Model_Tower.SetBool("Attack", true);
                }

                // Xoay (Rotion) trụ theo hướng enemy
                Vector3 direction = targetEnemy.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                targetRotation.x = 0;
                targetRotation.z = 0;
                Model_Tower.transform.rotation = Quaternion.Lerp(Model_Tower.transform.rotation, targetRotation, Time.deltaTime * 5);
            }
        }
        // Khac team
    }

    #region Photon
    private void OnPhotonSerializeview(PhotonStream stream, PhotonMessageInfo info)
    {
        //Để smoot rotation của Tower
        if (stream.IsWriting)
        {
            stream.SendNext(Model_Tower.transform.rotation);
        }
        else
        {
           preRotionToSmoth = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion

    void ShootAt(Transform enemy)
    {
        
        GameObject BulletGO = (GameObject)Instantiate(BulletFrefab,transform);
        if (IsSuperTower)
        {
            BulletSuperAOE b = BulletGO.GetComponent<BulletSuperAOE>();
            b.target = enemy;
            b.damage = AttacDame;
            b.radius = bulletExplosionRadius;

            GameObject gOn = new GameObject();
            gOn.transform.SetPositionAndRotation(PosCreateBullet.transform.position, PosCreateBullet.transform.rotation);
            BulletGO.transform.SetPositionAndRotation(gOn.transform.position, gOn.transform.rotation);
        }
        else
        {
            Bullet b = BulletGO.GetComponent<Bullet>();
            b.target = enemy;
            b.damage = AttacDame;
            b.radius = bulletExplosionRadius;
        }
    }

    private GameObject GetEnemyNearest()
    {
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemyOnMap.ToArray())
        {
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (nearestEnemy == null || (d < dist && d <= AttackRange))// Tìm kẻ địch trong tầm bắn gần nhất
            {
                nearestEnemy = enemy;
                dist = d;
            }
        }
        //Debug.Log("Got enemy nearest...!!!!!");
        return nearestEnemy;
    }

    private void SmoothNetRotation()
    {
        Model_Tower.transform.rotation = Quaternion.Lerp(Model_Tower.transform.rotation, preRotionToSmoth, Time.deltaTime * 5);
    }

}
