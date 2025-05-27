using System.Collections;
using UnityEngine;

/// <summary>
/// Reproduce en bucle un mensaje de audio en código Morse, con un retraso configurable entre repeticiones.
/// </summary>
public class MorseAudioLooper : MonoBehaviour
{
    /// <summary>
    /// Clips de audio que contienen mensajes en código Morse.
    /// </summary>
    [Header("Audio Clips con mensajes en código Morse")]
    public AudioClip[] morseAudios;

    /// <summary>
    /// Índice del mensaje seleccionado para reproducir desde el arreglo <see cref="morseAudios"/>.
    /// </summary>
    [Tooltip("Selecciona el índice del audio a reproducir")]
    public int mensajeSeleccionado = 0;

    /// <summary>
    /// Tiempo de espera en segundos entre cada reproducción del mensaje.
    /// </summary>
    [Tooltip("Retraso entre cada reproducción (en segundos)")]
    public float delayEntreRepeticiones = 2f;

    /// <summary>
    /// Componente <see cref="AudioSource"/> que reproducirá los clips de audio.
    /// </summary>
    [Header("AudioSource que reproducirá los clips")]
    public AudioSource audioSource;

    /// <summary>
    /// Inicia la reproducción en bucle del mensaje Morse seleccionado, si los parámetros son válidos.
    /// </summary>
    void Start()
    {
        if (morseAudios == null || morseAudios.Length == 0)
        {
            Debug.LogError("No se asignaron audios Morse.");
            return;
        }

        if (mensajeSeleccionado < 0 || mensajeSeleccionado >= morseAudios.Length)
        {
            Debug.LogError("Índice de mensaje fuera de rango.");
            return;
        }

        StartCoroutine(ReproducirMorse());
    }

    /// <summary>
    /// Corrutina que reproduce el mensaje Morse seleccionado en bucle, con un retraso entre repeticiones.
    /// </summary>
    /// <returns>Objeto <see cref="IEnumerator"/> para la corrutina.</returns>
    IEnumerator ReproducirMorse()
    {
        while (true)
        {
            AudioClip clip = morseAudios[mensajeSeleccionado];

            audioSource.PlayOneShot(clip);

            // Esperar la duración del audio más el retraso especificado
            yield return new WaitForSeconds(clip.length + delayEntreRepeticiones);
        }
    }
}
