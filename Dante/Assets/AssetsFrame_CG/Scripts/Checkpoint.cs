using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un punto de control que, al ser atravesado por el jugador,
/// actualiza la posici�n de respawn en <see cref="CheckpointManager"/>.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Se ejecuta en edici�n para asegurarse de que el collider sea trigger.
    /// </summary>
    private void Reset()
    {
        // Asegura que sea trigger
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Detecta la colisi�n con el jugador y avisa al
    /// <see cref="CheckpointManager"/> para actualizar el checkpoint.
    /// </summary>
    /// <param name="other">El collider que entra; debe llevar tag "Player".</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // El jugador pas� por aqu�: actualizamos el checkpoint
        CheckpointManager.Instance.SetCheckpoint(transform);
    }
}
