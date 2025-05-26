using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja la navegación del menú, carga de escenas y efectos de sonido para botones.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Nombre de la escena destino para cargar.
    /// </summary>
    public string nombreEscenaDestino;

    /// <summary>
    /// Sonido que se reproduce al presionar un botón.
    /// </summary>
    public AudioClip sonidoBoton;

    /// <summary>
    /// Detecta la pulsación de la tecla espacio para cargar el siguiente nivel no completado.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string siguienteNivel = GameManager.Instance.ObtenerSiguienteNivelNoCompletado();
            if (!string.IsNullOrEmpty(siguienteNivel))
            {
                SceneManager.LoadScene(siguienteNivel);
            }
            else
            {
                Debug.Log("No hay más niveles por jugar.");
                // Puedes cargar una escena de créditos o volver al menú
            }
        }
    }

    /// <summary>
    /// Cambia la escena usando el método del GameManager.
    /// </summary>
    /// <param name="EscenaCambio">Nombre de la escena a cargar.</param>
    public void CargarEscena(string EscenaCambio)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CambiarEscena(EscenaCambio);
        }
    }

    /// <summary>
    /// Cambia la escena al inicio con el nombre especificado.
    /// </summary>
    /// <param name="nombreEscenaDestino">Nombre de la escena destino.</param>
    public void CambiarEscenaInicio(string nombreEscenaDestino)
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena a cargar.");
        }
    }

    /// <summary>
    /// Reproduce el sonido asignado al botón en la posición de la cámara principal.
    /// </summary>
    public void ButtonSonido()
    {
        AudioSource.PlayClipAtPoint(sonidoBoton, Camera.main.transform.position, 50f);
    }

    /// <summary>
    /// Inicia una nueva partida borrando el progreso guardado, inicializando el progreso y cargando la escena especificada.
    /// </summary>
    /// <param name="rafa">Nombre de la escena a cargar para la nueva partida.</param>
    public void NuevaPartida(string rafa)
    {
        JsonGuardado.BorrarProgreso(); // Borra el progreso guardado
        GameManager.Instance.InicializarProgreso();
        SceneManager.LoadScene(rafa);
    }
}
