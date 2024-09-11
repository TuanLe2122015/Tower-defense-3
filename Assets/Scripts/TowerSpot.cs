using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//public class TowerSpot : Photon.Pun.MonoBehaviourPun    
public class TowerSpot : MonoBehaviour    
{
    public bool activeButtonBuyTower;
    public bool IsChosing;
    static GameObject preSpot;

    GameObject gameManager;

    // màu của spot
    // R 68 - G 255 - B 0 - A 190

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }
    void Start()
    {
        
    }

    private void OnMouseUp()
    {
        // ẩn spot được chọn trước đó
        if (preSpot != null)
        {
            preSpot.GetComponent<MeshRenderer>().enabled = false;
            preSpot.GetComponent<TowerSpot>().IsChosing = false;
        }
        
        // hiện Spot được chọn hiện tại
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        IsChosing = true;

        gameManager.GetComponent<GameManager>().spotSelected = gameObject;
        preSpot = gameObject;
    }
    public void BuildTower(GameObject TowerSelect)
    {
        //Tạo Tower mua được
        //PhotonNetwork.Instantiate(TowerSelect.name, transform.position, transform.rotation);

        Instantiate(TowerSelect, transform.position, transform.rotation);
        //Cập nhật lại ma trận liền kề
        transform.GetComponentInParent<ShortestWay>().adjustMatrixWhenBuyTower(transform);

        Destroy(gameObject);
    }
}
