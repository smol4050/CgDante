using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la aparición aleatoria y activación visual y sonora de un enemigo tipo esqueleto.
/// Implementa la interfaz <see cref="IInteractuable"/> para permitir interacción (ej. desactivación al presionar 'E').
/// </summary>
public class InteractionEsqueletos : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al controlador general del enemigo en el nivel.
    /// </summary>
    EnemyController_Paraiso enemigoControllerPuertas;

    /// <summary>
    /// Componente MeshRenderer para controlar la visibilidad del esqueleto.
    /// </summary>
    MeshRenderer meshRenderer;

    /// <summary>
    /// Collider para activar o desactivar la detección de colisiones del esqueleto.
    /// </summary>
    Collider SuCollider;

    /// <summary>
    /// AudioSource para reproducir sonidos asociados al esqueleto.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Referencia a la corrutina que controla la aparición aleatoria.
    /// </summary>
    private Coroutine rutinaAparicion;

    /// <summary>
    /// Inicializa referencias y oculta visualmente el esqueleto al inicio.
    /// </summary>
    void Start()
    {
        enemigoControllerPuertas = FindObjectOfType<EnemyController_Paraiso>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        SuCollider = GetComponentInChildren<Collider>();
        audioSource = GetComponent<AudioSource>();

        DesactivarVisual();
    }

    /// <summary>
    /// Verifica si el jugador perdió para detener la corrutina de aparición y ocultar el esqueleto.
    /// </summary>
    void Update()
    {
        if (enemigoControllerPuertas.JugadorPerdio)
        {
            if (rutinaAparicion != null)
            {
                StopCoroutine(rutinaAparicion);
                rutinaAparicion = null;
                DesactivarVisual();
            }
        }
    }

    /// <summary>
    /// Inicia la corrutina que controla la aparición aleatoria del esqueleto.
    /// </summary>
    public void IniciarEnemigoEsqueletos()
    {
        rutinaAparicion = StartCoroutine(AparicionAleatoria());
    }

    /// <summary>
    /// Detiene la corrutina de aparición y oculta visualmente el esqueleto.
    /// </summary>
    public void DetenerEnemigoEsqueletos()
    {
        if (rutinaAparicion != null)
        {
            StopCoroutine(rutinaAparicion);
            rutinaAparicion = null;
        }

        DesactivarVisual();
    }

    /// <summary>
    /// Corrutina que espera un tiempo aleatorio entre 10 y 20 segundos y activa el esqueleto.
    /// </summary>
    /// <returns>IEnumerator para la corrutina.</returns>
    IEnumerator AparicionAleatoria()
    {
        while (true)
        {
            float tiempoEspera = Random.Range(10f, 20f);
            yield return new WaitForSeconds(tiempoEspera);

            ActivarEsqueleto();
        }
    }

    /// <summary>
    /// Activa visualmente y sonoramente el esqueleto.
    /// </summary>
    public void ActivarEsqueleto()
    {
        meshRenderer.enabled = true;
        SuCollider.enabled = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    /// <summary>
    /// Desactiva la visualización y el sonido del esqueleto.
    /// </summary>
    public void DesactivarVisual()
    {
        meshRenderer.enabled = false;
        SuCollider.enabled = false;
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    /// <summary>
    /// Método de la interfaz <see cref="IInteractuable"/> que desactiva el esqueleto al ser activado (ej. al presionar 'E').
    /// </summary>
    public void ActivarObjeto()
    {
        DesactivarVisual();
    }
}
