using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa el progreso de un nivel específico en el juego.
/// </summary>
[System.Serializable]
public class ProgresoNivel
{
    /// <summary>
    /// Nombre identificador del nivel.
    /// </summary>
    public string nombreNivel;

    /// <summary>
    /// Cantidad de objetos recolectados en este nivel.
    /// </summary>
    public int objetosRecolectados;

    /// <summary>
    /// Indica si el nivel ha sido completado.
    /// </summary>
    public bool nivelCompletado;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProgresoNivel"/> con el nombre del nivel.
    /// </summary>
    /// <param name="nombre">El nombre identificador del nivel.</param>
    public ProgresoNivel(string nombre)
    {
        nombreNivel = nombre;
        objetosRecolectados = 0;
        nivelCompletado = false;
    }
}

