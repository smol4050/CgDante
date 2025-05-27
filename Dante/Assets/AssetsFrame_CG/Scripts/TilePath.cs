using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contenedor serializable que representa un camino de memoria.
/// Cada entrada es el GameObject padre que contiene al <see cref="TileScript"/>.
/// </summary>
[System.Serializable]
public class TilePath
{
    /// <summary>Nombre descriptivo del camino.</summary>
    public string name;

    /// <summary>
    /// Lista de GameObjects que contienen un <see cref="TileScript"/>,
    /// en el orden que debe seguirse.
    /// </summary>
    [Tooltip("Arrastra aquí el GameObject que CONTIENE al TileScript")]
    public List<GameObject> tileParents = new List<GameObject>();
}
