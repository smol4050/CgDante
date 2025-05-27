using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activa la transición de regreso desde la zona invertida a la zona original cuando el jugador entra en esta área.
/// Este script debe estar en un GameObject con un Collider configurado como trigger.
/// </summary>
public class ActivadorZonaInvertida : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente <see cref="TiempoZonaInversa"/> que controla la lógica de zona invertida y su retorno.
    /// </summary>
    public TiempoZonaInversa zonaInversa;

    /// <summary>
    /// Método que se ejecuta automáticamente cuando otro Collider entra en el trigger de este objeto.
    /// Si el objeto es el jugador, inicia la rutina para regresar a la zona original.
    /// </summary>
    /// <param name="other">El Collider que ha entrado en el trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(zonaInversa.VolverZonaOriginal());
        }
    }
}
