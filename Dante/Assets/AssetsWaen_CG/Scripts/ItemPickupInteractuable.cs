using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupInteractuable : MonoBehaviour, IInteractuable
{
    private Inventario inventario;
    private Item item;

    private void Start()
    {
        inventario = FindObjectOfType<Inventario>();
        item = GetComponent<Item>();
    }

    public void ActivarObjeto()
    {
        if (item != null && inventario != null && !item.pickedUp)
        {
            inventario.RecogerItem(gameObject, item.ID, item.tipo, item.descripcion, item.icon);
        }
    }
}
