using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el comportamiento de una puerta que puede abrirse mediante rotaci�n suave.
/// </summary>
public class PuertaController : MonoBehaviour
{
    /// <summary>
    /// Indica si la puerta est� abierta actualmente.
    /// </summary>
    private bool estaAbierta = false;

    /// <summary>
    /// Rotaci�n objetivo cuando la puerta est� completamente abierta.
    /// </summary>
    private Quaternion rotacionAbierta;

    /// <summary>
    /// Rotaci�n original de la puerta (cerrada).
    /// </summary>
    private Quaternion rotacionCerrada;

    /// <summary>
    /// Velocidad a la que se abre la puerta.
    /// </summary>
    [SerializeField] private float velocidadApertura = 2f;

    /// <summary>
    /// Al iniciar, guarda la rotaci�n cerrada e inicializa la rotaci�n abierta con un giro de 90 grados en Y.
    /// </summary>
    void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0)); // Gira 90 grados en el eje Y
    }

    /// <summary>
    /// En cada frame, interpola suavemente entre la rotaci�n actual y la deseada seg�n el estado de la puerta.
    /// </summary>
    void Update()
    {
        if (estaAbierta)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionAbierta, Time.deltaTime * velocidadApertura);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionCerrada, Time.deltaTime * velocidadApertura);
        }
    }

    /// <summary>
    /// M�todo p�blico para activar la apertura de la puerta.
    /// </summary>
    public void AbrirPuerta()
    {
        estaAbierta = true;
    }
}
