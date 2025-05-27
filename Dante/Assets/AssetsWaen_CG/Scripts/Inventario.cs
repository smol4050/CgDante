using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la apertura del inventario y la lógica de recolección de ítems en los slots disponibles.
/// </summary>
public class Inventario : MonoBehaviour
{
    /// <summary>
    /// Determina si el inventario está abierto o cerrado.
    /// </summary>
    private bool inventarioAbierto;

    /// <summary>
    /// Referencia al panel visual del inventario (UI).
    /// </summary>
    public GameObject inventario;

    /// <summary>
    /// Total de slots disponibles en el inventario.
    /// </summary>
    private int allSlots;

    /// <summary>
    /// Arreglo que almacena las referencias a cada slot del inventario.
    /// </summary>
    private GameObject[] slot;

    /// <summary>
    /// Objeto contenedor que tiene como hijos los slots.
    /// </summary>
    public GameObject slotHolder;

    /// <summary>
    /// Inicializa las referencias a los slots y marca los vacíos como disponibles.
    /// </summary>
    void Start()
    {
        allSlots = slotHolder.transform.childCount;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    /// <summary>
    /// Alterna la visibilidad del inventario al presionar la tecla 'I'.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventarioAbierto = !inventarioAbierto;
        }

        inventario.SetActive(inventarioAbierto);
    }

    /// <summary>
    /// Intenta agregar un ítem al primer slot vacío disponible en el inventario.
    /// </summary>
    /// <param name="itemObject">Objeto del ítem recogido en la escena.</param>
    /// <param name="itemID">ID único del ítem.</param>
    /// <param name="itemType">Tipo o categoría del ítem.</param>
    /// <param name="itemDescription">Descripción del ítem.</param>
    /// <param name="itemIcon">Ícono del ítem (sprite).</param>
    public void RecogerItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            Slot currentSlot = slot[i].GetComponent<Slot>();
            if (currentSlot.empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                currentSlot.item = itemObject;
                currentSlot.ID = itemID;
                currentSlot.tipo = itemType;
                currentSlot.descripcion = itemDescription;
                currentSlot.icon = itemIcon;
                currentSlot.empty = false;

                itemObject.SetActive(false); // Ocultamos el objeto del mundo
                currentSlot.UpdateSlot();

                return;
            }
        }

        Debug.Log("Inventario lleno. No se pudo recoger el objeto.");
    }
}

