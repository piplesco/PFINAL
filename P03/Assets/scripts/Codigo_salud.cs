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
    public AudioClip sonidoDaño; // El sonido que se reproducirá al recibir daño
    private AudioSource audioSource; // Componente para reproducir el sonido

    void Start()
    {
        // Asegúrate de que el objeto tenga un AudioSource para reproducir sonidos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Evitar que el sonido se reproduzca automáticamente
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

    public void RecibirDaño(float daño)
    {
        Salud -= daño;
        OjosRojos.alpha = 1;

        // Reproducir el sonido de daño
        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
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


