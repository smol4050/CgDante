using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la apertura del inventario y la l�gica de recolecci�n de �tems en los slots disponibles.
/// </summary>
public class Inventario : MonoBehaviour
{
    /// <summary>
    /// Determina si el inventario est� abierto o cerrado.
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
    /// Inicializa las referencias a los slots y marca los vac�os como disponibles.
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
    /// Intenta agregar un �tem al primer slot vac�o disponible en el inventario.
    /// </summary>
    /// <param name="itemObject">Objeto del �tem recogido en la escena.</param>
    /// <param name="itemID">ID �nico del �tem.</param>
    /// <param name="itemType">Tipo o categor�a del �tem.</param>
    /// <param name="itemDescription">Descripci�n del �tem.</param>
    /// <param name="itemIcon">�cono del �tem (sprite).</param>
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

