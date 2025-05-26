using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla un grupo de luces con funcionalidades para encenderlas, hacerlas titilar y configurar un ambiente de pérdida.
/// </summary>
public class LucesController : MonoBehaviour
{
    /// <summary>
    /// Lista de luces que serán controladas.
    /// </summary>
    [Header("Luces a controlar")]
    public List<Light> luces;

    /// <summary>
    /// Tiempo mínimo del intervalo de titileo.
    /// </summary>
    [Header("Parámetros de titileo")]
    public float tiempoTitileoMin = 0.05f;

    /// <summary>
    /// Tiempo máximo del intervalo de titileo.
    /// </summary>
    public float tiempoTitileoMax = 0.2f;

    /// <summary>
    /// Duración total del efecto de titileo.
    /// </summary>
    public float duracionTitileo = 3f;

    /// <summary>
    /// Objeto que contiene luces nuevas para el ambiente de pérdida (luces dramáticas).
    /// </summary>
    [Header("Luces para ambiente de pérdida")]
    public GameObject lucesFinales;

    /// <summary>
    /// Luz principal del tipo Directional Light.
    /// </summary>
    public Light luzPrincipal;

    /// <summary>
    /// Nueva intensidad a la que se quiere llevar la luz principal en la transición.
    /// </summary>
    public float nuevaIntensidad = 0.5f;

    /// <summary>
    /// Objeto que contiene las luces iniciales que serán apagadas en el ambiente de pérdida.
    /// </summary>
    public GameObject lucesIniciales;

    /// <summary>
    /// Referencia a la corrutina que controla el titileo para poder detenerla.
    /// </summary>
    private Coroutine coroutineTitileo;

    /// <summary>
    /// Velocidad de la transición para cambios de intensidad de luz.
    /// </summary>
    // Arrastra tu luz direccional desde el Inspector
    public float velocidadTransicion = 1f;

    /// <summary>
    /// Corrutina actual que controla la transición de la luz principal.
    /// </summary>
    public Coroutine transicionLuzActual;

    /// <summary>
    /// Enciende todas las luces normalmente y detiene cualquier titileo en curso.
    /// </summary>
    public void EncenderLuzNormal()
    {
        if (coroutineTitileo != null)
        {
            StopCoroutine(coroutineTitileo);
        }

        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = true;
        }
    }

    /// <summary>
    /// Inicia el efecto de titileo de las luces, alternando su estado encendido/apagado.
    /// </summary>
    public void TitilarLuces()
    {
        if (coroutineTitileo != null)
        {
            StopCoroutine(coroutineTitileo);
        }
        coroutineTitileo = StartCoroutine(TitilarCoroutine());
    }

    /// <summary>
    /// Corrutina que alterna el encendido/apagado de las luces por un tiempo determinado, con intervalos aleatorios.
    /// </summary>
    private IEnumerator TitilarCoroutine()
    {
        float tiempoPasado = 0f;

        while (tiempoPasado < duracionTitileo)
        {
            foreach (Light luz in luces)
            {
                if (luz != null)
                    luz.enabled = !luz.enabled;
            }

            float tiempoEspera = Random.Range(tiempoTitileoMin, tiempoTitileoMax);
            yield return new WaitForSeconds(tiempoEspera);
            tiempoPasado += tiempoEspera;
        }

        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = false;
        }

        coroutineTitileo = null;
    }

    /// <summary>
    /// Activa el ambiente de pérdida, encendiendo luces dramáticas, ajustando la intensidad de la luz principal y apagando las luces iniciales.
    /// </summary>
    public void ActivarAmbientePerdida()
    {
        if (lucesFinales != null)
            lucesFinales.SetActive(true);

        if (luzPrincipal != null)
            StartCoroutine(TransicionIntensidadLuz(nuevaIntensidad));

        if (lucesIniciales != null)
            lucesIniciales.SetActive(false);

        Debug.Log("Ambiente de pérdida activado.");
    }

    /// <summary>
    /// Corrutina para hacer una transición suave en la intensidad de la luz principal.
    /// </summary>
    /// <param name="intensidadObjetivo">Valor final de la intensidad que la luz principal debe alcanzar.</param>
    /// <returns>IEnumerator para la corrutina.</returns>
    public IEnumerator TransicionIntensidadLuz(float intensidadObjetivo)
    {
        float intensidadInicial = luzPrincipal.intensity;
        float duracion = 1f / velocidadTransicion;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            luzPrincipal.intensity = Mathf.Lerp(intensidadInicial, intensidadObjetivo, tiempo / duracion);
            yield return null;
        }

        luzPrincipal.intensity = intensidadObjetivo;
    }
}
