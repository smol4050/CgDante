using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador para una puerta que se activa mediante un bot�n f�sico en el escenario.
/// Incluye movimientos suaves de puerta y bot�n, control de acceso y reproducci�n de sonidos.
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias de Objetos")]
    /// <summary>
    /// Referencia al objeto que act�a como bot�n f�sico.
    /// </summary>
    public GameObject button;

    /// <summary>
    /// Referencia al objeto que representa la puerta.
    /// </summary>
    public GameObject doorObject;

    /// <summary>
    /// Referencia al objeto que representa una pared (no usado en el c�digo).
    /// </summary>
    public GameObject wallObject;

    /// <summary>
    /// Referencia al objeto que representa una columna (no usado en el c�digo).
    /// </summary>
    public GameObject columnObject;

    [Header("Posiciones de Movimiento")]
    /// <summary>
    /// Transform que define la posici�n abierta de la puerta.
    /// </summary>
    public Transform openPositionTransform;

    /// <summary>
    /// Transform que define la posici�n cerrada de la puerta.
    /// </summary>
    public Transform closedPositionTransform;

    /// <summary>
    /// Transform que define la posici�n abierta del bot�n.
    /// </summary>
    public Transform openPositionTransformB;

    /// <summary>
    /// Transform que define la posici�n cerrada del bot�n.
    /// </summary>
    public Transform closedPositionTransformB;

    [Header("Estados")]
    /// <summary>
    /// Indica si el bot�n est� desbloqueado y puede usarse.
    /// </summary>
    public bool botonDesbloqueado = false;

    /// <summary>
    /// Estado actual de la puerta: true = abierta, false = cerrada.
    /// </summary>
    public bool doorState = false;

    /// <summary>
    /// Estado actual del bot�n: true = presionado, false = sin presionar.
    /// </summary>
    public bool buttonState = false;

    [Header("Velocidades")]
    /// <summary>
    /// Velocidad de movimiento de la puerta.
    /// </summary>
    public float doorSpeed = 2f;

    /// <summary>
    /// Referencia a la coroutine actualmente en ejecuci�n.
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
    /// Clip de audio que se reproduce si la puerta est� bloqueada.
    /// </summary>
    public AudioClip audioBloqueado;

    /// <summary>
    /// Inicializa el estado de la puerta y el bot�n al iniciar la escena.
    /// </summary>
    private void Start()
    {
        SetDoorState(doorState);
        SetDoorState(buttonState);
    }

    /// <summary>
    /// Establece la posici�n de la puerta y del bot�n seg�n el estado indicado.
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
    /// Coroutine que anima el cambio de estado de la puerta y la animaci�n del bot�n.
    /// </summary>
    /// <param name="newState">Nuevo estado de la puerta: true para abrir, false para cerrar.</param>
    /// <returns>IEnumerator para coroutine.</returns>
    private IEnumerator ChangeDoorState(bool newState)
    {
        Transform doorTargetPosition = newState ? openPositionTransform : closedPositionTransform;
        Vector3 buttonPressedPosition = openPositionTransformB.position;
        Vector3 buttonOriginalPosition = closedPositionTransformB.position;

        float pressSpeed = doorSpeed * 2f;

        // Animar bot�n presion�ndose
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

        // Animar puerta movi�ndose a posici�n objetivo
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
                    yield return new WaitForSeconds(3f); // Espera antes de cerrar autom�ticamente
                    currentCoroutine = StartCoroutine(ChangeDoorState(false));
                    yield break;
                }
            }
            yield return null;
        }

        // Animar bot�n volviendo a su posici�n original (soltarse)
        while (Vector3.Distance(button.transform.position, buttonOriginalPosition) > 0.01f)
        {
            button.transform.position = Vector3.MoveTowards(button.transform.position, buttonOriginalPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Intenta activar la puerta (abrir o cerrar).
    /// Si el bot�n est� bloqueado, reproduce un sonido y no realiza acci�n.
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
    /// Desbloquea el bot�n para permitir la interacci�n.
    /// </summary>
    public void DesbloquearBoton()
    {
        botonDesbloqueado = true;
    }
}
