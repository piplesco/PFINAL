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
    public AudioClip sonidoDa�o; // El sonido que se reproducir� al recibir da�o
    private AudioSource audioSource; // Componente para reproducir el sonido

    void Start()
    {
        // Aseg�rate de que el objeto tenga un AudioSource para reproducir sonidos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Evitar que el sonido se reproduzca autom�ticamente
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
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

        // Reproducir el sonido de da�o
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


