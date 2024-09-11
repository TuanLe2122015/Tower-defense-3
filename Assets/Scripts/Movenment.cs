using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movenment : MonoBehaviour
{
    float speedCamera = 10f;
    protected Joystick joystick;

    // để kiểm tra camera di chuyển
    float thresholdChange = 0.05f;
    private Vector3 lastTransformPos;

    public GameObject objSpot;

    public GameObject buttonBuilding;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(
            (speedCamera*joystick.Horizontal) * Time.deltaTime,
            0,
            (speedCamera*joystick.Vertical) * Time.deltaTime);

        // để kiểm tra camera di chuyển
        if (!buttonBuilding.GetComponent<ButtonControl>().IsPurchasing) // Nếu đang ở chế độ xây Tower
            return;
        var diffVector = transform.position - lastTransformPos;
        if (diffVector.magnitude >= thresholdChange)
        {
            lastTransformPos = transform.position;

            // chỉ thao tác với các Spot trong tầm nhìn của camera, dùng để tránh click button ảnh hưởng đến Spot
            for (int i = 0; i < objSpot.transform.childCount; i++)
            {
                objSpot.transform.GetChild(i).gameObject.SetActive(false);

                if (Mathf.Abs(transform.position.z + 21 -
                              objSpot.transform.GetChild(i).gameObject.transform.position.z
                             )
                                 < 5
                   )
                    objSpot.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
