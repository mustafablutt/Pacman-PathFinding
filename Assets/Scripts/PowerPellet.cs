using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    private int score = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddPellet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.ReducePellet(score);
            GameManager.instance.frigthened = true;
            Destroy(gameObject);
        }
    }
}
