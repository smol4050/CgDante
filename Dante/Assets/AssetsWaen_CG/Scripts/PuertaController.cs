using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el comportamiento de una puerta que puede abrirse mediante rotación suave.
/// </summary>
public class PuertaController : MonoBehaviour
{
    /// <summary>
    /// Indica si la puerta está abierta actualmente.
    /// </summary>
    private bool estaAbierta = false;

    /// <summary>
    /// Rotación objetivo cuando la puerta está completamente abierta.
    /// </summary>
    private Quaternion rotacionAbierta;

    /// <summary>
    /// Rotación original de la puerta (cerrada).
    /// </summary>
    private Quaternion rotacionCerrada;

    /// <summary>
    /// Velocidad a la que se abre la puerta.
    /// </summary>
    [SerializeField] private float velocidadApertura = 2f;

    /// <summary>
    /// Al iniciar, guarda la rotación cerrada e inicializa la rotación abierta con un giro de 90 grados en Y.
    /// </summary>
    void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0)); // Gira 90 grados en el eje Y
    }

    /// <summary>
    /// En cada frame, interpola suavemente entre la rotación actual y la deseada según el estado de la puerta.
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
    /// Método público para activar la apertura de la puerta.
    /// </summary>
    public void AbrirPuerta()
    {
        estaAbierta = true;
    }
}
