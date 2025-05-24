using System.Collections;
using UnityEngine;

/// <summary>
/// Controla la interacción con una puerta que puede abrirse y cerrarse con animación y sonido.
/// </summary>
public class PuertaInteractuable : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Indica si la puerta está actualmente abierta.
    /// </summary>
    private bool estaAbierta = false;

    /// <summary>
    /// Indica si la puerta está en medio de una animación.
    /// </summary>
    private bool enAnimacion = false;

    /// <summary>
    /// Ángulo en grados que la puerta gira para abrirse.
    /// </summary>
    public float anguloApertura = 90f;

    /// <summary>
    /// Duración de la animación de apertura o cierre en segundos.
    /// </summary>
    public float duracionApertura = 2f;

    /// <summary>
    /// Rotación original (cerrada) de la puerta.
    /// </summary>
    private Quaternion rotacionCerrada;

    /// <summary>
    /// Rotación objetivo (abierta) de la puerta.
    /// </summary>
    private Quaternion rotacionAbierta;

    /// <summary>
    /// Clip de audio que se reproduce al abrir la puerta.
    /// </summary>
    public AudioClip audioAbrir;

    /// <summary>
    /// Clip de audio que se reproduce al cerrar la puerta.
    /// </summary>
    public AudioClip audioCerrar;

    /// <summary>
    /// Fuente de audio para reproducir sonidos.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Inicializa las rotaciones y obtiene la fuente de audio.
    /// </summary>
    private void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Activa la interacción con la puerta: la abre o la cierra con animación y sonido.
    /// </summary>
    public void ActivarObjeto()
    {
        if (enAnimacion) return;

        if (audioSource != null)
        {
            audioSource.clip = estaAbierta ? audioCerrar : audioAbrir;
            audioSource.Play();
        }

        if (estaAbierta)
        {
            StartCoroutine(RotarPuerta(rotacionAbierta, rotacionCerrada));
        }
        else
        {
            StartCoroutine(RotarPuerta(rotacionCerrada, rotacionAbierta));
        }

        estaAbierta = !estaAbierta;
    }

    /// <summary>
    /// Coroutine que rota la puerta entre dos rotaciones en un tiempo determinado.
    /// </summary>
    /// <param name="desde">Rotación inicial.</param>
    /// <param name="hasta">Rotación final.</param>
    /// <returns>IEnumerator para la coroutine.</returns>
    private System.Collections.IEnumerator RotarPuerta(Quaternion desde, Quaternion hasta)
    {
        enAnimacion = true;

        float tiempo = 0f;
        while (tiempo < duracionApertura)
        {
            transform.rotation = Quaternion.Slerp(desde, hasta, tiempo / duracionApertura);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.rotation = hasta;
        enAnimacion = false;
    }
}
