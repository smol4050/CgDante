using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public PathManager pathManager;

    public Camera fpsCamThisButton;
    public Camera sequenceCamThisButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Asigna dinámicamente las cámaras de este botón
            pathManager.fpsCamera = fpsCamThisButton;
            pathManager.sequenceCamera = sequenceCamThisButton;

            pathManager.StartMemorySequence();
        }
    }
}
