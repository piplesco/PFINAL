using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    public CharacterController Controlador;
    public Transform Camara;
    public float Velocidad = 8f;
    public float VelocidadCorrer = 13f;
    public float AlturaSalto = 2f;
    public float Gravedad = -9.81f;

    private Vector3 velocidad;
    private bool enElSuelo;

    public float VelocidadMirada = 100f;
    private float RotacionX = 0f;
    private float RotacionY = 0f;

    [Header("Sonidos de Movimiento")]
    public AudioSource audioSource;
    public AudioClip pasosCorrer;
    public AudioClip pasosCaminar;

    public float volumenPasos = 1.2f; 

    private bool corriendo = false;
    private bool caminando = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.playOnAwake = false; 
    }

    void Update()
    {
        // Movimient
        enElSuelo = Controlador.isGrounded;

        if (enElSuelo && velocidad.y < 0)
        {
            velocidad.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float velocidadActual = Input.GetKey(KeyCode.LeftShift) ? VelocidadCorrer : Velocidad;

        Vector3 adelante = Camara.forward;
        Vector3 derecha = Camara.right;

        adelante.y = 0f;
        derecha.y = 0f;

        Vector3 mover = (derecha * x + adelante * z).normalized * velocidadActual;
        Controlador.Move(mover * Time.deltaTime);

        // Salto
        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            velocidad.y = Mathf.Sqrt(AlturaSalto * -2f * Gravedad);
        }

        // Gravedad
        velocidad.y += Gravedad * Time.deltaTime;
        Controlador.Move(velocidad * Time.deltaTime);

        // Movimiento del mouse
        float MauseX = Input.GetAxis("Mouse X") * VelocidadMirada * Time.deltaTime;
        float MauseY = Input.GetAxis("Mouse Y") * VelocidadMirada * Time.deltaTime;

        RotacionX -= MauseY;
        RotacionX = Mathf.Clamp(RotacionX, -90f, 90f);

        RotacionY += MauseX;

        Camara.localRotation = Quaternion.Euler(RotacionX, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, RotacionY, 0f);

        
        ReproducirSonidosMovimiento(mover);
    }

    void ReproducirSonidosMovimiento(Vector3 mover)
    {
        if (mover.magnitude > 0)
        {
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                corriendo = true;
                caminando = false;

                
                if (!audioSource.isPlaying || audioSource.clip != pasosCorrer)
                {
                    audioSource.clip = pasosCorrer;
                    audioSource.volume = volumenPasos; 
                    audioSource.Play();
                }
            }
            else
            {
                corriendo = false;
                caminando = true;

                
                if (!audioSource.isPlaying || audioSource.clip != pasosCaminar)
                {
                    audioSource.clip = pasosCaminar;
                    audioSource.volume = volumenPasos; 
                    audioSource.Play();
                }
            }
        }
        else
        {
            
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}



