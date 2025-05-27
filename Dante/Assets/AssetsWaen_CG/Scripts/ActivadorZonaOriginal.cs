using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activa el contador de tiempo de la zona inversa cuando el jugador entra en el área.
/// Este script debe estar en un GameObject con un Collider configurado como trigger.
/// </summary>
public class ActivadorZonaOriginal : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente <see cref="TiempoZonaInversa"/> que controla la lógica de zona invertida.
    /// </summary>
    public TiempoZonaInversa zonaInversa;

    /// <summary>
    /// Método que se ejecuta automáticamente cuando otro Collider entra en el trigger de este objeto.
    /// Si el objeto es el jugador, activa el contador de tiempo.
    /// </summary>
    /// <param name="other">El Collider que ha entrado en el trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zonaInversa.ActivarContador();
        }
    }
}
