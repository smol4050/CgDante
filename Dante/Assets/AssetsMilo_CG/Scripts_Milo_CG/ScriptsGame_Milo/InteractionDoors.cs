using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el comportamiento de una puerta interactuable que puede abrirse y cerrarse.
/// Implementa la interfaz <see cref="IInteractuable"/>.
/// </summary>
public class InteractionDoors : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Indica si la puerta está abierta (true) o cerrada (false).
    /// </summary>
    public bool doorOpen;

    /// <summary>
    /// Ángulo en grados al que la puerta se abre.
    /// </summary>
    public float doorOpenAngle = 90f;

    /// <summary>
    /// Ángulo en grados al que la puerta se cierra.
    /// </summary>
    public float doorCloseAngle = 0f;

    /// <summary>
    /// Velocidad de interpolación para la animación de apertura/cierre.
    /// </summary>
    public float smooth = 2f;

    /// <summary>
    /// Índice único asignado manualmente en el Inspector, para identificar la puerta (0 a 3).
    /// </summary>
    public int indicePuerta;

    /// <summary>
    /// Estado estático compartido por todas las puertas, indicando si están abiertas o cerradas.
    /// El índice corresponde al <see cref="indicePuerta"/>.
    /// </summary>
    public static List<bool> EstadoPuertas = new List<bool>() { true, true, true, true };

    /// <summary>
    /// Inicializa la lista de estados para asegurar que tenga el tamaño suficiente y
    /// asigna el estado inicial de la puerta correspondiente.
    /// </summary>
    void Awake()
    {
        if (indicePuerta >= EstadoPuertas.Count)
        {
            for (int i = EstadoPuertas.Count; i <= indicePuerta; i++)
            {
                EstadoPuertas.Add(true);
            }
        }

        EstadoPuertas[indicePuerta] = doorOpen;
    }

    /// <summary>
    /// Método que activa la puerta: la cierra si está abierta, y la abre si está cerrada.
    /// Al cerrarla, se programará que se abra automáticamente después de 5 segundos.
    /// </summary>
    public void ActivarObjeto()
    {
        if (doorOpen)
        {
            doorOpen = false;
            Debug.Log("Cerrando puerta");
            EstadoPuertas[indicePuerta] = false;
            StartCoroutine(AbrirDespuesDeTiempo(5f));
        }
        else
        {
            doorOpen = true;
            Debug.Log("Abriendo puerta");
            EstadoPuertas[indicePuerta] = true;
        }
    }

    /// <summary>
    /// Corrutina que abre la puerta después de un retraso dado en segundos.
    /// </summary>
    /// <param name="segundos">Tiempo en segundos antes de abrir la puerta.</param>
    /// <returns>IEnumerator para la corrutina.</returns>
    IEnumerator AbrirDespuesDeTiempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        doorOpen = true;
        EstadoPuertas[indicePuerta] = true;
    }

    /// <summary>
    /// Actualiza la rotación de la puerta interpolando suavemente entre abierta y cerrada.
    /// </summary>
    void Update()
    {
        if (doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }

    /// <summary>
    /// Método estático que cierra todas las puertas registradas en <see cref="EstadoPuertas"/>.
    /// </summary>
    public static void CerrarTodasLasPuertas()
    {
        for (int i = 0; i < EstadoPuertas.Count; i++)
        {
            EstadoPuertas[i] = false; // false = cerrada
        }
    }
}

