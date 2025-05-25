using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla una luz que parpadea encendiéndose y apagándose a intervalos aleatorios.
/// </summary>
public class LuzParpadeante : MonoBehaviour
{
    /// <summary>
    /// Componente Light que se encenderá y apagará.
    /// </summary>
    public Light luz;

    /// <summary>
    /// Tiempo mínimo en segundos entre parpadeos.
    /// </summary>
    public float tiempoMinimo = 0.1f;

    /// <summary>
    /// Tiempo máximo en segundos entre parpadeos.
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
    /// <returns>Una enumeración para controlar la ejecución de la corrutina.</returns>
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
