using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public GameObject waves;
    // Start is called before the first frame update
    GameObject curWave = null;
    void Start()
    {
        curWave = Instantiate(waves, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (curWave==null)
        {
            curWave = Instantiate(waves, transform);
        }
    }
}