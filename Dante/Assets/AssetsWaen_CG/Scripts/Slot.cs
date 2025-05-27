using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Representa un espacio del inventario donde puede colocarse un �tem.
/// Controla la informaci�n del �tem y su representaci�n visual.
/// </summary>
public class Slot : MonoBehaviour
{
    /// <summary>
    /// Referencia al GameObject del �tem almacenado en este slot.
    /// </summary>
    public GameObject item;

    /// <summary>
    /// Identificador �nico del �tem almacenado.
    /// </summary>
    public int ID;

    /// <summary>
    /// Tipo del �tem (ej. "arma", "consumible", etc.).
    /// </summary>
    public string tipo;

    /// <summary>
    /// Descripci�n del �tem, usada para mostrar detalles en la UI.
    /// </summary>
    public string descripcion;

    /// <summary>
    /// Indica si el slot est� vac�o y puede recibir un �tem.
    /// </summary>
    public bool empty;

    /// <summary>
    /// Sprite del �cono del �tem mostrado en la UI del inventario.
    /// </summary>
    public Sprite icon;

    // Referencia a la imagen UI del �tem (componente Image del hijo).
    private Image itemImage;

    /// <summary>
    /// Inicializa el componente de imagen y la oculta si no hay �tem.
    /// </summary>
    void Start()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemImage.enabled = false; // Ocultamos inicialmente
    }

    /// <summary>
    /// Actualiza la imagen del slot con el �cono del �tem asignado.
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
