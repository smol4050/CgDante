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

    private Coroutine coroutineTitileo;

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

        // Al final, las apaga o prende según lo que necesites
        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = false; // o true si quieres que terminen encendidas
        }

        coroutineTitileo = null;
    }
}
