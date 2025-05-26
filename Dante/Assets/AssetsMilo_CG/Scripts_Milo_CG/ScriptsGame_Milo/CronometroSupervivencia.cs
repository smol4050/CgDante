using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controla un cronómetro de supervivencia que cuenta regresivamente desde un tiempo inicial.
/// Al llegar a ciertos umbrales de tiempo, activa eventos como cambio de dificultad o fin de nivel.
/// </summary>
public class CronometroSupervivencia : MonoBehaviour
{
    /// <summary>
    /// Tiempo total del cronómetro en segundos.
    /// </summary>
    public float tiempoTotal = 120f; // 2 minutos

    /// <summary>
    /// Tiempo restante actual del cronómetro.
    /// </summary>
    private float tiempoRestante;

    /// <summary>
    /// Texto en pantalla que muestra el tiempo restante (debe asignarse en el Inspector).
    /// </summary>
    public TextMeshProUGUI textoTiempo;

    /// <summary>
    /// Indica si el cronómetro está activo.
    /// </summary>
    private bool enEjecucion = false;

    /// <summary>
    /// Controla si ya se realizó el cambio de dificultad.
    /// </summary>
    private bool cambioDificultadHecho = false;

    /// <summary>
    /// Referencia al controlador principal del nivel.
    /// </summary>
    GameController_ParaisoOscuro GameC;

    /// <summary>
    /// Inicializa las variables y obtiene referencias necesarias al iniciar el componente.
    /// </summary>
    void Start()
    {
        tiempoRestante = tiempoTotal;
        GameC = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    /// <summary>
    /// Actualiza el cronómetro cada frame si está en ejecución.
    /// Cambia la dificultad a mitad del tiempo y ejecuta el final del nivel si el tiempo se agota.
    /// </summary>
    void Update()
    {
        if (!enEjecucion) return;

        tiempoRestante -= Time.deltaTime;

        // Mostrar tiempo en formato MM:SS
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTiempo.text = $"{minutos:00}:{segundos:00}";

        // Cambiar dificultad al llegar a los 90 segundos
        if (!cambioDificultadHecho && tiempoRestante <= 90f)
        {
            cambioDificultadHecho = true;
            ActivarModoDificil();
        }

        // Finalizar nivel si el tiempo se agota
        if (tiempoRestante <= 0)
        {
            enEjecucion = false;
            FinDelNivel();
        }
    }

    /// <summary>
    /// Inicia el temporizador y permite que comience a descontar tiempo.
    /// </summary>
    public void IniciarTemporizador()
    {
        enEjecucion = true;
        Debug.Log("Temporizador iniciado.");
    }

    /// <summary>
    /// Activa el modo difícil, lo cual puede modificar el estado del juego.
    /// </summary>
    void ActivarModoDificil()
    {
        Debug.Log("¡Modo difícil activado!");

        GameC.EstadoJugando();
        Debug.Log("Estado: JugandoEsqueletos");
        // Aquí se podrían modificar aspectos como música, enemigos, etc.
    }

    /// <summary>
    /// Llama al final del juego exitosamente y marca el nivel como completado.
    /// </summary>
    void FinDelNivel()
    {
        Debug.Log("¡Sobreviviste!");
        GameC.EstadoFinJuego();
        GameManager.Instance.CompletarNivel();
        Debug.Log("Estado: FinJuego");

        // Aquí se puede mostrar una UI de victoria o pasar a otra escena.
    }
}
