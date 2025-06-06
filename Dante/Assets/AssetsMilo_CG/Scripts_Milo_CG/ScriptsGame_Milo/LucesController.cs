using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucesController : MonoBehaviour
{
    [Header("Luces a controlar")]
    public List<Light> luces;

    [Header("Par�metros de titileo")]
    public float tiempoTitileoMin = 0.05f;
    public float tiempoTitileoMax = 0.2f;
    public float duracionTitileo = 3f;

    [Header("Luces para ambiente de p�rdida")]
    public GameObject lucesFinales;           // Luces nuevas tipo dram�ticas
    public Light luzPrincipal;                // Directional Light
    public float nuevaIntensidad = 0.2f;      // Intensidad bajada
    public GameObject lucesIniciales;         // Luces que quieres apagar

    private Coroutine coroutineTitileo;

    // Llama este m�todo para encender las luces normalmente
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

    // Llama este m�todo para que empiecen a titilar
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

    //Nuevo m�todo para ambiente de p�rdida
    public void ActivarAmbientePerdida()
    {
        if (lucesFinales != null)
            lucesFinales.SetActive(true);

        if (luzPrincipal != null)
            luzPrincipal.intensity = nuevaIntensidad;

        if (lucesIniciales != null)
            lucesIniciales.SetActive(false);

        Debug.Log("Ambiente de p�rdida activado.");
    }
}
