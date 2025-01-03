using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacerDaño : MonoBehaviour
{
   public float CantidadDaño;

   private void OnTriggerEnter(Collider other){
   
   if(other.CompareTag("Player") && other.GetComponent<Codigo_salud>()){
   
    other.GetComponent<Codigo_salud>().RecibirDaño(CantidadDaño);
   }
   }

}
