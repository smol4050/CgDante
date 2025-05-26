using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el resaltado visual de un objeto cambiando su color.
/// </summary>
public class selectorVisual : MonoBehaviour
{
    /// <summary>
    /// Componente MeshRenderer del objeto para cambiar el color del material.
    /// </summary>
    private MeshRenderer _renderer;

    /// <summary>
    /// Color original del material para restaurar cuando se desactiva el resaltado.
    /// </summary>
    private Color _originalColor;

    /// <summary>
    /// Color usado para el resaltado visual.
    /// </summary>
    public Color colorResaltado = Color.red;

    /// <summary>
    /// Inicializa el componente MeshRenderer y guarda el color original del material.
    /// </summary>
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }

    /// <summary>
    /// Cambia el color del material al color de resaltado.
    /// </summary>
    public void ActivarResaltado()
    {
        if (_renderer != null)
        {
            _renderer.material.color = colorResaltado;
        }
    }

    /// <summary>
    /// Restaura el color original del material, desactivando el resaltado.
    /// </summary>
    public void DesactivarResaltado()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }
}
