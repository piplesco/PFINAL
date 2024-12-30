using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    public CharacterController Controlador;
    public Transform Camara;
    public float Velocidad = 15f;
    public float AlturaSalto = 2f;
    public float Gravedad = -9.81f;

    private Vector3 velocidad;
    private bool enElSuelo;

    public float VelocidadMirada = 100f;
    private float RotacionX = 0f;
    private float RotacionY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento del personaje
        enElSuelo = Controlador.isGrounded;

        if (enElSuelo && velocidad.y < 0)
        {
            velocidad.y = -2f; // Mantener al personaje pegado al suelo
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Movimiento basado en la cámara
        Vector3 adelante = Camara.forward;
        Vector3 derecha = Camara.right;

        adelante.y = 0f;
        derecha.y = 0f;

        Vector3 mover = (derecha * x + adelante * z).normalized * Velocidad;
        Controlador.Move(mover * Time.deltaTime);

        // Salto
        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            velocidad.y = Mathf.Sqrt(AlturaSalto * -2f * Gravedad);
        }

        // Aplicar gravedad
        velocidad.y += Gravedad * Time.deltaTime;

        // Aplicar movimiento vertical
        Controlador.Move(velocidad * Time.deltaTime);

        // Movimiento de la cámara
        float MauseX = Input.GetAxis("Mouse X") * VelocidadMirada * Time.deltaTime;
        float MauseY = Input.GetAxis("Mouse Y") * VelocidadMirada * Time.deltaTime;

        // Rotación vertical
        RotacionX -= MauseY;
        RotacionX = Mathf.Clamp(RotacionX, -90f, 90f);

        // Rotación horizontal
        RotacionY += MauseX;

        // Aplicar las rotaciones
        Camara.localRotation = Quaternion.Euler(RotacionX, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, RotacionY, 0f);
    }
}

