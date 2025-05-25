using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controla la animaci�n del panel de introducci�n mostrando el mensaje letra por letra
/// con un sonido de tipeo y ocultando el panel despu�s de un tiempo.
/// </summary>
public class PanelIntro : MonoBehaviour
{
    /// <summary>
    /// Panel que contiene el mensaje de introducci�n.
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// Componente TextMeshProUGUI donde se escribe el mensaje letra por letra.
    /// </summary>
    public TextMeshProUGUI mensajeTexto;

    /// <summary>
    /// Tiempo entre la aparici�n de cada letra del mensaje.
    /// </summary>
    public float tiempoEntreLetras = 0.05f;

    /// <summary>
    /// Tiempo total que se muestra el panel despu�s de completar el mensaje.
    /// </summary>
    public float duracionPanel = 7f;

    /// <summary>
    /// Fuente de audio para reproducir el sonido mientras se escriben letras.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip de sonido que se reproduce en loop durante la escritura del mensaje.
    /// </summary>
    public AudioClip sonidoLetraLoop;

    /// <summary>
    /// Inicia la secuencia de animaci�n del mensaje.
    /// </summary>
    void Start()
    {
        StartCoroutine(MostrarMensajeConAnimacion());
    }

    /// <summary>
    /// Corrutina que muestra el mensaje letra por letra, reproduce el sonido,
    /// y oculta el panel al finalizar.
    /// </summary>
    IEnumerator MostrarMensajeConAnimacion()
    {
        // Activar el panel
        panel.SetActive(true);

        // Iniciar sonido de letras
        if (audioSource != null && sonidoLetraLoop != null)
        {
            audioSource.clip = sonidoLetraLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Mostrar mensaje letra por letra
        string mensajeCompleto = mensajeTexto.text;
        mensajeTexto.text = "";

        for (int i = 0; i < mensajeCompleto.Length; i++)
        {
            mensajeTexto.text += mensajeCompleto[i];
            yield return new WaitForSeconds(tiempoEntreLetras);
        }

        // Detener el sonido al terminar
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Esperar tiempo final y ocultar panel
        yield return new WaitForSeconds(duracionPanel);
        panel.SetActive(false);
    }
}
