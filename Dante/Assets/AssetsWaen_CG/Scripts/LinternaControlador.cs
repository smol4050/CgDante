using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinternaControlador : MonoBehaviour
{
    public GameObject linternaEnCamara; // Asigna la linterna de la cámara
    public AudioClip sonidoEncender;    // Asigna el sonido en el Inspector
    public AudioClip sonidoApagar;      // (Opcional) otro sonido al apagar

    private AudioSource audioSource;
    private bool recogida = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!recogida) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool nuevaActivo = !linternaEnCamara.activeSelf;
            linternaEnCamara.SetActive(nuevaActivo);

            // Reproduce el sonido correspondiente
            if (audioSource != null)
            {
                audioSource.PlayOneShot(nuevaActivo ? sonidoEncender : sonidoApagar ?? sonidoEncender);
            }
        }
    }

    public void ActivarLinterna()
    {
        recogida = true;
        linternaEnCamara.SetActive(true);
    }
}
