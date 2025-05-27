using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que permite al jugador recoger una linterna.
/// Al interactuar, activa la linterna en la cámara y reproduce un sonido.
/// </summary>
public class LinternaPickup : MonoBehaviour, IInteractuable
{
    [SerializeField] private LinternaControlador controlador; // Asigna este desde el Inspector
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

        controlador.ActivarLinterna(); // Notifica que ya la recogió

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
