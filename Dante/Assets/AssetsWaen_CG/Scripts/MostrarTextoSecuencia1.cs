using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MostrarTextoSecuencia1 : MonoBehaviour
{
    public TextMeshProUGUI[] textosOrdenados;
    public float tiempoEntreTextos = 1f;
    public CanvasGroup canvasGroupIntro;
    public GameObject panelMision; // Asigna aquí el panel de misión en el inspector
    public float fadeDuration = 1.5f;

    void Start()
    {
        foreach (var texto in textosOrdenados)
        {
            texto.gameObject.SetActive(false);
        }

        // Ocultar el panel de misión al principio
        if (panelMision != null)
            panelMision.SetActive(false);

        StartCoroutine(FlujoIntro());
    }

    IEnumerator FlujoIntro()
    {
        // Fade In del panel de introducción
        canvasGroupIntro.interactable = true;
        canvasGroupIntro.blocksRaycasts = true;
        yield return StartCoroutine(FadeCanvasGroup(canvasGroupIntro, 0f, 1f, fadeDuration));

        // Mostrar textos uno por uno
        foreach (var texto in textosOrdenados)
        {
            texto.gameObject.SetActive(true);
            yield return new WaitForSeconds(tiempoEntreTextos);
        }

        // Espera un poco antes de desaparecer
        yield return new WaitForSeconds(1f);

        // Fade Out del panel de introducción
        yield return StartCoroutine(FadeCanvasGroup(canvasGroupIntro, 1f, 0f, fadeDuration));
        canvasGroupIntro.interactable = false;
        canvasGroupIntro.blocksRaycasts = false;
        // NOTA: No se desactiva el GameObject

        // Activa el panel de misión suavemente
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
    }

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
