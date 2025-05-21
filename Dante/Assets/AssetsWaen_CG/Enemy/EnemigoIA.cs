using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : MonoBehaviour
{

    public Transform jugador;
    public float rangoVision = 10f;
    public float rangoAtaque = 2f;
    public List<Transform> puntosPatrullaje; // Lista de puntos de patrullaje

    private NavMeshAgent agente;
    private Animator animador;
    private int puntoActual = 0;
    private float tiempoParaNuevoDestino = 5f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
        ElegirNuevoDestino();
    }

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

                
                jugador.GetComponent<PlayerHealth>()?.RecibirGolpe();
            }
            else
            {
                animador.SetBool("Atacando", false);
            }
        }
        else
        {
            animador.SetBool("Atacando", false);

            // Verifica si ha llegado a su destino aleatorio
            if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && !agente.hasPath)
            {
                // En punto de espera (Idle)
                agente.isStopped = true;
                animador.SetBool("Idle", true);

                tiempoParaNuevoDestino -= Time.deltaTime;
                if (tiempoParaNuevoDestino <= 0f)
                {
                    ElegirNuevoDestino();
                    tiempoParaNuevoDestino = Random.Range(2f, 4f); // Nuevos tiempos de espera aleatorios
                    animador.SetBool("Idle", false); // volver a caminar
                    agente.isStopped = false;
                }
            }
            else
            {
                // Está en movimiento
                animador.SetBool("Idle", false);
                agente.isStopped = false;
            }
        }
    }

    void ElegirNuevoDestino()
    {
        if (puntosPatrullaje.Count == 0)
            return;

        // Selecciona el siguiente punto de patrullaje de la lista
        Transform destino = puntosPatrullaje[puntoActual];
        agente.SetDestination(destino.position);

        // Actualiza el índice del punto actual para patrullar en el siguiente
        puntoActual = (puntoActual + 1) % puntosPatrullaje.Count;
    }
}
