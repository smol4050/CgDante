using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class InteractionRadio : MonoBehaviour, IInteractuable
{

    public AudioClip[] canales; // 0: estática, 1: voz, 2: canción1, 3: canción2
    public AudioClip cambioCanal;
    private AudioSource fxSource;

    private AudioSource[] fuentes;
    private int canalActual = 0;

    void Start()
    {
        fuentes = new AudioSource[canales.Length];

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

    public void ActivarObjeto()
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
}
