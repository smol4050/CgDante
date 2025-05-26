using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa un objeto recolectable que puede ser activado/interactuado.
/// Implementa la interfaz <see cref="IInteractuable"/>.
/// </summary>
public class IInteractuableObjetoRecolectable : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Método que se llama al activar el objeto.
    /// Suma el objeto al contador del <see cref="GameManager"/> y destruye el objeto en escena.
    /// </summary>
    public void ActivarObjeto()
    {
        GameManager.Instance.SumarObjeto(); // Ya hace todo el conteo y logs
        GameObject.Destroy(gameObject);
    }
}
