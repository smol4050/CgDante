using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reproduce sonidos específicos al entrar o salir de un trigger relacionado con una puerta.
/// </summary>
public class SonidosPuerta : MonoBehaviour
{
    /// <summary>
    /// Clip de audio que se reproduce cuando la puerta se abre.
    /// </summary>
    public AudioClip openDoor;

    /// <summary>
    /// Clip de audio que se reproduce cuando la puerta se cierra.
    /// </summary>
    public AudioClip closeDoor;

    /// <summary>
    /// Método llamado cuando un collider entra en el trigger de este objeto.
    /// Si el collider tiene la etiqueta "TriggerDoor", se reproduce el sonido de cerrar puerta.
    /// </summary>
    /// <param name="other">El collider que entró en el trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerDoor"))
        {
            AudioSource.PlayClipAtPoint(closeDoor, transform.position, 100);
            Debug.Log("Puerta cerrada");
        }
    }

    /// <summary>
    /// Método llamado cuando un collider sale del trigger de este objeto.
    /// Si el collider tiene la etiqueta "TriggerDoor", se reproduce el sonido de abrir puerta.
    /// </summary>
    /// <param name="other">El collider que salió del trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerDoor"))
        {
            AudioSource.PlayClipAtPoint(openDoor, transform.position, 100);
            Debug.Log("Puerta abierta");
        }
    }
}
