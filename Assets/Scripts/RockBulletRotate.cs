using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBulletRotate : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {
        Quaternion rotaCh = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 360);
        transform.GetChild(0).transform.rotation = rotaCh;
        transform.GetChild(1).transform.rotation = rotaCh;
        transform.GetChild(2).transform.rotation = rotaCh;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(-200 * Time.deltaTime*Vector3.left, Space.Self);
        transform.Rotate(200 * Time.deltaTime , 0, 0, Space.Self);
    }
}
