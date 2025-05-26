using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controla la aparición y comportamiento del enemigo en el nivel "Paraíso Oscuro",
/// incluyendo su posición aleatoria, sonidos de aparición y lógica de pérdida.
/// </summary>
public class EnemyController_Paraiso : MonoBehaviour
{
    private GameController_ParaisoOscuro gameC;

    /// <summary>
    /// Puntos donde puede aparecer el enemigo. Se asignan desde el Inspector.
    /// </summary>
    public GameObject[] puntosDeAparicion;

    /// <summary>
    /// Tiempo en segundos que el jugador tiene para reaccionar antes de perder.
    /// </summary>
    public float tiempoParaReaccionar = 15f;

    /// <summary>
    /// Velocidad del enemigo. Se puede usar para aumentar la dificultad.
    /// </summary>
    public float speedEnemy = 1f;

    /// <summary>
    /// Componente de audio del enemigo.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clips de sonido asociados a cada puerta. Se reproducen cuando el enemigo aparece.
    /// </summary>
    public AudioClip[] sonidosPuerta;

    private bool jugadorPerdio = false;
    private int indiceActual = -1;
    private Coroutine rutinaEnemigo;

    /// <summary>
    /// Indica si el jugador ha perdido.
    /// </summary>
    public bool JugadorPerdio { get => jugadorPerdio; set => jugadorPerdio = value; }

    /// <summary>
    /// Inicializa el controlador, desactiva esferas de aparición y encuentra el controlador del juego.
    /// </summary>
    void Start()
    {
        for (int i = 0; i < puntosDeAparicion.Length; i++)
        {
            puntosDeAparicion[i].SetActive(false);
        }

        gameC = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    /// <summary>
    /// Inicia la rutina del enemigo que aparece en puntos aleatorios.
    /// </summary>
    public void IniciarRutinaEnemigo1()
    {
        rutinaEnemigo = StartCoroutine(AparicionEnemigoPuertas());
        Debug.Log("Iniciando rutina enemigo 1");
    }

    /// <summary>
    /// Detiene la rutina actual del enemigo si está en ejecución.
    /// </summary>
    public void DetenerRutinaEnemigo1()
    {
        if (rutinaEnemigo != null)
            StopCoroutine(rutinaEnemigo);
        Debug.Log("Deteniendo rutina enemigo 1");
    }

    /// <summary>
    /// Corrutina que controla la aparición del enemigo, la lógica de detección de la puerta cerrada
    /// y el resultado (seguir o perder).
    /// </summary>
    /// <returns>IEnumerator para la corrutina de aparición.</returns>
    IEnumerator AparicionEnemigoPuertas()
    {
        while (!jugadorPerdio)
        {
            // Desactiva todas las esferas antes de activar la nueva
            for (int i = 0; i < puntosDeAparicion.Length; i++)
            {
                puntosDeAparicion[i].SetActive(false);
            }

            // Elegir punto aleatorio y mover al enemigo
            indiceActual = Random.Range(0, puntosDeAparicion.Length);
            Transform punto = puntosDeAparicion[indiceActual].transform;
            puntosDeAparicion[indiceActual].SetActive(true); // Activar la nueva esfera
            transform.position = punto.position;

            // Sonido de puerta si hay clip asignado
            if (audioSource && sonidosPuerta.Length > indiceActual)
                AudioSource.PlayClipAtPoint(sonidosPuerta[indiceActual], punto.position);

            Debug.Log("¡Enemigo en puerta: " + indiceActual + "!");

            // Esperar por reacción del jugador
            float tiempo = 0f;
            while (tiempo < tiempoParaReaccionar * speedEnemy)
            {
                if (InteractionDoors.EstadoPuertas[indiceActual] == false)
                {
                    Debug.Log("Puerta cerrada " + indiceActual + " a tiempo.");
                    if (gameC != null) gameC.PuertaCerradaCorrectamente();
                    break;
                }

                tiempo += Time.deltaTime;
                yield return null;
            }

            // Si no se cerró la puerta a tiempo
            if (InteractionDoors.EstadoPuertas[indiceActual] == true)
            {
                jugadorPerdio = true;
                gameC.EstadoGameOver();
                Debug.Log("¡PERDISTE!");
                break;
            }

            yield return new WaitForSeconds(2f);
        }

        // Asegúrate de desactivar todas al terminar
        for (int i = 0; i < puntosDeAparicion.Length; i++)
        {
            puntosDeAparicion[i].SetActive(false);
        }
    }
}
