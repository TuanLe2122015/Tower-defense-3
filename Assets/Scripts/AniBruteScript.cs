using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniBruteScript : MonoBehaviour
{
    public Animator aniBrute;

    private void Awake()
    {
        aniBrute = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attacking()
    {
        aniBrute.SetBool("Attack", true);
    }

    public void Idle()
    {
        aniBrute.SetBool("Attack", false);
    }
}
