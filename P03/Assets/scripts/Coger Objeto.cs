using System.Collections;
using UnityEngine;
using TMPro; 

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint; 
    private GameObject pickedObject = null; 

    
    public TMP_Text contadorTexto;
    private int objetosEntregados = 0; 
    public int totalObjetos = 3; 

    [Header("Pantalla de Ganar")]
    public GameObject PantallaGanar; 
    public float retrasoPantalla = 1.0f; 

    [Header("Sonido")]
    public AudioSource audioSource; 
    public AudioClip sonidoMoneda; 

    void Update()
    {
        
        if (pickedObject != null && Input.GetKey("r"))
        {
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.transform.SetParent(null);
            pickedObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Objeto") && Input.GetKey("e") && pickedObject == null)
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.position = handPoint.transform.position;
            other.transform.SetParent(handPoint.transform);
            pickedObject = other.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Baul") && pickedObject != null)
        {
            
            objetosEntregados++;
            ActualizarContador();

            
            if (audioSource != null && sonidoMoneda != null)
            {
                audioSource.PlayOneShot(sonidoMoneda);
            }

            
            Destroy(pickedObject);
            pickedObject = null;

            
            if (objetosEntregados >= totalObjetos)
            {
                StartCoroutine(MostrarPantallaGanarConRetraso());
            }
        }
    }

    
    private IEnumerator MostrarPantallaGanarConRetraso()
    {
        yield return new WaitForSeconds(retrasoPantalla);

        if (PantallaGanar != null)
        {
            Instantiate(PantallaGanar);
        }
    }

    
    private void ActualizarContador()
    {
        
        contadorTexto.text = "OBJETOS SAQUEADOS: " + objetosEntregados + "/" + totalObjetos;
    }
}

