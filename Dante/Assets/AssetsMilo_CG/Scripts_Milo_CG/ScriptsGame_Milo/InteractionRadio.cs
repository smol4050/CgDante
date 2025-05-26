using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

/// <summary>
/// Controla la radio interactuable con múltiples canales de audio, manejando cambios según el estado del juego.
/// Implementa la interfaz <see cref="IInteractuable"/> para permitir la interacción del jugador.
/// </summary>
public class InteractionRadio : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al controlador general del juego, para leer el estado actual.
    /// </summary>
    GameController_ParaisoOscuro gameController;

    /// <summary>
    /// Lista de clips de audio que representan los canales de la radio.
    /// 0: estática, 1: voz, 2: canción1, 3: canción2, etc.
    /// </summary>
    public AudioClip[] canales;

    /// <summary>
    /// Clip de sonido que se reproduce al cambiar de canal.
    /// </summary>
    public AudioClip cambioCanal;

    /// <summary>
    /// AudioSource dedicado para reproducir el efecto de cambio de canal.
    /// </summary>
    private AudioSource fxSource;

    /// <summary>
    /// Array de AudioSource, uno por canal, para control independiente.
    /// </summary>
    private AudioSource[] fuentes;

    /// <summary>
    /// Índice del canal actualmente activo.
    /// </summary>
    private int canalActual = 0;

    /// <summary>
    /// Flags para controlar eventos que solo deben ocurrir una vez por estado.
    /// </summary>
    private bool reiniciadoPorEstado = false;
    private bool cambiadoPorTutorial = false;
    private bool radioApagada = false;

    void Start()
    {
        gameController = FindObjectOfType<GameController_ParaisoOscuro>();

        // Inicializa AudioSources, uno por canal
        fuentes = new AudioSource[canales.Length];
        for (int i = 0; i < canales.Length; i++)
        {
            AudioSource fuente = gameObject.AddComponent<AudioSource>();
            fuente.clip = canales[i];
            fuente.loop = true;
            // Ajuste para sonido 3D
            fuente.spatialBlend = 1f;
            fuente.minDistance = 3f;
            fuente.maxDistance = 50f;
            fuente.rolloffMode = AudioRolloffMode.Linear;

            fuente.Play();
            fuente.volume = (i == canalActual) ? 1f : 0f;

            fuentes[i] = fuente;
        }

        // AudioSource para efectos FX (cambio de canal)
        fxSource = gameObject.AddComponent<AudioSource>();
        fxSource.spatialBlend = 1f;
        fxSource.minDistance = 3f;
        fxSource.maxDistance = 50f;
        fxSource.rolloffMode = AudioRolloffMode.Linear;
    }

    void Update()
    {
        switch (gameController.estadoActual)
        {
            case GameState.Jugando:
                if (!reiniciadoPorEstado)
                {
                    if (canalActual != 0)
                    {
                        fuentes[canalActual].volume = 0f;
                        canalActual = 0;
                        fuentes[0].volume = 0.25f;
                    }
                    reiniciadoPorEstado = true;
                    cambiadoPorTutorial = false;
                    radioApagada = false;
                }
                break;

            case GameState.JugandoTutorial:
                if (!cambiadoPorTutorial)
                {
                    if (cambioCanal != null)
                        fxSource.PlayOneShot(cambioCanal, 0.7f);

                    fuentes[canalActual].volume = 0f;
                    canalActual = 1;
                    fuentes[canalActual].volume = 0.25f;

                    cambiadoPorTutorial = true;
                    reiniciadoPorEstado = false;
                    radioApagada = false;
                }
                break;

            case GameState.FinJuego:
                if (!radioApagada)
                {
                    foreach (AudioSource fuente in fuentes)
                    {
                        fuente.volume = 0f;
                        fuente.Stop(); 
                    }
                    radioApagada = true;
                }
                break;

            default:
                // Resetea flags para otros estados
                reiniciadoPorEstado = false;
                cambiadoPorTutorial = false;
                radioApagada = false;
                break;
        }
    }

    /// <summary>
    /// Cambia el canal al siguiente cuando el jugador interactúa con la radio,
    /// respetando las reglas según el estado actual del juego.
    /// </summary>
    public void ActivarObjeto()
    {
        if (gameController.estadoActual == GameState.FinJuego)
        {
            Debug.Log("El radio está apagado. No se puede interactuar.");
            return;
        }

        if (gameController.estadoActual == GameState.JugandoTutorial)
        {
            if (cambioCanal != null)
                fxSource.PlayOneShot(cambioCanal, 0.7f);

            fuentes[canalActual].volume = 0f;
            canalActual = (canalActual + 1) % canales.Length;
            fuentes[canalActual].volume = 0.25f;
        }
        else if (gameController.estadoActual == GameState.Jugando || gameController.estadoActual == GameState.PreInicio)
        {
            Debug.Log("El radio no puede ser activado en este estado.");
        }
    }
}
