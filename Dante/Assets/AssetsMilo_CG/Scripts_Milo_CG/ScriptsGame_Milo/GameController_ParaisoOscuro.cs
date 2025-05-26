using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// Estados posibles del juego.
/// </summary>
public enum GameState
{
    PreInicio,
    JugandoTutorial,
    Jugando,
    FinJuego,
    GameOver
}

/// <summary>
/// Controlador principal del nivel "Paraíso Oscuro". Maneja el estado del juego, eventos clave, transición de luces, enemigo y UI.
/// </summary>
public class GameController_ParaisoOscuro : MonoBehaviour
{
    /// <summary>Estado actual del juego.</summary>
    public GameState estadoActual = GameState.PreInicio;

    EnemyController_Paraiso enemigoController;

    /// <summary>Referencia al cronómetro de supervivencia.</summary>
    public CronometroSupervivencia CSupervivencia;

    /// <summary>Array de esqueletos interactuables.</summary>
    public InteractionEsqueletos[] esqueletos;

    /// <summary>Referencia a la primera puerta final roja.</summary>
    public PuertaFinalRoja1 puertaFinalRoja1;

    /// <summary>GameObject de la segunda puerta final roja.</summary>
    public GameObject puertaFinalRoja2;

    /// <summary>Audio ambiental del entorno.</summary>
    public AudioSource AudioEntorno;

    /// <summary>Controlador de las luces del entorno.</summary>
    public LucesController lucesController;

    /// <summary>Luz direccional principal.</summary>
    public Light DirectionalLight;

    /// <summary>Texto en pantalla que muestra la cantidad de puertas abiertas.</summary>
    public TextMeshProUGUI CantidadPuertas;

    /// <summary>Texto en pantalla que muestra el puntaje.</summary>
    public TextMeshProUGUI textoPuntaje;

    /// <summary>Cantidad de objetos necesarios para completar la escena.</summary>
    public int objetosNecesariosEscena = 10;

    /// <summary>Contador de puertas abiertas por el jugador.</summary>
    public int puertasAbiertas = 0;

    /// <summary>Cantidad de puertas necesarias para pasar a la segunda parte.</summary>
    public int puertasNecesarias2daParte = 8;

    /// <summary>Cantidad de puertas necesarias para terminar el nivel.</summary>
    public int puertasNecesariasTerminar = 20;

    /// <summary>Referencia al controlador de pausa y reanudación.</summary>
    public PausarReanudar pausarReanudar;

    /// <summary>Panel de información mostrado al inicio del juego.</summary>
    public GameObject panelInfoInicio;

    /// <summary>Transform del agua que sube al final del juego.</summary>
    public Transform aguaTransform;

    /// <summary>Altura final a la que debe subir el agua.</summary>
    public float alturaFinal = 6.5f;

    /// <summary>Velocidad a la que sube el agua.</summary>
    public float velocidadSubida = 0.05f;

    private GameManager gm;

    /// <summary>
    /// Inicializa el estado del juego y las referencias necesarias.
    /// </summary>
    void Start()
    {
        estadoActual = GameState.PreInicio;
        gm = GameManager.Instance;

        if (gm != null)
        {
            gm.OnObjetoRecolectado += ObjetoRecolectado;
        }

        enemigoController = FindObjectOfType<EnemyController_Paraiso>();
        puertaFinalRoja2.SetActive(false);
        AudioEntorno.Stop();

        int recolectados = gm.ObtenerObjetosRecolectados();
        textoPuntaje.text = recolectados.ToString();

        StartCoroutine(InicioDiapositivasTutorial());
    }

