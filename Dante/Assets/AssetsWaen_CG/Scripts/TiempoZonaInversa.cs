using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla el sistema de tiempo para una zona que se invierte si el jugador no recarga el tiempo a tiempo.
/// Incluye lógica de transición, reubicación del jugador y desactivación del contador al completar objetivos.
/// </summary>
public class TiempoZonaInversa : MonoBehaviour
{
    /// <summary>
    /// Tiempo máximo permitido antes de activar la zona invertida.
    /// </summary>
    public float tiempoLimite = 30f;

    /// <summary>
    /// Tiempo restante antes de que ocurra la transición.
    /// </summary>
    private float tiempoRestante;

    /// <summary>
    /// Indica si el contador de tiempo está activo.
    /// </summary>
    private bool contadorActivo = false;

    /// <summary>
    /// Indica si el jugador se encuentra actualmente en la zona invertida.
    /// </summary>
    private bool enZonaInvertida = false;

    /// <summary>
    /// Texto que muestra el tiempo restante en la interfaz.
    /// </summary>
    public TextMeshProUGUI textoTiempo;

    /// <summary>
    /// Panel de la UI que contiene el contador de tiempo.
    /// </summary>
    public GameObject panelTiempo;

    /// <summary>
    /// Punto de destino al entrar en la zona invertida.
    /// </summary>
    public Transform puntoZonaInvertida;

    /// <summary>
    /// Punto de retorno a la zona original.
    /// </summary>
    public Transform puntoZonaOriginal;

    /// <summary>
    /// Referencia al controlador del juego.
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// Referencia al transform del jugador.
    /// </summary>
    private Transform jugador;

    /// <summary>
    /// Inicializa referencias y oculta el panel de tiempo al inicio.
    /// </summary>
    void Start()
    {
        panelTiempo.SetActive(false);
        gameController = GameController.Instance;
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    /// <summary>
    /// Actualiza el contador si está activo y aún no se ha ingresado a la zona invertida.
    /// Detiene el contador si se han recolectado todos los corazones.
    /// </summary>
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

    /// <summary>
    /// Corrutina que realiza la transición del jugador a la zona invertida tras agotarse el tiempo.
    /// Incluye efecto de fade para una transición visual fluida.
    /// </summary>
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

    /// <summary>
    /// Reinicia el contador al tiempo máximo definido por <see cref="tiempoLimite"/>.
    /// </summary>
    public void RecargarTiempo()
    {
        tiempoRestante = tiempoLimite;
    }

    /// <summary>
    /// Activa el contador de tiempo y muestra el panel si el jugador aún no está en la zona invertida.
    /// </summary>
    public void ActivarContador()
    {
        if (enZonaInvertida) return;

        tiempoRestante = tiempoLimite;
        contadorActivo = true;
        panelTiempo.SetActive(true);
    }

    /// <summary>
    /// Corrutina que devuelve al jugador a la zona original con una transición visual.
    /// </summary>
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
