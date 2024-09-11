using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public float speed = 3.5f;
	public float health = 5f;
	public int moneyReward = 1;
	GameManager gameMana;
	public bool addOnMap = false;// Dùng khi không khởi tạo enemy bằng code

	public List<Transform> List_Path_Of_goald ;
    public GameObject gameObjSpot;
    public bool stopMove = false;
	public GameObject pathGO;
	Transform targetPathNode = null;
	int pathNodeIndex = 0;
	bool enemyOutMap = false;
	float timeCDColliderToMove = 0.5f;
	float curTimeToMove;
	void Start()
	{
        List_Path_Of_goald = new List<Transform>();
        gameObjSpot.GetComponent<ShortestWay>().BestWayForEnemy(transform, ref List_Path_Of_goald, ref enemyOutMap);
        if (addOnMap)
            gameMana.AddEnenemyToMap(gameObject);
        InvokeRepeating("FindTheBestWay", 1f, 0.8f);



        // Test Spawn enemy for Photon Networking
  //      List_Path_Of_goald = new List<Transform>();
		//for (int i = 0; i < pathGO.transform.childCount; i++)
  //      {
		//	List_Path_Of_goald.Add(pathGO.transform.GetChild(i).transform);
		//}
	}
	void FindTheBestWay()
    {
		stopMove = true;
		pathNodeIndex = 0;
		targetPathNode = null;
        gameObjSpot.GetComponent<ShortestWay>().BestWayForEnemy(transform, ref List_Path_Of_goald, ref enemyOutMap);
        stopMove = false;
	}
	private void Awake()
	{
		pathGO = GameObject.Find("RoadOfenemy");
		gameMana = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObjSpot = GameObject.Find("spot");
        curTimeToMove = Time.time;
	}

	public void GetPower(float value)
	{
		health *= value;
	}
	void GetNextPathNode() {
		//if (pathNodeIndex < pathGO.transform.childCount) {
		//	targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
		//	pathNodeIndex++;
		//}
		//else {
		//	targetPathNode = null;
		//}

		if (pathNodeIndex < List_Path_Of_goald.Count)
		{
			targetPathNode = List_Path_Of_goald[pathNodeIndex];
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
			TakeDamage(9999999999);
		}
		if (!stopMove)
        {
			//if (targetPathNode == null && pathNodeIndex <= pathGO.transform.childCount)
			if(targetPathNode==null && pathNodeIndex <= List_Path_Of_goald.Count)
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

	public void TakeDamage(float damage) {
		health -= damage;
		if(health <= 0) {
			gameMana.EarnMoney ( moneyReward);
			Die();
		}
	}

	public void Die() {
		// TODO: Do this more safely!
		GameObject.FindObjectOfType<GameManager>().DeleteEnemy(this.gameObject);
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
