using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el comportamiento de cada baldosa en la cuadrícula:
/// materiales, triggers y estados de resaltado.
/// </summary>
public class TileScript : MonoBehaviour
{
    /// <summary>Coordenada lógica X en la cuadrícula.</summary>
    public int x, y;
    /// <summary>Material base de la baldosa.</summary>
    public Material defaultMaterial;
    /// <summary>Material de iluminación/resaltado.</summary>
    public Material highlightMaterial;

    private Renderer rend;
    private BoxCollider boxCollider;

    /// <summary>
    /// Inicializa referencias a Renderer y BoxCollider,
    /// y aplica el material por defecto.
    /// </summary>
    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = defaultMaterial;
    }

    /// <summary>
    /// Ilumina la baldosa y la bloquea (isTrigger = false).
    /// </summary>
    public void Highlight()
    {
        boxCollider.isTrigger = false;
        rend.sharedMaterial = highlightMaterial;
    }

    /// <summary>
    /// Oculta la iluminación restaurando el material por defecto,
    /// sin modificar el estado de trigger.
    /// </summary>
    public void HideHighlight()
    {
        rend.sharedMaterial = defaultMaterial;
    }

    /// <summary>
    /// Restaura el estado inicial: material por defecto y
    /// trigger activo (isTrigger = true).
    /// </summary>
    public void ResetTile()
    {
        boxCollider.isTrigger = true;
        rend.sharedMaterial = defaultMaterial;
    }
}
