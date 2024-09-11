using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSuperAOE : MonoBehaviour
{
    public float speed = 0.2f;
    public Transform target;

    public float damage;

    public float radius;
    public float timeDestroy;

    public float timeInfDame;
    float infDameCD;

    Vector3 dirTarget;
    Vector3 dirTargetTop;

    float hightX = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        // Xác định hướng giữa đạn và mục tiêu
        dirTarget = new Vector3( target.position.x,hightX, target.position.z) - new Vector3( transform.position.x, hightX, transform.position.z);
        dirTargetTop = new Vector3(transform.position.x, hightX, transform.position.z) - transform.position;
        //dirTargetTop += dirTarget;

        infDameCD = timeInfDame;
    }

    // Update is called once per frame
    void Update()
    {
        timeDestroy -= Time.deltaTime;
        if (timeDestroy <= 0)
        {
            Destroy(gameObject);
        }

        // Khoảng cách đạn di chuyển được trong 1 frame
        float distThisFrame = speed * Time.deltaTime;

        //Gây sát thương cứ mỗi <timeInfDame> giây
        infDameCD -= Time.deltaTime;
        if (infDameCD <= 0)
        {
            BulletHit();// Đạn gây dame cho enemy
            infDameCD = timeInfDame;
        }

        if (transform.position.y > hightX)
        {
            // Đạn rơi từ từ theo đường cong đến đụng đất thì thôi
            transform.Translate(dirTargetTop.normalized * distThisFrame, Space.World);//Di chuyển xuống
            transform.Translate(dirTarget.normalized * distThisFrame, Space.World);// Kết hợp thêm di chuyển thẳng, cùng lúc
        }
        else
            transform.Translate(dirTarget.normalized * distThisFrame, Space.World);//Khi đạn đụng đất di chuyển theo đường thẳng
    }

    void BulletHit()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in col)
        {
            Enemy d = c.GetComponent<Enemy>();
            if (d != null)
                d.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
