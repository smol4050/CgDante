using System.Collections;
using UnityEngine;

public class MorseAudioLooper : MonoBehaviour
{
    [Header("Audio Clips con mensajes en código Morse")]
    public AudioClip[] morseAudios;

    [Tooltip("Selecciona el índice del audio a reproducir")]
    public int mensajeSeleccionado = 0;

    [Tooltip("Retraso entre cada reproducción (en segundos)")]
    public float delayEntreRepeticiones = 2f;

    [Header("AudioSource que reproducirá los clips")]
    public AudioSource audioSource;

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

    IEnumerator ReproducirMorse()
    {
        while (true)
        {
            AudioClip clip = morseAudios[mensajeSeleccionado];

            audioSource.PlayOneShot(clip);

            // Esperar duración del audio + delay
            yield return new WaitForSeconds(clip.length + delayEntreRepeticiones);
        }
    }
}
