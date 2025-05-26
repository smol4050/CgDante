using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja la navegaci�n del men�, carga de escenas y efectos de sonido para botones.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Nombre de la escena destino para cargar.
    /// </summary>
    public string nombreEscenaDestino;

    /// <summary>
    /// Sonido que se reproduce al presionar un bot�n.
    /// </summary>
    public AudioClip sonidoBoton;

    /// <summary>
    /// Detecta la pulsaci�n de la tecla espacio para cargar el siguiente nivel no completado.
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
                Debug.Log("No hay m�s niveles por jugar.");
                // Puedes cargar una escena de cr�ditos o volver al men�
            }
        }
    }

    /// <summary>
    /// Cambia la escena usando el m�todo del GameManager.
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
    /// Reproduce el sonido asignado al bot�n en la posici�n de la c�mara principal.
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
