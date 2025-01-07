using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Codigo_salud : MonoBehaviour
{
    public float Salud = 100;
    public float SaludMaxima = 100;

    [Header("Interfaz")]
    public Image BarraSalud;
    public Text TextoSalud;
    public CanvasGroup OjosRojos;

    [Header("Muerto")]
    public GameObject Muerto;

    [Header("Sonido")]
    public AudioClip sonidoDa�o; 
    private AudioSource audioSource; 

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        audioSource.playOnAwake = false;
    }

    
    void Update()
    {
        if (OjosRojos.alpha > 0)
        {
            OjosRojos.alpha -= Time.deltaTime;
        }
        ActualizarInterfaz();
    }

    public void RecibirDa�o(float da�o)
    {
        Salud -= da�o;
        OjosRojos.alpha = 1;

        
        if (sonidoDa�o != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDa�o);
        }

        if (Salud <= 0)
        {
            Instantiate(Muerto);
            Destroy(gameObject);
        }
    }

    void ActualizarInterfaz()
    {
        BarraSalud.fillAmount = Salud / SaludMaxima;
        TextoSalud.text = "HP: " + Salud.ToString("f0");
    }
}


