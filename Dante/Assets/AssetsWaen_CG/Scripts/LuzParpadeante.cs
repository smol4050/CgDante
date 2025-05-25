using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    public Light luz; // Asigna tu componente Light aquí
    public float tiempoMinimo = 0.1f;
    public float tiempoMaximo = 1.5f;

    private void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        StartCoroutine(Parpadear());
    }

    System.Collections.IEnumerator Parpadear()
    {
        while (true)
        {
            luz.enabled = !luz.enabled;

            float intervalo = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(intervalo);
        }
    }
}

