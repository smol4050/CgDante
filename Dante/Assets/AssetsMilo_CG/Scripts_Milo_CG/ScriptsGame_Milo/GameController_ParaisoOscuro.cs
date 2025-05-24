using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public InteractionEsqueletos[] esqueletos;
    public PuertaFinalRoja1 puertaFinalRoja1;
    public GameObject puertaFinalRoja2;
    public AudioSource AudioEntorno;
    public LucesController lucesController;

    public TextMeshProUGUI CantidadPuertas;
    public TextMeshProUGUI textoPuntaje;

    public int objetosNecesariosEscena = 10;

    public int puertasAbiertas = 0;
    public int puertasNecesarias2daParte = 8;
    public int puertasNecesariasTerminar = 20;

    public PausarReanudar pausarReanudar;


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
    }

    private void OnDestroy()
    {
        if (gm != null)
        {
            //Siempre desuscribirse para evitar memory leaks
            gm.OnObjetoRecolectado -= ObjetoRecolectado;
        }
    }

    public void EstadoJugandoTutorial()
    {
        estadoActual = GameState.JugandoTutorial;
        enemigoController.IniciarRutinaEnemigo1();
        AudioEntorno.Play();

        Debug.Log("Estado: Jugando Tutorial");
    }

    public void EstadoJugando()
    {
        estadoActual = GameState.Jugando;

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

        foreach (var esqueleto in esqueletos)
        {
            if (esqueleto != null)
                esqueleto.DesactivarEsqueletos();
            else
                Debug.LogWarning("Un esqueleto no se pudo apagar");
        }

        lucesController.EncenderLuzNormal();
        puertaFinalRoja1.AbrirPuerta();
        puertaFinalRoja2.SetActive(true);
        AudioEntorno.Stop();

        Debug.Log("Estado: FinJuego");
    }

    public void EstadoGameOver()
    {
        estadoActual = GameState.GameOver;
        //enemigoController.DetenerRutinaEnemigo1();
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


    public void PuertaCerradaCorrectamente()
    {
        if (estadoActual == GameState.JugandoTutorial || estadoActual == GameState.Jugando)
        {
            puertasAbiertas++;
            CantidadPuertas.text = puertasAbiertas.ToString();

            if (puertasAbiertas == puertasNecesarias2daParte && estadoActual == GameState.JugandoTutorial)
            {
                EstadoJugando();
                Debug.Log("Estado: JugandoEsqueletos");
            }

            if (puertasAbiertas == puertasNecesariasTerminar)
            {
                EstadoFinJuego();
                gm.CompletarNivel();
                Debug.Log("Estado: FinJuego");
            }
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
}
