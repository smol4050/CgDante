using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que representa un botón interactuable para recargar el tiempo
/// en una zona de inversión. Implementa la interfaz <c>IInteractuable</c>.
/// </summary>
public class BotonRecargaTiempo : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al componente que gestiona la lógica de tiempo en la zona inversa.
    /// </summary>
    public TiempoZonaInversa zonaInversa;

    /// <summary>
    /// Método invocado al interactuar con el botón. Recarga el tiempo
    /// de la <see cref="zonaInversa"/> si está asignada.
    /// </summary>
    public void ActivarObjeto()
    {
        if (zonaInversa != null)
        {
            zonaInversa.RecargarTiempo();
            Debug.Log("Tiempo recargado desde el botón.");
        }
    }
}
