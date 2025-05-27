using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Representa una puerta que permite cambiar a otra escena al interactuar con ella.
/// </summary>
public class PuertaCambioEscena : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Nombre de la escena a la que se cambiará al activar la puerta.
    /// </summary>
    public string nombreEscena;

    /// <summary>
    /// Método que se llama al interactuar con la puerta.
    /// Completa el nivel actual y carga la escena destino.
    /// </summary>
    public void ActivarObjeto()
    {
        GameManager.Instance.CompletarNivel();
        SceneManager.LoadScene(nombreEscena); // Asegúrate que el nombre esté bien escrito
    }
}
