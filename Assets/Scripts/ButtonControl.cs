using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objSpot;
    public GameObject bottomPanel;
    public GameObject curCamera;

    public bool IsPurchasing = false;
    void Start()
    {
        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(4).gameObject.SetActive(true);
        //bottomPanel.transform.GetChild(5).gameObject.SetActive(true);
    }
    public void ButtonPurchaseTower()// Khi click button build tower
    {
        IsPurchasing = true;

        bottomPanel.transform.GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(1).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(2).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(3).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(4).gameObject.SetActive(false);
        //bottomPanel.transform.GetChild(5).gameObject.SetActive(false);

        for (int i = 0; i < objSpot.transform.childCount; i++)
        {
            //objSpot.transform.GetChild(i).gameObject.SetActive(true); // hiện lại Tower Spot

            if (    Mathf.Abs(  curCamera.transform.position.z + 14 - 
                                objSpot.transform.GetChild(i).gameObject.transform.position.z
                             )
                                    < 9
               )
            {
                objSpot.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ButtonFinishBuildingTower()// Khi click Button Finish Building
    {
        IsPurchasing = false;

        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(4).gameObject.SetActive(true);
        //bottomPanel.transform.GetChild(5).gameObject.SetActive(true);

        for (int i = 0; i < objSpot.transform.childCount; i++)
        {
            objSpot.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //public void ButtonBuildTower()
    //{
    //    bottomPanel.transform.GetChild(4).gameObject.SetActive(false);
    //    bottomPanel.transform.GetChild(5).gameObject.SetActive(true);
    //}
}
