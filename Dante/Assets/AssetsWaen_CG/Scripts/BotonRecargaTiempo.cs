using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonRecargaTiempo : MonoBehaviour, IInteractuable
{

    public TiempoZonaInversa zonaInversa;

    public void ActivarObjeto()
    {
        if (zonaInversa != null)
        {
            zonaInversa.RecargarTiempo();
            Debug.Log("Tiempo recargado desde el botón.");
        }
    }
}
