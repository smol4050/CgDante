using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int totalCorazones;
    public int corazonesRecolectados = 0;
    public bool misionCompletada = false;

    public TextMeshProUGUI textoMision;
    public TextMeshProUGUI textoCorazones;
    public GameObject panelCorazonesRestantes;
    public CanvasGroup panelCanvasGroup;
    public PuertaController puerta;

    public List<NewBehaviourScript> puertasBloqueadas;



    [Header("Sonidos")]
    public AudioClip sonidoCorazon;

    private AudioSource audioSource;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

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


    }


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


    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void ReproducirSonidoCorazon()
    {
        ReproducirSonido(sonidoCorazon);
    }

    public void RecolectarCorazon()
    {
        corazonesRecolectados++;

        ActualizarUI();

        if (corazonesRecolectados == 3)
        {
            foreach (var puerta in puertasBloqueadas)
            {
                puerta.DesbloquearBoton(); // Este método debe estar en tu script NewBehaviourScript
            }
        }

        if (corazonesRecolectados >= totalCorazones)
        {
            textoMision.text = "¡Completaste la misión! \nSal de la estación y sube las escaleras";
            puerta.AbrirPuerta();
        }
    }

    void ActualizarUI()
    {
        int restantes = totalCorazones - corazonesRecolectados;

        if (textoCorazones != null)
            textoCorazones.text = restantes.ToString();

        if (panelCorazonesRestantes != null)
            panelCorazonesRestantes.SetActive(restantes > 0);
    }

    public void IniciarSecuenciaDesdeIntro()
    {
        StartCoroutine(ContinuarDespuesIntro());
    }

    private IEnumerator ContinuarDespuesIntro()
    {
        panelCorazonesRestantes.SetActive(true);
        yield return StartCoroutine(FadeInPanel(panelCanvasGroup, 3f));
    }
}
