using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que permite recoger un objeto del mundo e integrarlo al inventario.
/// Implementa la interfaz <see cref="IInteractuable"/> para permitir la interacción.
/// </summary>
public class ItemPickupInteractuable : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al sistema de inventario del jugador.
    /// </summary>
    private Inventario inventario;

    /// <summary>
    /// Referencia al componente <see cref="Item"/> asociado a este objeto.
    /// </summary>
    private Item item;

    /// <summary>
    /// Inicializa las referencias al inventario y al objeto <see cref="Item"/>.
    /// </summary>
    private void Start()
    {
        inventario = FindObjectOfType<Inventario>();
        item = GetComponent<Item>();
    }

    /// <summary>
    /// Método llamado al interactuar con el objeto.
    /// Si el ítem no ha sido recogido, lo añade al inventario.
    /// </summary>
    public void ActivarObjeto()
    {
        if (item != null && inventario != null && !item.pickedUp)
        {
            inventario.RecogerItem(gameObject, item.ID, item.tipo, item.descripcion, item.icon);
        }
    }
}
