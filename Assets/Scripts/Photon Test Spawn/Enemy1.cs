using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Photon.Pun;

public class Enemy1 : MonoBehaviour
{
	public float speed = 3.5f;
	public float health = 5f;
	public int moneyReward = 1;
	GameManager1 gameMana;
	public bool addOnMap = false;// Dùng khi không khởi tạo enemy bằng code

	[System.NonSerialized]
	public List<Transform> List_Road_Of_goald ;

    //public GameObject gameObjSpot;
    public bool stopMove = false;
	public GameObject pathGO;
	Transform targetPathNode = null;
	int pathNodeIndex = 0;
	bool enemyOutMap = false;
	float timeCDColliderToMove = 0.5f;
	float curTimeToMove;

	private void Awake()
	{
		pathGO = GameObject.Find("RoadOfenemy");
		gameMana = GameObject.Find("GameManager").GetComponent<GameManager1>();
		curTimeToMove = Time.time;
	}

	void Start()
	{
		List_Road_Of_goald = new List<Transform>();

		// Test Spawn enemy for Photon Networking
		List_Road_Of_goald = new List<Transform>();
        for (int i = 0; i < pathGO.transform.childCount; i++)
        {
			List_Road_Of_goald.Add(pathGO.transform.GetChild(i).transform);
        }
    }
	

	void GetNextPathNode() 
	{
		if (pathNodeIndex < List_Road_Of_goald.Count)
		{
			targetPathNode = List_Road_Of_goald[pathNodeIndex];
			pathNodeIndex++;
		}
		else
		{
			targetPathNode = null;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (enemyOutMap)
        {
			Debug.Log("Enemy out map !!!!!!!!!");
			Die();
		}
		if (!stopMove)
        {
			//if (targetPathNode == null && pathNodeIndex <= pathGO.transform.childCount)
			if(targetPathNode==null && pathNodeIndex <= List_Road_Of_goald.Count)
			{
				GetNextPathNode();
				if (targetPathNode == null)
				{
					// We've run out of path!
					ReachedGoal();
					return;
				}
			}

			Vector3 dir = targetPathNode.position - this.transform.position;
			float distThisFrame = speed * Time.deltaTime;// Khoảng cách di chuyển trong một frame

			if (dir.magnitude <= distThisFrame)

			{
				// We reached the node
				targetPathNode = null;
			}
			else
			{
                // TODO: Consider ways to smooth this motion.
                // Move towards node
                transform.Translate(dir.normalized * distThisFrame, Space.World);// thay đổi vị trí ( di chuyển )

				// dùng để xoay
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
            }
		}
        else
        {
			if (Time.time - curTimeToMove > timeCDColliderToMove)
            {
				stopMove = false;
				curTimeToMove = Time.time;
			}
        }
    }

    void ReachedGoal() {
		gameMana.LoseLives();
		Die();
	}

    //public void TakeDamage(float damage) {
    //	health -= damage;
    //	if(health <= 0) {
    //		gameMana.EarnMoney (moneyReward);
    //		Die();
    //	}
    //}

    public void Die()
    {
        // TODO: Do this more safely!
        //GameObject.FindObjectOfType<GameManager1>().DeleteEnemy(this.gameObject);
        Destroy(gameObject);
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //	if (collision.gameObject.CompareTag("Front Collision"))
    //       {

    //	}
    //}
    private void OnTriggerEnter(Collider other)
    {
		if (!other.CompareTag("Enemy"))
			return;
		stopMove = true;
		//Debug.Log("On Front Collision on Enemy");
	}
}
