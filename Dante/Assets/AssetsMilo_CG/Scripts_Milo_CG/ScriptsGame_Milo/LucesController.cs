using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesController : MonoBehaviour
{
    [Header("Luces a controlar")]
    public List<Light> luces;

    [Header("Parámetros de titileo")]
    public float tiempoTitileoMin = 0.05f;
    public float tiempoTitileoMax = 0.2f;
    public float duracionTitileo = 3f;

    [Header("Luces para ambiente de pérdida")]
    public GameObject lucesFinales;           // Luces nuevas tipo dramáticas
    public Light luzPrincipal;                // Directional Light
    public float nuevaIntensidad = 0.5f;      // Intensidad bajada
    public GameObject lucesIniciales;         // Luces que quieres apagar

    private Coroutine coroutineTitileo;
 // Arrastra tu luz direccional desde el Inspector
    public float velocidadTransicion = 1f;

    public Coroutine transicionLuzActual;

    // Llama este método para encender las luces normalmente
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

    // Llama este método para que empiecen a titilar
    public void TitilarLuces()
    {
        if (coroutineTitileo != null)
        {
            StopCoroutine(coroutineTitileo);
        }
        coroutineTitileo = StartCoroutine(TitilarCoroutine());
    }

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

    //Nuevo método para ambiente de pérdida
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
