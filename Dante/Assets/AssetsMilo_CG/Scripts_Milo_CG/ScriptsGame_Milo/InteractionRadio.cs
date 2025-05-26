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
    private bool radioApagada = false;

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
            fuente.minDistance = 3f;
            fuente.maxDistance = 50f;
            fuente.rolloffMode = AudioRolloffMode.Linear;

            fuente.Play();
            fuente.volume = (i == canalActual) ? 1f : 0f;

            fuentes[i] = fuente;
        }

        fxSource = gameObject.AddComponent<AudioSource>();
        fxSource.spatialBlend = 1f;
        fxSource.minDistance = 3f;
        fxSource.maxDistance = 50f;
        fxSource.rolloffMode = AudioRolloffMode.Linear;
    }

    void Update()
    {
        if (gameController.estadoActual == GameState.Jugando && !reiniciadoPorEstado)
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

        if (gameController.estadoActual == GameState.JugandoTutorial && !cambiadoPorTutorial)
        {
            fxSource.PlayOneShot(cambioCanal, 0.7f);

            fuentes[canalActual].volume = 0f;
            canalActual = 1;
            fuentes[canalActual].volume = 0.25f;

            cambiadoPorTutorial = true;
            reiniciadoPorEstado = false;
            radioApagada = false;
        }

        if (gameController.estadoActual == GameState.FinJuego && !radioApagada)
        {
            foreach (AudioSource fuente in fuentes)
            {
                fuente.volume = 0f;
                fuente.Stop(); // Opcional: detén el audio en lugar de solo silenciarlo
            }

            radioApagada = true;
        }

        // Resetear flags si vuelve a otro estado (opcional)
        if (gameController.estadoActual != GameState.FinJuego &&
            gameController.estadoActual != GameState.Jugando &&
            gameController.estadoActual != GameState.JugandoTutorial)
        {
            reiniciadoPorEstado = false;
            cambiadoPorTutorial = false;
            radioApagada = false;
        }


    }

    public void ActivarObjeto()
    {
        if (gameController.estadoActual == GameState.FinJuego)
        {
            Debug.Log("El radio está apagado. No se puede interactuar.");
            return;
        }

        if (gameController.estadoActual == GameState.JugandoTutorial || gameController.estadoActual == GameState.FinJuego)
        {
            if (cambioCanal != null)
            {
                fxSource.PlayOneShot(cambioCanal, 0.7f);
            }

            fuentes[canalActual].volume = 0f;
            canalActual = (canalActual + 1) % canales.Length;
            fuentes[canalActual].volume = 0.25f;
        }

        if (gameController.estadoActual == GameState.PreInicio || gameController.estadoActual == GameState.Jugando)
        {
            Debug.Log("El radio no puede ser activado en este estado.");
        }

    }
}