    /// <summary>
    /// Limpia eventos al destruir el objeto para evitar memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (gm != null)
        {
            gm.OnObjetoRecolectado -= ObjetoRecolectado;
        }
    }

    /// <summary>
    /// Corrutina que inicia la presentación del tutorial después de 2 segundos.
    /// </summary>
    IEnumerator InicioDiapositivasTutorial()
    {
        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
        panelInfoInicio.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.canMove = false;
    }

    /// <summary>
    /// Cambia el estado del juego a "JugandoTutorial" y activa enemigos, cronómetro y audio.
    /// </summary>
    public void EstadoJugandoTutorial()
    {
        estadoActual = GameState.JugandoTutorial;

        enemigoController.IniciarRutinaEnemigo1();
        CSupervivencia.IniciarTemporizador();
        AudioEntorno.Play();

        Debug.Log("Estado: Jugando Tutorial");
    }

    /// <summary>
    /// Cambia el estado del juego a "Jugando" y activa enemigos, luces y efectos.
    /// </summary>
    public void EstadoJugando()
    {
        estadoActual = GameState.Jugando;
        enemigoController.speedEnemy = 0.65f;

        if (lucesController.transicionLuzActual != null) StopCoroutine(lucesController.transicionLuzActual);
        lucesController.transicionLuzActual = StartCoroutine(lucesController.TransicionIntensidadLuz(6f));

        foreach (var esqueleto in esqueletos)
        {
            if (esqueleto != null)
                esqueleto.IniciarEnemigoEsqueletos();
            else
                Debug.LogWarning("Un esqueleto está sin asignar en el array.");
        }

        lucesController.TitilarLuces();
        Debug.Log("Estado: Jugando");
    }

    /// <summary>
    /// Cambia el estado del juego a "FinJuego", detiene enemigos y activa efectos de finalización.
    /// </summary>
    public void EstadoFinJuego()
    {
        estadoActual = GameState.FinJuego;

        enemigoController.DetenerRutinaEnemigo1();

        if (lucesController.transicionLuzActual != null) StopCoroutine(lucesController.transicionLuzActual);
        lucesController.transicionLuzActual = StartCoroutine(lucesController.TransicionIntensidadLuz(100f));

        foreach (var esqueleto in esqueletos)
        {
            if (esqueleto != null)
                esqueleto.DetenerEnemigoEsqueletos();
            else
                Debug.LogWarning("Un esqueleto no se pudo apagar");
        }

        lucesController.EncenderLuzNormal();
        puertaFinalRoja1.AbrirPuerta();
        puertaFinalRoja2.SetActive(true);
        AudioEntorno.Stop();

        StartCoroutine(SubirAgua());

        Debug.Log("Estado: FinJuego");
    }

    /// <summary>
    /// Cambia el estado del juego a "GameOver", detiene enemigos y muestra la pantalla de derrota.
    /// </summary>
    public void EstadoGameOver()
    {
        estadoActual = GameState.GameOver;
        enemigoController.DetenerRutinaEnemigo1();

        foreach (var esqueleto in esqueletos)
        {
            if (esqueleto != null)
                esqueleto.DetenerEnemigoEsqueletos();
            else
                Debug.LogWarning("Logica Esqueletos Desactivada");
        }

        foreach (var esqueleto in esqueletos)
        {
            if (esqueleto != null)
                esqueleto.ActivarEsqueleto();
            else
                Debug.LogWarning("MomentoGameOver fue interrumpido");
        }

        Debug.Log("Estado: GameOver");
        StartCoroutine(MostrarGameOverConDelay());

        gm.ReiniciarObjetosDelNivelActual();
    }

    /// <summary>
    /// Muestra la pantalla de Game Over con un retraso de 10 segundos.
    /// </summary>
    private IEnumerator MostrarGameOverConDelay()
    {
        if (lucesController != null)
        {
            lucesController.ActivarAmbientePerdida();
        }

        yield return null;
        yield return new WaitForSeconds(10f);

        pausarReanudar.MostrarGameOver();
    }

    /// <summary>
    /// Corrutina que sube lentamente el nivel del agua hasta una altura final.
    /// </summary>
    IEnumerator SubirAgua()
    {
        float t = 0f;
        Vector3 posicionInicial = aguaTransform.position;
        Vector3 posicionFinal = new Vector3(posicionInicial.x, alturaFinal, posicionInicial.z);

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadSubida;
            aguaTransform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return null;
        }

        aguaTransform.position = posicionFinal;
    }

    /// <summary>
    /// Aumenta el contador de puertas abiertas si el estado del juego lo permite.
    /// </summary>
    public void PuertaCerradaCorrectamente()
    {
        if (estadoActual == GameState.JugandoTutorial || estadoActual == GameState.Jugando)
        {
            puertasAbiertas++;
            CantidadPuertas.text = puertasAbiertas.ToString();
        }
    }

    /// <summary>
    /// Lógica que se ejecuta cuando se recolecta un objeto del nivel.
    /// </summary>
    public void ObjetoRecolectado()
    {
        if (gm != null)
        {
            int recolectados = gm.ObtenerObjetosRecolectados();
            textoPuntaje.text = recolectados.ToString();

            if (recolectados >= objetosNecesariosEscena)
            {
                Debug.Log("Se han recolectado todos los objetos de este nivel.");
            }
        }
    }

    /// <summary>
    /// Cierra el panel de información inicial y reanuda el juego.
    /// </summary>
    public void ReanudarInfoInicio()
    {
        Time.timeScale = 1f;

        panelInfoInicio.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.Instance.canMove = true;
    }
}
