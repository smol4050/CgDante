using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    PreInicio,
    JugandoTutorial,
    Jugando,
    FinJuego
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
    //InteractionDoors InteraccionPuerta;

    public TextMeshProUGUI CantidadPuertas;

    private GameManager gm;
    public int objetosRecolectadosEscena = 0;
    public int objetosNecesariosEscena = 10;
    public TextMeshProUGUI textoPuntaje;

    public int puertasAbiertas= 0;
    public int puertasNecesarias2daParte = 8;
    public int puertasNecesariasTerminar = 20;

    void Start()
    {
        estadoActual = GameState.PreInicio;
        gm = GameManager.Instance;
        enemigoController = FindObjectOfType<EnemyController_Paraiso>();

        puertaFinalRoja2.SetActive(false); // Desactiva la puerta final roja 2 al inicio
        AudioEntorno.Stop();
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
                Debug.LogWarning("Un esqueleto est� sin asignar en el array.");
        }
        lucesController.TitilarLuces(); // Inicia el titileo de luces

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
                Debug.LogWarning("Un esuqeleto no se pudo apagar");
        }

        lucesController.EncenderLuzNormal(); // Detiene el titileo de luces

        puertaFinalRoja1.AbrirPuerta(); // Abre la puerta final
        puertaFinalRoja2.SetActive(true); // Activa la puerta final roja 2
        AudioEntorno.Stop(); // Detiene el audio del entorno

        //InteractionDoors.CerrarTodasLasPuertas();

        Debug.Log("Estado: FinJuego");
    }

    public void PuertaCerradaCorrectamente()
    {
        if(estadoActual == GameState.JugandoTutorial || estadoActual == GameState.Jugando)
        {
            puertasAbiertas++;
            CantidadPuertas.text = puertasAbiertas.ToString();
            //gm.SumarObjeto();
            if (puertasAbiertas == puertasNecesarias2daParte && estadoActual == GameState.JugandoTutorial)
            {
                EstadoJugando();
                Debug.Log("Estado: JugandoEsqueletos");
            }
            if (puertasAbiertas == puertasNecesariasTerminar)
            {
                EstadoFinJuego(); //Detiene la rutina del enemigoPuertas
                Debug.Log("Estado: FinJuego");
            }
        }
    }

    public void ObjetoRecolectado()
    {
        if (objetosRecolectadosEscena < objetosNecesariosEscena)
            if (gm != null)
            {
            objetosRecolectadosEscena++;
            textoPuntaje.text = objetosRecolectadosEscena.ToString();
            gm.SumarObjeto();
        }
        
    }
}
