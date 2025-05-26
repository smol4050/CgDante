using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controlador principal del juego. Gestiona el progreso de la misión,
/// la recolección de corazones y objetos, la UI y la activación de puertas.
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// Instancia única del GameController (patrón Singleton).
    /// </summary>
    /// 
    //public static GameController Instance;

    private GameManager gm;

    /// <summary>
    /// Total de corazones disponibles en la escena.
    /// </summary>
    public int totalCorazones;

    /// <summary>
    /// Cantidad de corazones recolectados por el jugador.
    /// </summary>
    public int corazonesRecolectados = 0;

    /// <summary>
    /// Indica si la misión fue completada.
    /// </summary>
    public bool misionCompletada = false;

    /// <summary>
    /// Texto que muestra el estado de la misión.
    /// </summary>
    public TextMeshProUGUI textoMision;

    /// <summary>
    /// Texto que muestra la cantidad de corazones restantes.
    /// </summary>
    public TextMeshProUGUI textoCorazones;

    /// <summary>
    /// Panel que indica los corazones restantes.
    /// </summary>
    public GameObject panelCorazonesRestantes;

    /// <summary>
    /// Componente CanvasGroup para hacer efectos de fade.
    /// </summary>
    public CanvasGroup panelCanvasGroup;

    /// <summary>
    /// Referencia al controlador de la puerta principal.
    /// </summary>
    public PuertaController puerta;

    /// <summary>
    /// Texto que muestra el puntaje de objetos recolectados.
    /// </summary>
    public TextMeshProUGUI textoPuntaje;

    /// <summary>
    /// Cantidad de objetos necesarios para completar la escena.
    /// </summary>
    public int objetosNecesariosEscena = 10;

    /// <summary>
    /// Lista de puertas bloqueadas que se desbloquean al recolectar corazones.
    /// </summary>
    public List<NewBehaviourScript> puertasBloqueadas;

    [Header("Sonidos")]
    /// <summary>
    /// Clip de audio que se reproduce al recolectar un corazón.
    /// </summary>
    public AudioClip sonidoCorazon;

    private AudioSource audioSource;

    /// <summary>
    /// Inicializa la instancia del GameController y asegura que sea única.
    /// </summary>
    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    /// <summary>
    /// Inicializa referencias y el estado inicial del juego.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        totalCorazones = GameObject.FindGameObjectsWithTag("Objetivo").Length;
        corazonesRecolectados = 0;

        panelCorazonesRestantes.SetActive(false);
        panelCanvasGroup.alpha = 0f;
        ActualizarUI();

        gm = GameManager.Instance;

        if (gm != null)
        {
            gm.OnObjetoRecolectado += ObjetoRecolectado;
        }

        int recolectados = gm.ObtenerObjetosRecolectados();
        textoPuntaje.text = recolectados.ToString();
    }

    /// <summary>
    /// Se llama cuando el GameController es destruido. Se desuscribe de eventos.
    /// </summary>
    private void OnDestroy()
    {
        if (gm != null)
        {
            gm.OnObjetoRecolectado -= ObjetoRecolectado;
        }
    }

    /// <summary>
    /// Realiza un efecto de aparición gradual (fade in) en un CanvasGroup.
    /// </summary>
    /// <param name="canvasGroup">Grupo de Canvas a animar.</param>
    /// <param name="duration">Duración del efecto en segundos.</param>
    private IEnumerator FadeInPanel(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// Reproduce un clip de audio específico.
    /// </summary>
    /// <param name="clip">Clip de audio a reproducir.</param>
    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Reproduce el sonido específico del corazón.
    /// </summary>
    public void ReproducirSonidoCorazon()
    {
        ReproducirSonido(sonidoCorazon);
    }

    /// <summary>
    /// Maneja la lógica de recolección de un corazón.
    /// Actualiza UI y desbloquea puertas si se cumplen condiciones.
    /// </summary>
    public void RecolectarCorazon()
    {
        corazonesRecolectados++;

        ActualizarUI();

        if (corazonesRecolectados == 3)
        {
            foreach (var puerta in puertasBloqueadas)
            {
                puerta.DesbloquearBoton();
            }
        }

        if (corazonesRecolectados >= totalCorazones)
        {
            textoMision.text = "¡Completaste la misión! \nSal de la estación y sube las escaleras";
            puerta.AbrirPuerta();
        }
    }

    /// <summary>
    /// Actualiza la UI relacionada con los corazones restantes.
    /// </summary>
    void ActualizarUI()
    {
        int restantes = totalCorazones - corazonesRecolectados;

        if (textoCorazones != null)
            textoCorazones.text = restantes.ToString();

        if (panelCorazonesRestantes != null)
            panelCorazonesRestantes.SetActive(restantes > 0);
    }

    /// <summary>
    /// Inicia la secuencia desde el panel de introducción.
    /// </summary>
    public void IniciarSecuenciaDesdeIntro()
    {
        StartCoroutine(ContinuarDespuesIntro());
    }

    /// <summary>
    /// Coroutine para mostrar gradualmente el panel tras la intro.
    /// </summary>
    private IEnumerator ContinuarDespuesIntro()
    {
        panelCorazonesRestantes.SetActive(true);
        yield return StartCoroutine(FadeInPanel(panelCanvasGroup, 3f));
    }

    /// <summary>
    /// Se llama cuando se recolecta un objeto genérico. Actualiza el puntaje y verifica si se completa el nivel.
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
                // gm.CompletarNivel();
                // Activar evento especial si aplica
            }
        }
    }
}
