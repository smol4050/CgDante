using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorZonaOriginal : MonoBehaviour
{
    public TiempoZonaInversa zonaInversa;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zonaInversa.ActivarContador();
        }
    }
}
