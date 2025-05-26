using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum GameState
{
    PreInicio,
    JugandoTutorial,
    Jugando,
    FinJuego,
    GameOver
}
public class GameController_ParaisoOscuro : MonoBehaviour
{
    public GameState estadoActual = GameState.PreInicio;
    EnemyController_Paraiso enemigoController;
    public CronometroSupervivencia CSupervivencia;

    public InteractionEsqueletos[] esqueletos;
    public PuertaFinalRoja1 puertaFinalRoja1;
    public GameObject puertaFinalRoja2;
    public AudioSource AudioEntorno;
    public LucesController lucesController;
    public Light DirectionalLight;

    public TextMeshProUGUI CantidadPuertas;
    public TextMeshProUGUI textoPuntaje;

    public int objetosNecesariosEscena = 10;

    public int puertasAbiertas = 0;
    public int puertasNecesarias2daParte = 8;
    public int puertasNecesariasTerminar = 20;

    public PausarReanudar pausarReanudar;
    public GameObject panelInfoInicio;

    public Transform aguaTransform;
    public float alturaFinal = 6.5f;
    public float velocidadSubida = 0.05f;


    private GameManager gm;

    void Start()
    {
        estadoActual = GameState.PreInicio;
        gm = GameManager.Instance;

        if (gm != null)
        {
            // Suscribirse al evento
            gm.OnObjetoRecolectado += ObjetoRecolectado;
        }

        enemigoController = FindObjectOfType<EnemyController_Paraiso>();

        puertaFinalRoja2.SetActive(false);
        AudioEntorno.Stop();

        // Actualiza el contador de UI desde GameManager
        int recolectados = gm.ObtenerObjetosRecolectados();
        textoPuntaje.text = recolectados.ToString();

        StartCoroutine(InicioDiapositivasTutorial());
        
    }

    private void OnDestroy()
    {
        if (gm != null)
        {
            //Siempre desuscribirse para evitar memory leaks
            gm.OnObjetoRecolectado -= ObjetoRecolectado;
        }
    }

    IEnumerator InicioDiapositivasTutorial() {

        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
        panelInfoInicio.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.canMove = false;

    }

    public void EstadoJugandoTutorial()
    {
        estadoActual = GameState.JugandoTutorial;

        enemigoController.IniciarRutinaEnemigo1();
        CSupervivencia.IniciarTemporizador();
        AudioEntorno.Play();

        Debug.Log("Estado: Jugando Tutorial");
    }

    public void EstadoJugando()
    {
        estadoActual = GameState.Jugando;
        enemigoController.speedEnemy = 0.65f;

        // Transición suave a luz más baja
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

    public void EstadoFinJuego()
    {
        estadoActual = GameState.FinJuego;

        enemigoController.DetenerRutinaEnemigo1();

        // Transición a luz intensa
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

        gm.ReiniciarObjetosDelNivelActual(); // Restablece el contador de objetos recolectados

    }

    private IEnumerator MostrarGameOverConDelay()
    {

        if (lucesController != null)
        {
            lucesController.ActivarAmbientePerdida();
        }

        yield return null;

        // Esperar segundos antes de mostrar el panel
        yield return new WaitForSeconds(10f); // aquí eliges cuánto esperar

        pausarReanudar.MostrarGameOver();
    }

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

        aguaTransform.position = posicionFinal; // Asegura que termine exacto
    }


    public void PuertaCerradaCorrectamente()
    {
        if (estadoActual == GameState.JugandoTutorial || estadoActual == GameState.Jugando)
        {
            puertasAbiertas++;
            CantidadPuertas.text = puertasAbiertas.ToString();

        }
    }

    public void ObjetoRecolectado()
    {
        if (gm != null)
        {
            int recolectados = gm.ObtenerObjetosRecolectados();
            textoPuntaje.text = recolectados.ToString();

            // Si necesitas hacer algo al alcanzar todos los objetos del nivel:
            if (recolectados >= objetosNecesariosEscena)
            {
                Debug.Log("Se han recolectado todos los objetos de este nivel.");
                //gm.CompletarNivel();
                // Activar algún evento especial si aplica
            }
        }
    }

    

    public void ReanudarInfoInicio()
    {
        Time.timeScale = 1f;

        panelInfoInicio.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.Instance.canMove = true;
    }
}
