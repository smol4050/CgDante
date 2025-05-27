using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla efectos de fundido (fade in/fade out) mediante un CanvasGroup.
/// Utiliza una implementaci�n singleton para facilitar el acceso global.
/// </summary>
public class FadeController : MonoBehaviour
{
    /// <summary>
    /// Instancia �nica del controlador de fundido.
    /// </summary>
    public static FadeController Instance;

    /// <summary>
    /// Grupo de Canvas sobre el que se aplicar� el efecto de fundido.
    /// </summary>
    public CanvasGroup fadeCanvas;

    /// <summary>
    /// Duraci�n en segundos del efecto de fundido.
    /// </summary>
    public float fadeDuration = 1f;

    /// <summary>
    /// Establece la instancia singleton o destruye el objeto si ya existe una instancia.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Ejecuta una transici�n de fundido hacia negro (opacidad completa).
    /// </summary>
    /// <returns>Una corrutina que realiza la animaci�n de opacidad.</returns>
    public IEnumerator FadeOut()
    {
        float t = 0f;
        fadeCanvas.blocksRaycasts = true;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
    }

    /// <summary>
    /// Ejecuta una transici�n de fundido desde negro (a opacidad cero).
    /// </summary>
    /// <returns>Una corrutina que realiza la animaci�n de opacidad.</returns>
    public IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.blocksRaycasts = false;
    }
}
