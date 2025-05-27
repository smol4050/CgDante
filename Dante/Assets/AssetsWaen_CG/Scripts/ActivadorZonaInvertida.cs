using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorZonaInvertida : MonoBehaviour
{
    public TiempoZonaInversa zonaInversa;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(zonaInversa.VolverZonaOriginal());
        }
    }
}
