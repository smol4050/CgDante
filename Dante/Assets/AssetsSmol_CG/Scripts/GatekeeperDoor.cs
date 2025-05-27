using UnityEngine;

public class GatekeeperDoor : MonoBehaviour
{
    [Tooltip("Marca esta puerta como la izquierda (true) o la derecha (false)")]
    public bool isLeftDoor;

    private GatekeeperDoorTrigger controller;

    private void Start()
    {
        controller = FindObjectOfType<GatekeeperDoorTrigger>();

        if (controller == null)
        {
            Debug.LogError("No se encontró el controlador GatekeeperDoorTrigger en la escena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            controller.DoorTriggered(isLeftDoor, other);
        }
    }
}
