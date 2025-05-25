using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CronometroSupervivencia : MonoBehaviour
{
    public float tiempoTotal = 120f; // 5 minutos en segundos
    private float tiempoRestante;

    public TextMeshProUGUI textoTiempo; // Asignar en el Inspector
    private bool enEjecucion = false;
    private bool cambioDificultadHecho = false;

    GameController_ParaisoOscuro GameC;

    void Start()
    {
        tiempoRestante = tiempoTotal;
        GameC = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    void Update()
    {
        if (!enEjecucion) return;

        tiempoRestante -= Time.deltaTime;

        // Mostrar tiempo en formato MM:SS
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTiempo.text = $"{minutos:00}:{segundos:00}";

        // Cambiar dificultad cuando llegue a 2:30
        if (!cambioDificultadHecho && tiempoRestante <= 90f)
        {
            cambioDificultadHecho = true;
            ActivarModoDificil();
        }

        // Si el tiempo se acaba
        if (tiempoRestante <= 0)
        {
            enEjecucion = false;
            FinDelNivel();
        }
    }

    public void IniciarTemporizador()
    {
        enEjecucion = true;
        Debug.Log("Temporizador iniciado.");
    }

    void ActivarModoDificil()
    {
        Debug.Log("¡Modo difícil activado!");

        GameC.EstadoJugando();
        Debug.Log("Estado: JugandoEsqueletos");
        // Aquí puedes cambiar música, efectos, enemigos, etc.
        // Por ejemplo:
        // enemigo.SetVelocidad(2f);
        // GameManager.Instance.SetModo("jugando");
    }

    void FinDelNivel()
    {
        Debug.Log("¡Sobreviviste!");
        GameC.EstadoFinJuego();
        GameManager.Instance.CompletarNivel();
        Debug.Log("Estado: FinJuego");

        // Puedes mostrar UI de victoria o cambiar de escena
    }
}
