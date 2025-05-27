using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que permite al jugador recoger una linterna.
/// Al interactuar, activa la linterna en la c�mara y reproduce un sonido.
/// Implementa la interfaz <see cref="IInteractuable"/> para permitir la interacci�n.
/// </summary>
public class LinternaPickup : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Referencia al controlador de la linterna que se activar� al recogerla.
    /// Debe asignarse desde el Inspector.
    /// </summary>
    [SerializeField] private LinternaControlador controlador;

    /// <summary>
    /// Componente de audio que se usar� para reproducir el sonido de recogida.
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Bandera para evitar que la linterna sea recogida m�ltiples veces.
    /// </summary>
    private bool recogida = false;

    /// <summary>
    /// Inicializa referencias necesarias.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// M�todo que se ejecuta cuando el jugador interact�a con el objeto.
    /// Activa la linterna, reproduce un sonido y desactiva o destruye el objeto.
    /// </summary>
    public void ActivarObjeto()
    {
        if (recogida) return;
        recogida = true;

        controlador.ActivarLinterna(); // Notifica que la linterna fue recogida

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length); // Espera a que termine el sonido antes de destruir
        }
        else
        {
            gameObject.SetActive(false); // Si no hay sonido, simplemente desactiva el objeto
        }
    }
}
