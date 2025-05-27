using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un ítem que puede ser recogido en el mundo del juego.
/// Almacena datos como ID, tipo, descripción e ícono del ítem.
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// Identificador único del ítem.
    /// </summary>
    public int ID;

    /// <summary>
    /// Categoría o tipo del ítem (ej. "consumible", "arma", "llave").
    /// </summary>
    public string tipo;

    /// <summary>
    /// Descripción breve del ítem, útil para mostrar en el UI.
    /// </summary>
    public string descripcion;

    /// <summary>
    /// Ícono visual del ítem que se muestra en el inventario.
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// Indica si el ítem ya ha sido recogido por el jugador.
    /// Es oculto en el Inspector de Unity.
    /// </summary>
    [HideInInspector]
    public bool pickedUp;
}
