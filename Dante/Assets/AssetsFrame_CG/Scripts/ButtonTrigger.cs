using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dispara la rutina de memoria en el <see cref="PathManager"/>
/// cuando el jugador pisa este trigger.
/// </summary>
public class ButtonTrigger : MonoBehaviour
{
    /// <summary>Referencia al PathManager a usar.</summary>
    public PathManager pathManager;

    /// <summary>C�mara FPS de este nivel.</summary>
    public Camera fpsCamThisButton;
    /// <summary>C�mara de secuencia de este bot�n.</summary>
    public Camera sequenceCamThisButton;

    /// <summary>
    /// Al detectar el jugador, asigna las c�maras al PathManager
    /// y arranca la secuencia de memoria.
    /// </summary>
    /// <param name="other">Collider del jugador (tag "Player").</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pathManager.fpsCamera = fpsCamThisButton;
            pathManager.sequenceCamera = sequenceCamThisButton;
            pathManager.StartMemorySequence();
        }
    }
}
