using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controla la inteligencia artificial del enemigo, incluyendo patrullaje, persecución y ataque al jugador.
/// </summary>
public class EnemigoIA : MonoBehaviour
{
    /// <summary>
    /// Transform del jugador para seguimiento y ataque.
    /// </summary>
    public Transform jugador;

    /// <summary>
    /// Distancia máxima a la que el enemigo puede detectar al jugador.
    /// </summary>
    public float rangoVision = 10f;

    /// <summary>
    /// Distancia a la que el enemigo puede atacar al jugador.
    /// </summary>
    public float rangoAtaque = 2f;

    /// <summary>
    /// Lista de puntos de patrullaje por los que el enemigo se mueve cuando no persigue al jugador.
    /// </summary>
    public List<Transform> puntosPatrullaje;

    private NavMeshAgent agente;
    private Animator animador;
    private int puntoActual = 0;
    private float tiempoParaNuevoDestino = 5f;
    private float tiempoEntreGolpes = 2f;
    private float tiempoUltimoGolpe = 0f;
    public float delayParaGolpe = 0.5f;

    /// <summary>
    /// Inicializa referencias y establece el primer destino de patrullaje.
    /// </summary>
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
        ElegirNuevoDestino();
    }

    /// <summary>
    /// Actualiza el comportamiento del enemigo cada frame:
    /// - Patrulla puntos de interés si el jugador está lejos.
    /// - Persigue y ataca si el jugador está cerca.
    /// </summary>
    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoVision)
        {
            // Perseguir al jugador
            agente.SetDestination(jugador.position);
            agente.isStopped = false;
            animador.SetBool("Idle", false);

            if (distancia <= rangoAtaque)
            {
                agente.isStopped = true;
                animador.SetBool("Atacando", true);

                if (Time.time - tiempoUltimoGolpe >= tiempoEntreGolpes)
                {
                    // Ejecutar ataque con delay
                    StartCoroutine(GolpeConDelay(delayParaGolpe));
                    tiempoUltimoGolpe = Time.time;
                }
            }
            else
            {
                animador.SetBool("Atacando", false);
            }
        }
        else
        {
            animador.SetBool("Atacando", false);

            if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && !agente.hasPath)
            {
                agente.isStopped = true;
                animador.SetBool("Idle", true);

                tiempoParaNuevoDestino -= Time.deltaTime;
                if (tiempoParaNuevoDestino <= 0f)
                {
                    ElegirNuevoDestino();
                    tiempoParaNuevoDestino = Random.Range(2f, 4f);
                    animador.SetBool("Idle", false);
                    agente.isStopped = false;
                }
            }
            else
            {
                animador.SetBool("Idle", false);
                agente.isStopped = false;
            }
        }
    }

    /// <summary>
    /// Selecciona el siguiente punto de patrullaje y establece el destino para el NavMeshAgent.
    /// </summary>
    void ElegirNuevoDestino()
    {
        if (puntosPatrullaje.Count == 0)
            return;

        Transform destino = puntosPatrullaje[puntoActual];
        agente.SetDestination(destino.position);

        puntoActual = (puntoActual + 1) % puntosPatrullaje.Count;
    }

    /// <summary>
    /// Corrutina que espera un tiempo determinado antes de aplicar daño al jugador si está en rango.
    /// </summary>
    /// <param name="delay">Tiempo en segundos antes de aplicar el golpe.</param>
    /// <returns>IEnumerator para corrutina.</returns>
    private IEnumerator GolpeConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Vector3.Distance(transform.position, jugador.position) <= rangoAtaque)
        {
            jugador.GetComponent<PlayerHealth>()?.RecibirGolpe();
        }
    }
}
