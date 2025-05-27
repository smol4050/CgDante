using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representa un espacio del inventario donde puede colocarse un ítem.
/// Controla la información del ítem y su representación visual.
/// </summary>
public class Slot : MonoBehaviour
{
    /// <summary>
    /// Referencia al GameObject del ítem almacenado en este slot.
    /// </summary>
    public GameObject item;

    /// <summary>
    /// Identificador único del ítem almacenado.
    /// </summary>
    public int ID;

    /// <summary>
    /// Tipo del ítem (ej. "arma", "consumible", etc.).
    /// </summary>
    public string tipo;

    /// <summary>
    /// Descripción del ítem, usada para mostrar detalles en la UI.
    /// </summary>
    public string descripcion;

    /// <summary>
    /// Indica si el slot está vacío y puede recibir un ítem.
    /// </summary>
    public bool empty;

    /// <summary>
    /// Sprite del ícono del ítem mostrado en la UI del inventario.
    /// </summary>
    public Sprite icon;

    // Referencia a la imagen UI del ítem (componente Image del hijo).
    private Image itemImage;

    /// <summary>
    /// Inicializa el componente de imagen y la oculta si no hay ítem.
    /// </summary>
    void Start()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemImage.enabled = false; // Ocultamos inicialmente
    }

    /// <summary>
    /// Actualiza la imagen del slot con el ícono del ítem asignado.
    /// </summary>
    public void UpdateSlot()
    {
        if (icon != null)
        {
            itemImage.sprite = icon;
            itemImage.enabled = true;
        }
    }
}
