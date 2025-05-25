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
            // Asigna din�micamente las c�maras de este bot�n
            pathManager.fpsCamera = fpsCamThisButton;
            pathManager.sequenceCamera = sequenceCamThisButton;

            pathManager.StartMemorySequence();
        }
    }
}
