using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador para una puerta que se activa mediante un botón físico en el escenario.
/// Incluye movimientos suaves de puerta y botón, control de acceso y reproducción de sonidos.
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias de Objetos")]
    /// <summary>
    /// Referencia al objeto que actúa como botón físico.
    /// </summary>
    public GameObject button;

    /// <summary>
    /// Referencia al objeto que representa la puerta.
    /// </summary>
    public GameObject doorObject;

    /// <summary>
    /// Referencia al objeto que representa una pared (no usado en el código).
    /// </summary>
    public GameObject wallObject;

    /// <summary>
    /// Referencia al objeto que representa una columna (no usado en el código).
    /// </summary>
    public GameObject columnObject;

    [Header("Posiciones de Movimiento")]
    /// <summary>
    /// Transform que define la posición abierta de la puerta.
    /// </summary>
    public Transform openPositionTransform;

    /// <summary>
    /// Transform que define la posición cerrada de la puerta.
    /// </summary>
    public Transform closedPositionTransform;

    /// <summary>
    /// Transform que define la posición abierta del botón.
    /// </summary>
    public Transform openPositionTransformB;

    /// <summary>
    /// Transform que define la posición cerrada del botón.
    /// </summary>
    public Transform closedPositionTransformB;

    [Header("Estados")]
    /// <summary>
    /// Indica si el botón está desbloqueado y puede usarse.
    /// </summary>
    public bool botonDesbloqueado = false;

    /// <summary>
    /// Estado actual de la puerta: true = abierta, false = cerrada.
    /// </summary>
    public bool doorState = false;

    /// <summary>
    /// Estado actual del botón: true = presionado, false = sin presionar.
    /// </summary>
    public bool buttonState = false;

    [Header("Velocidades")]
    /// <summary>
    /// Velocidad de movimiento de la puerta.
    /// </summary>
    public float doorSpeed = 2f;

    /// <summary>
    /// Referencia a la coroutine actualmente en ejecución.
    /// </summary>
    private Coroutine currentCoroutine;

    [Header("Audio")]
    /// <summary>
    /// Componente AudioSource para reproducir sonidos.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip de audio que se reproduce al abrir la puerta.
    /// </summary>
    public AudioClip audioOpen;

    /// <summary>
    /// Clip de audio que se reproduce al cerrar la puerta.
    /// </summary>
    public AudioClip audioClose;

    /// <summary>
    /// Clip de audio que se reproduce si la puerta está bloqueada.
    /// </summary>
    public AudioClip audioBloqueado;

    /// <summary>
    /// Inicializa el estado de la puerta y el botón al iniciar la escena.
    /// </summary>
    private void Start()
    {
        SetDoorState(doorState);
        SetDoorState(buttonState);
    }

    /// <summary>
    /// Establece la posición de la puerta y del botón según el estado indicado.
    /// </summary>
    /// <param name="s">Estado deseado: true para abierto, false para cerrado.</param>
    private void SetDoorState(bool s)
    {
        if (s)
        {
            button.transform.position = openPositionTransformB.position;
            doorObject.transform.position = openPositionTransform.position;
        }
        else
        {
            button.transform.position = closedPositionTransformB.position;
            doorObject.transform.position = closedPositionTransform.position;
        }
    }

    /// <summary>
    /// Inicia la apertura animada de la puerta.
    /// </summary>
    public void OpenDoor()
    {
        Debug.Log("Open Door");
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeDoorState(true));
    }

    /// <summary>
    /// Inicia el cierre animado de la puerta.
    /// </summary>
    public void CloseDoor()
    {
        Debug.Log("Close Door");
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ChangeDoorState(false));
    }

    /// <summary>
    /// Coroutine que anima el cambio de estado de la puerta y la animación del botón.
    /// </summary>
    /// <param name="newState">Nuevo estado de la puerta: true para abrir, false para cerrar.</param>
    /// <returns>IEnumerator para coroutine.</returns>
    private IEnumerator ChangeDoorState(bool newState)
    {
        Transform doorTargetPosition = newState ? openPositionTransform : closedPositionTransform;
        Vector3 buttonPressedPosition = openPositionTransformB.position;
        Vector3 buttonOriginalPosition = closedPositionTransformB.position;

        float pressSpeed = doorSpeed * 2f;

        // Animar botón presionándose
        while (Vector3.Distance(button.transform.position, buttonPressedPosition) > 0.01f)
        {
            button.transform.position = Vector3.MoveTowards(button.transform.position, buttonPressedPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }

        // Reproducir sonido correspondiente
        if (audioSource != null)
        {
            if (newState && audioOpen != null)
            {
                audioSource.PlayOneShot(audioOpen);
            }
            else if (!newState && audioClose != null)
            {
                audioSource.PlayOneShot(audioClose);
            }
        }

        // Animar puerta moviéndose a posición objetivo
        bool finish = false;
        while (!finish)
        {
            doorObject.transform.position = Vector3.MoveTowards(doorObject.transform.position, doorTargetPosition.position, doorSpeed * Time.deltaTime);
            float distance = Vector3.Distance(doorObject.transform.position, doorTargetPosition.position);

            if (distance < 0.1f)
            {
                SetDoorState(newState);
                finish = true;

                if (newState)
                {
                    yield return new WaitForSeconds(3f); // Espera antes de cerrar automáticamente
                    currentCoroutine = StartCoroutine(ChangeDoorState(false));
                    yield break;
                }
            }
            yield return null;
        }

        // Animar botón volviendo a su posición original (soltarse)
        while (Vector3.Distance(button.transform.position, buttonOriginalPosition) > 0.01f)
        {
            button.transform.position = Vector3.MoveTowards(button.transform.position, buttonOriginalPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Intenta activar la puerta (abrir o cerrar).
    /// Si el botón está bloqueado, reproduce un sonido y no realiza acción.
    /// </summary>
    public void ActivarObjeto()
    {
        if (!botonDesbloqueado)
        {
            if (audioSource != null && audioBloqueado != null)
            {
                audioSource.PlayOneShot(audioBloqueado);
            }
            return;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ChangeDoorState(!doorState));
        doorState = !doorState;
    }

    /// <summary>
    /// Desbloquea el botón para permitir la interacción.
    /// </summary>
    public void DesbloquearBoton()
    {
        botonDesbloqueado = true;
    }
}
