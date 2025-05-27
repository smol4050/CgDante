using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Cambia automáticamente la escena cuando el jugador entra en un área determinada (trigger).
/// Este script debe colocarse en un GameObject con un <see cref="Collider"/> marcado como "Is Trigger".
/// </summary>
public class cambiarEscenaAlEntrar : MonoBehaviour
{
    /// <summary>
    /// Nombre de la escena que se cargará al entrar al trigger. Debe coincidir exactamente con el nombre en la configuración de escenas.
    /// </summary>
    public string nombreEscena;

    /// <summary>
    /// Método llamado automáticamente por Unity cuando otro <see cref="Collider"/> entra en el trigger.
    /// Si el objeto es el jugador, se completa el nivel actual y se carga la escena especificada.
    /// </summary>
    /// <param name="other">El collider que ha entrado en el área de activación.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CompletarNivel();
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
