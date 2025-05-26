using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la animaci�n de apertura de una puerta final roja.
/// </summary>
public class PuertaFinalRoja1 : MonoBehaviour
{
    /// <summary>
    /// �ngulo al que la puerta se abre en grados.
    /// </summary>
    public float doorOpenAngle = 90f;

    /// <summary>
    /// Velocidad suave de interpolaci�n para la apertura de la puerta.
    /// </summary>
    public float smooth = 2f;

    /// <summary>
    /// Indica si la puerta est� abierta o cerrada.
    /// </summary>
    private bool doorOpen = false;

    /// <summary>
    /// M�todo para iniciar la apertura de la puerta.
    /// Cambia el estado de la puerta a abierta y registra un mensaje en consola.
    /// </summary>
    public void AbrirPuerta()
    {
        if (!doorOpen)
        {
            doorOpen = true;
            Debug.Log("�Puerta final abri�ndose!");
        }
    }

    /// <summary>
    /// Actualiza la rotaci�n de la puerta para animar la apertura.
    /// </summary>
    void Update()
    {
        if (doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }
}
