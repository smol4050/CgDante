using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Representa el progreso guardado de un nivel específico,
/// incluyendo su nombre, cantidad de objetos recolectados y si fue completado.
/// </summary>
[System.Serializable]
public class ProgresoNivelGuardado
{
    /// <summary>
    /// Nombre del nivel.
    /// </summary>
    public string nombreNivel;

    /// <summary>
    /// Cantidad de objetos recolectados en ese nivel.
    /// </summary>
    public int objetosRecolectados;

    /// <summary>
    /// Indica si el nivel fue completado.
    /// </summary>
    public bool nivelCompletado;
    public string nombrePartida;

}

/// <summary>
/// Contenedor general para guardar el progreso de todos los niveles del juego.
/// </summary>
[System.Serializable]
public class DatosGuardados
{
    /// <summary>
    /// Nombre de la partida.
    /// </summary>
    public string nombrePartida;

    /// <summary>
    /// Lista del progreso registrado por cada nivel.
    /// </summary>
    public List<ProgresoNivelGuardado> progresoPorNivel = new List<ProgresoNivelGuardado>();
}

