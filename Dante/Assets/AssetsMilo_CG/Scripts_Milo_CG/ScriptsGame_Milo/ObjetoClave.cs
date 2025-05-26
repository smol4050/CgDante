using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un objeto clave que puede ser recolectado e interactuado en el juego.
/// Implementa la interfaz <see cref="IInteractuable"/> para permitir la interacci�n.
/// </summary>
public class ObjetoClave : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Indica si el objeto ya fue recolectado.
    /// </summary>
    private bool recolectado = false;

    /// <summary>
    /// Referencia al controlador principal del juego <see cref="GameController_ParaisoOscuro"/>.
    /// </summary>
    GameController_ParaisoOscuro Gm;

    /// <summary>
    /// Inicializa la referencia al controlador del juego y muestra advertencia si no se encuentra.
    /// </summary>
    void Start()
    {
        Gm = FindObjectOfType<GameController_ParaisoOscuro>();
        if (Gm == null)
        {
            Debug.LogWarning("No se encontr� GameController en la escena.");
        }
    }

    /// <summary>
    /// M�todo para activar el objeto cuando se interact�a.
    /// Marca el objeto como recolectado, notifica al GameController y destruye el objeto en la escena.
    /// </summary>
    public void ActivarObjeto()
    {
        if (recolectado) return;

        recolectado = true;
        Gm.ObjetoRecolectado();

        // Feedback visual/audio
        Debug.Log("Objeto clave recolectado: " + gameObject.name);

        // Desactivar o destruir objeto
        Destroy(gameObject);
    }
}
