using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinternaPickup : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject linternaEnCamara; // Asigna la linterna en la cámara

    private AudioSource audioSource;
    private bool recogida = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivarObjeto()
    {
        if (recogida) return;
        recogida = true;

        if (linternaEnCamara != null)
            linternaEnCamara.SetActive(true);

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            // Destruye el objeto después de que termine el sonido
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            // Si no hay sonido, lo desactiva de inmediato
            gameObject.SetActive(false);
        }
    }
}
