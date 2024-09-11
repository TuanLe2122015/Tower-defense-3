using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager1 : MonoBehaviour
{
    public GameObject waves;
    // Start is called before the first frame update
    GameObject curWave = null;
    

    // Update is called once per frame
    void Update()
    {
        if (curWave==null)
        {
            curWave = Instantiate(waves, this.transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}