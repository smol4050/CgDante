using UnityEngine;

/// <summary>
/// Representa una puerta que puede ser la izquierda o la derecha en el desafío del guardián.
/// </summary>
public class GatekeeperDoor : MonoBehaviour
{
    /// <summary>
    /// Indica si esta puerta es la izquierda (true) o la derecha (false).
    /// </summary>
    [Tooltip("Marca esta puerta como la izquierda (true) o la derecha (false)")]
    public bool isLeftDoor;

    /// <summary>
    /// Referencia al controlador de las puertas del guardián.
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
            Debug.LogError("No se encontró el controlador GatekeeperDoorTrigger en la escena.");
        }
    }

    /// <summary>
    /// Detecta cuando el jugador entra en el área de activación de la puerta y notifica al controlador.
    /// </summary>
    /// <param name="other">Colisionador del objeto que entra en el área de activación.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            controller.DoorTriggered(isLeftDoor, other);
        }
    }
}
