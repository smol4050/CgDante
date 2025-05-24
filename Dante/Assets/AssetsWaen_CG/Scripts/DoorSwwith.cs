using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que representa un interruptor que activa una puerta.
/// Implementa la interfaz IInteractuable para permitir la interacción del jugador u otro sistema.
/// </summary>
public class DoorSwwith : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al script que controla la puerta que será activada por este interruptor.
    /// </summary>
    public NewBehaviourScript myDoor;

    /// <summary>
    /// Activa la puerta asociada llamando a su método ActivarObjeto().
    /// Este método es invocado al interactuar con el interruptor.
    /// </summary>
    public void ActivarObjeto()
    {
        myDoor.ActivarObjeto();
    }
}
