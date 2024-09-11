using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;

    public float damage = 2;
    public float radius = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) // Nếu không có mục tiêu thì hủy đạn
        {
            // Khoảng cách đạn di chuyển được trong 1 frame
            float distThisFrame = speed * Time.deltaTime;
            // Hướng giữa đạn và mục tiêu
            Vector3 dirTarget = target.position - transform.position;

            if (dirTarget.magnitude <= distThisFrame)//Nếu đạn chạm đến mục tiêu, đạn sẽ nổ
            {
                BulletHit();
            }
            else// Còn nếu đạn chưa chạm đến mục tiêu thì tiếp tục di chuyển hướng đến mục tiêu
            {
                //Di chuyển vị trí ( thay đổi Poistion ) đạn đến mục tiêu
                transform.Translate(dirTarget.normalized * distThisFrame, Space.World);

                // Xoay ( Thay đổi Rotation ) để hướng đạn đến mục tiêu
                if (dirTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(dirTarget);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void BulletHit()
    {
        if(radius==0)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
        else
        {
            Collider[] col = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in col)
            {
                Enemy d = c.GetComponent<Enemy>();
                if (d != null)
                    d.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}