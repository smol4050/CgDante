using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que permite al jugador recoger una linterna.
/// Al interactuar, activa la linterna en la c�mara y reproduce un sonido.
/// </summary>
public class LinternaPickup : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia a la linterna que debe activarse en la c�mara del jugador.
    /// </summary>
    [SerializeField] private GameObject linternaEnCamara;

    private AudioSource audioSource;

    /// <summary>
    /// Indica si la linterna ya fue recogida.
    /// </summary>
    private bool recogida = false;

    /// <summary>
    /// Inicializa el componente AudioSource al comenzar la escena.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// M�todo que se llama al interactuar con el objeto.
    /// Activa la linterna en la c�mara y reproduce un sonido, si est� disponible.
    /// </summary>
    public void ActivarObjeto()
    {
        if (recogida) return;
        recogida = true;

        if (linternaEnCamara != null)
            linternaEnCamara.SetActive(true);

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            // Destruye el objeto despu�s de que el sonido termine
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            // Si no hay sonido, desactiva el objeto inmediatamente
            gameObject.SetActive(false);
        }
    }
}
