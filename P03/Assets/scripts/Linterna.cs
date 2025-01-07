using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linterna : MonoBehaviour
{
    public Light LuzLinterna;

    
    void Start()
    {
        LuzLinterna.enabled = false; // Llanterna apagada al començar
    }

    
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            // alternar estat llanterna
            LuzLinterna.enabled = !LuzLinterna.enabled;
        }
    }
}

