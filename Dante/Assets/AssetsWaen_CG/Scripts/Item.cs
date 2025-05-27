using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un �tem que puede ser recogido en el mundo del juego.
/// Almacena datos como ID, tipo, descripci�n e �cono del �tem.
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// Identificador �nico del �tem.
    /// </summary>
    public int ID;

    /// <summary>
    /// Categor�a o tipo del �tem (ej. "consumible", "arma", "llave").
    /// </summary>
    public string tipo;

    /// <summary>
    /// Descripci�n breve del �tem, �til para mostrar en el UI.
    /// </summary>
    public string descripcion;

    /// <summary>
    /// �cono visual del �tem que se muestra en el inventario.
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// Indica si el �tem ya ha sido recogido por el jugador.
    /// Es oculto en el Inspector de Unity.
    /// </summary>
    [HideInInspector]
    public bool pickedUp;
}
