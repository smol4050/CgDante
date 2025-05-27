using UnityEngine;

/// <summary>
/// Representa una puerta que puede ser la izquierda o la derecha en el desaf�o del guardi�n.
/// </summary>
public class GatekeeperDoor : MonoBehaviour
{
    /// <summary>
    /// Indica si esta puerta es la izquierda (true) o la derecha (false).
    /// </summary>
    [Tooltip("Marca esta puerta como la izquierda (true) o la derecha (false)")]
    public bool isLeftDoor;

    /// <summary>
    /// Referencia al controlador de las puertas del guardi�n.
    /// </summary>
    private GatekeeperDoorTrigger controller;

    /// <summary>
    /// Busca y asigna el controlador de las puertas al iniciar.
    /// </summary>
    private void Start()
    {
        controller = FindObjectOfType<GatekeeperDoorTrigger>();

        if (controller == null)
        {
            Debug.LogError("No se encontr� el controlador GatekeeperDoorTrigger en la escena.");
        }
    }

    /// <summary>
    /// Detecta cuando el jugador entra en el �rea de activaci�n de la puerta y notifica al controlador.
    /// </summary>
    /// <param name="other">Colisionador del objeto que entra en el �rea de activaci�n.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            controller.DoorTriggered(isLeftDoor, other);
        }
    }
}
