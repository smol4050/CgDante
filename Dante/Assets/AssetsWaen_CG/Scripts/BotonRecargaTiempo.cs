using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que representa un bot�n interactuable para recargar el tiempo
/// en una zona de inversi�n. Implementa la interfaz <c>IInteractuable</c>.
/// </summary>
public class BotonRecargaTiempo : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al componente que gestiona la l�gica de tiempo en la zona inversa.
    /// </summary>
    public TiempoZonaInversa zonaInversa;

    /// <summary>
    /// M�todo invocado al interactuar con el bot�n. Recarga el tiempo
    /// de la <see cref="zonaInversa"/> si est� asignada.
    /// </summary>
    public void ActivarObjeto()
    {
        if (zonaInversa != null)
        {
            zonaInversa.RecargarTiempo();
            Debug.Log("Tiempo recargado desde el bot�n.");
        }
    }
}
