using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linterna : MonoBehaviour
{
    public Light LuzLinterna;

    
    void Start()
    {
        LuzLinterna.enabled = false; // Apagar la linterna al inicio
    }

    
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            // abrircerrar linterna
            LuzLinterna.enabled = !LuzLinterna.enabled;
        }
    }
}

