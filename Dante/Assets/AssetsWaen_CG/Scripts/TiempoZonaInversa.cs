using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TiempoZonaInversa : MonoBehaviour
{
    public float tiempoLimite = 30f;
    private float tiempoRestante;
    private bool contadorActivo = false;
    private bool enZonaInvertida = false;

    public TextMeshProUGUI textoTiempo;
    public GameObject panelTiempo;
    public Transform puntoZonaInvertida;
    public Transform puntoZonaOriginal;

    private GameController gameController;
    private Transform jugador;

    void Start()
    {
        panelTiempo.SetActive(false);
        gameController = GameController.Instance;
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!contadorActivo || enZonaInvertida) return;

        if (gameController != null && gameController.corazonesRecolectados >= gameController.totalCorazones)
        {
            contadorActivo = false;
            panelTiempo.SetActive(false);
            return;
        }

        tiempoRestante -= Time.deltaTime;
        textoTiempo.text = $"Tiempo restante: {Mathf.CeilToInt(tiempoRestante)}";

        if (tiempoRestante <= 0f)
        {
            StartCoroutine(TransicionZonaInvertida());
        }
    }

    private IEnumerator TransicionZonaInvertida()
    {
        contadorActivo = false;
        panelTiempo.SetActive(false);

        yield return FadeController.Instance.FadeOut();

        if (jugador != null && puntoZonaInvertida != null)
        {
            jugador.position = puntoZonaInvertida.position;
        }

        enZonaInvertida = true;

        yield return FadeController.Instance.FadeIn();
    }

    public void RecargarTiempo()
    {
        tiempoRestante = tiempoLimite;
    }

    public void ActivarContador()
    {
        if (enZonaInvertida) return;

        tiempoRestante = tiempoLimite;
        contadorActivo = true;
        panelTiempo.SetActive(true);
    }

    public IEnumerator VolverZonaOriginal()
    {
        yield return FadeController.Instance.FadeOut();

        if (jugador != null && puntoZonaOriginal != null)
        {
            jugador.position = puntoZonaOriginal.position;
        }

        enZonaInvertida = false;

        yield return FadeController.Instance.FadeIn();
    }
}
