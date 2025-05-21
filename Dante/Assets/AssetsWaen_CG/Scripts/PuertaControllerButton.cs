using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject button;
    public GameObject doorObject;
    public GameObject wallObject;
    public GameObject columnObject;

    public Transform openPositionTransform;
    public Transform closedPositionTransform;

    public Transform openPositionTransformB;
    public Transform closedPositionTransformB;

    public bool botonDesbloqueado = false;

    public bool doorState = false;
    public bool buttonState = false;

    public float doorSpeed = 2f;

    Coroutine currentCorutine;

    public AudioSource audioSource;        // El componente de audio
    public AudioClip audioOpen;             // Sonido para abrir puerta
    public AudioClip audioClose;
    public AudioClip audioBloqueado;// Sonido para cerrar puerta

    // Start is called before the first frame update
    void Start()
    {
        setDoorState(doorState);
        setDoorState(buttonState);

    }


    void setDoorState(bool s)
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


    public void openDoor()
    {
        Debug.Log("Open Door");
        if(currentCorutine != null)
        {
            StopCoroutine(currentCorutine);
        }
        currentCorutine = StartCoroutine(ChanceDoorState(true));

    }

    public void closeDoor()
    {
      
        Debug.Log("Close Door");
        if (currentCorutine != null)
        {
            StopCoroutine(currentCorutine);
        }
        currentCorutine = StartCoroutine(ChanceDoorState(false));
    }

    IEnumerator ChanceDoorState(bool newState)
    {

        // Determinar posición objetivo para la puerta
        Transform doorTargetPosition = newState ? openPositionTransform : closedPositionTransform;

        // Determinar posición objetivo para el botón (simulando que se presiona y luego vuelve)
        Vector3 buttonPressedPosition = openPositionTransformB.position;
        Vector3 buttonOriginalPosition = closedPositionTransformB.position;

        // Mover botón hacia abajo (presionado)
        float pressSpeed = doorSpeed * 2f;
        while (Vector3.Distance(button.transform.position, buttonPressedPosition) > 0.01f)
        {
            button.transform.position = Vector3.MoveTowards(button.transform.position, buttonPressedPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }

        bool finish = false;

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

        Transform targetPosition;

        if (newState)
        {
            targetPosition = openPositionTransform;
        }
        else
        {
            targetPosition = closedPositionTransform;
        }

        while (!finish)
        {

            doorObject.transform.position = Vector3.MoveTowards(doorObject.transform.position, targetPosition.position, doorSpeed * Time.deltaTime);

            float distance = Vector3.Distance(doorObject.transform.position, targetPosition.position);

            if (distance < 0.1f)
            {
                setDoorState(newState);
                finish = true;

                if (newState) // Si abrimos la puerta
                {
                    yield return new WaitForSeconds(3f); // Esperamos 5 segundos
                    currentCorutine = StartCoroutine(ChanceDoorState(false)); // Cerramos
                    yield break;
                }
            }


            yield return null;

        }
        // Volver botón a su posición original (como si se "soltara")
        while (Vector3.Distance(button.transform.position, buttonOriginalPosition) > 0.01f)
        {
            button.transform.position = Vector3.MoveTowards(button.transform.position, buttonOriginalPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }

    }

    public void ActivarObjeto()
    {
        // Alterna el estado (abre o cierra la puerta)
        if (!botonDesbloqueado)
        {
            //  Reproducir sonido de puerta bloqueada
            if (audioSource != null && audioBloqueado != null)
            {
                audioSource.PlayOneShot(audioBloqueado);
            }
            return; // No permite abrir/cerrar la puerta si está bloqueada
        }
            if (currentCorutine != null)
            {
                StopCoroutine(currentCorutine);
            }

            currentCorutine = StartCoroutine(ChanceDoorState(!doorState));
            doorState = !doorState;
        
    }

    public void DesbloquearBoton()
    {
        botonDesbloqueado = true;
    }

}
