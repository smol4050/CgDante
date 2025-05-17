using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelIntro : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI mensajeTexto;

    public float tiempoEntreLetras = 0.05f;
    public float duracionPanel = 7f;

    public AudioSource audioSource;
    public AudioClip sonidoLetraLoop;

    void Start()
    {
        StartCoroutine(MostrarMensajeConAnimacion());
    }

    IEnumerator MostrarMensajeConAnimacion()
    {
        panel.SetActive(true);

        // Inicia sonido
        if (audioSource != null && sonidoLetraLoop != null)
        {
            audioSource.clip = sonidoLetraLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        string mensajeCompleto = mensajeTexto.text;
        mensajeTexto.text = "";

        for (int i = 0; i < mensajeCompleto.Length; i++)
        {
            mensajeTexto.text += mensajeCompleto[i];
            yield return new WaitForSeconds(tiempoEntreLetras);
        }

        // Detener sonido justo al terminar de escribir
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        yield return new WaitForSeconds(duracionPanel);
        panel.SetActive(false);
    }
}
