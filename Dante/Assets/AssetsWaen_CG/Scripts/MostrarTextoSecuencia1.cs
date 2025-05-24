using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controla la secuencia inicial de introducción mostrando textos ordenadamente,
/// y al finalizar muestra el panel de misión.
/// </summary>
public class MostrarTextoSecuencia1 : MonoBehaviour
{
    /// <summary>
    /// Arreglo de textos que se mostrarán en orden durante la introducción.
    /// </summary>
    public TextMeshProUGUI[] textosOrdenados;

    /// <summary>
    /// Tiempo de espera entre la aparición de cada texto.
    /// </summary>
    public float tiempoEntreTextos = 1f;

    /// <summary>
    /// CanvasGroup del panel de introducción para controlar el fade in/out.
    /// </summary>
    public CanvasGroup canvasGroupIntro;

    /// <summary>
    /// Panel que muestra la misión después de la introducción.
    /// </summary>
    public GameObject panelMision;

    /// <summary>
    /// Duración del efecto de desvanecimiento (fade) entre los paneles.
    /// </summary>
    public float fadeDuration = 1.5f;

    private bool introTerminada = false;

    /// <summary>
    /// Inicializa la escena ocultando todos los textos y paneles,
    /// y empieza la secuencia de introducción.
    /// </summary>
    void Start()
    {
        foreach (var texto in textosOrdenados)
        {
            texto.gameObject.SetActive(false);
        }

        if (panelMision != null)
            panelMision.SetActive(false);

        StartCoroutine(FlujoIntro());
    }

    /// <summary>
    /// Ejecuta la secuencia de introducción: fade in, muestra textos en orden,
    /// fade out, muestra el panel de misión y continúa el flujo en GameController.
    /// </summary>
    IEnumerator FlujoIntro()
    {
        // Fade In del panel de introducción
        canvasGroupIntro.alpha = 0f;
        canvasGroupIntro.interactable = true;
        canvasGroupIntro.blocksRaycasts = true;
        yield return StartCoroutine(FadeCanvasGroup(canvasGroupIntro, 0f, 1f, fadeDuration));

        // Mostrar textos uno por uno
        foreach (var texto in textosOrdenados)
        {
            texto.gameObject.SetActive(true);
            yield return new WaitForSeconds(tiempoEntreTextos);
        }

        // Espera antes de hacer fade out
        yield return new WaitForSeconds(1f);

        // Fade Out del panel de introducción
        yield return StartCoroutine(FadeCanvasGroup(canvasGroupIntro, 1f, 0f, fadeDuration));
        canvasGroupIntro.interactable = false;
        canvasGroupIntro.blocksRaycasts = false;

        // Activar el panel de misión
        if (panelMision != null)
        {
            panelMision.SetActive(true);
            CanvasGroup cgMision = panelMision.GetComponent<CanvasGroup>();
            if (cgMision != null)
            {
                cgMision.alpha = 0f;
                cgMision.interactable = true;
                cgMision.blocksRaycasts = true;
                yield return StartCoroutine(FadeCanvasGroup(cgMision, 0f, 1f, fadeDuration));
            }
        }

        // Notifica al GameController solo una vez
        if (!introTerminada && GameController.Instance != null)
        {
            introTerminada = true;
            GameController.Instance.IniciarSecuenciaDesdeIntro();
        }
    }

    /// <summary>
    /// Realiza un efecto de desvanecimiento en el CanvasGroup desde un valor de alfa inicial hasta uno final.
    /// </summary>
    /// <param name="cg">CanvasGroup al que se aplica el fade.</param>
    /// <param name="start">Valor inicial de alfa.</param>
    /// <param name="end">Valor final de alfa.</param>
    /// <param name="duration">Duración del efecto.</param>
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;

        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cg.alpha = end;
    }
}
