using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class InteractionRadio : MonoBehaviour, IInteractuable
{

    GameController_ParaisoOscuro gameController;

    public AudioClip[] canales; // 0: estática, 1: voz, 2: canción1, 3: canción2
    public AudioClip cambioCanal;
    private AudioSource fxSource;
    private bool reiniciadoPorEstado = false;
    private bool cambiadoPorTutorial = false;

    private AudioSource[] fuentes;
    private int canalActual = 0;

    void Start()
    {
        fuentes = new AudioSource[canales.Length];
        gameController = FindObjectOfType<GameController_ParaisoOscuro>();

        // Crear un AudioSource por canal
        for (int i = 0; i < canales.Length; i++)
        {
            AudioSource fuente = gameObject.AddComponent<AudioSource>();
            fuente.clip = canales[i];
            fuente.loop = true;
            fuente.spatialBlend = 0.5f;

            // Audio 3D
            fuente.spatialBlend = 1f;
            fuente.minDistance = 1f;
            fuente.maxDistance = 15f;
            fuente.rolloffMode = AudioRolloffMode.Linear;

            fuente.Play();
            fuente.volume = (i == canalActual) ? 1f : 0f;

            fuentes[i] = fuente;
        }

        fxSource = gameObject.AddComponent<AudioSource>();
        fxSource.spatialBlend = 1f;
        fxSource.minDistance = 1f;
        fxSource.maxDistance = 15f;
        fxSource.rolloffMode = AudioRolloffMode.Linear;
    }

    void Update()
    {
        // Si entras al estado Jugando y aún no se ha reiniciado al canal 0
        if (gameController.estadoActual == GameState.Jugando && !reiniciadoPorEstado)
        {
            if (canalActual != 0)
            {
                fuentes[canalActual].volume = 0f;
                canalActual = 0;
                fuentes[0].volume = 0.25f;
            }

            reiniciadoPorEstado = true;
            cambiadoPorTutorial = false; // Por si acaso entraste desde JugandoTutorial
        }

        // Si entras al estado JugandoTutorial y aún no cambiaste al canal 1
        if (gameController.estadoActual == GameState.JugandoTutorial && !cambiadoPorTutorial)
        {
            fuentes[canalActual].volume = 0f;
            canalActual = 1;
            fuentes[canalActual].volume = 0.25f;

            cambiadoPorTutorial = true;
            reiniciadoPorEstado = false; // por si vuelves a Jugando más tarde
        }

        // Si sales de ambos estados, resetea banderas
        if (gameController.estadoActual != GameState.Jugando &&
            gameController.estadoActual != GameState.JugandoTutorial)
        {
            reiniciadoPorEstado = false;
            cambiadoPorTutorial = false;
        }
    }

    public void ActivarObjeto()
    {
        if(gameController.estadoActual == GameState.JugandoTutorial || gameController.estadoActual == GameState.FinJuego)
        {
            if (cambioCanal != null)
        {
            fxSource.PlayOneShot(cambioCanal, 0.7f); // Puedes ajustar el volumen aquí
        }

        // Cambiar de canal
        fuentes[canalActual].volume = 0f; // Silencia el actual
        canalActual = (canalActual + 1) % canales.Length;
        fuentes[canalActual].volume = 0.25f; // Activa el nuevo
        }

        if (gameController.estadoActual == GameState.PreInicio || gameController.estadoActual == GameState.Jugando)
        {
            Debug.Log("El radio no puede ser activado en este estado.");
        }

    }
}
