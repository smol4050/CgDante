using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidosPuerta : MonoBehaviour
{
    public AudioClip openDoor;
    public AudioClip closeDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerDoor"))
        {
            AudioSource.PlayClipAtPoint(closeDoor, transform.position, 1);
            Debug.Log("Puerta cerrada");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerDoor"))
        {
            AudioSource.PlayClipAtPoint(openDoor, transform.position, 1);
            Debug.Log("Puerta abierta");
        }
    }
}
