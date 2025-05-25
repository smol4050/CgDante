using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla una luz que parpadea encendi�ndose y apag�ndose a intervalos aleatorios.
/// </summary>
public class LuzParpadeante : MonoBehaviour
{
    /// <summary>
    /// Componente Light que se encender� y apagar�.
    /// </summary>
    public Light luz;

    /// <summary>
    /// Tiempo m�nimo en segundos entre parpadeos.
    /// </summary>
    public float tiempoMinimo = 0.1f;

    /// <summary>
    /// Tiempo m�ximo en segundos entre parpadeos.
    /// </summary>
    public float tiempoMaximo = 1.5f;

    /// <summary>
    /// Inicializa el componente Light si no se ha asignado manualmente y comienza el parpadeo.
    /// </summary>
    private void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        StartCoroutine(Parpadear());
    }

    /// <summary>
    /// Corrutina que alterna el estado de la luz a intervalos aleatorios entre <see cref="tiempoMinimo"/> y <see cref="tiempoMaximo"/>.
    /// </summary>
    /// <returns>Una enumeraci�n para controlar la ejecuci�n de la corrutina.</returns>
    private IEnumerator Parpadear()
    {
        while (true)
        {
            luz.enabled = !luz.enabled;

            float intervalo = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(intervalo);
        }
    }
}
