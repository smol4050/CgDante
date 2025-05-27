using System.Collections;
using UnityEngine;

/// <summary>
/// Reproduce en bucle un mensaje de audio en c�digo Morse, con un retraso configurable entre repeticiones.
/// </summary>
public class MorseAudioLooper : MonoBehaviour
{
    /// <summary>
    /// Clips de audio que contienen mensajes en c�digo Morse.
    /// </summary>
    [Header("Audio Clips con mensajes en c�digo Morse")]
    public AudioClip[] morseAudios;

    /// <summary>
    /// �ndice del mensaje seleccionado para reproducir desde el arreglo <see cref="morseAudios"/>.
    /// </summary>
    [Tooltip("Selecciona el �ndice del audio a reproducir")]
    public int mensajeSeleccionado = 0;

    /// <summary>
    /// Tiempo de espera en segundos entre cada reproducci�n del mensaje.
    /// </summary>
    [Tooltip("Retraso entre cada reproducci�n (en segundos)")]
    public float delayEntreRepeticiones = 2f;

    /// <summary>
    /// Componente <see cref="AudioSource"/> que reproducir� los clips de audio.
    /// </summary>
    [Header("AudioSource que reproducir� los clips")]
    public AudioSource audioSource;

    /// <summary>
    /// Inicia la reproducci�n en bucle del mensaje Morse seleccionado, si los par�metros son v�lidos.
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
            Debug.LogError("�ndice de mensaje fuera de rango.");
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

            // Esperar la duraci�n del audio m�s el retraso especificado
            yield return new WaitForSeconds(clip.length + delayEntreRepeticiones);
        }
    }
}
