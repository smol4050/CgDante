using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public PathManager pathManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            pathManager.StartMemorySequence();
    }
}
