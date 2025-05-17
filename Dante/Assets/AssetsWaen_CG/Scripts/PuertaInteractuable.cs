using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaInteractuable : MonoBehaviour, IInteractuable
{

    private bool estaAbierta = false;
    private bool enAnimacion = false;

    public float anguloApertura = 90f;
    public float duracionApertura = 2f;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    public AudioClip audioAbrir;
    public AudioClip audioCerrar;
    private AudioSource audioSource;

    private void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivarObjeto()
    {

        if (enAnimacion) return;

        if (audioSource != null)
        {
            audioSource.clip = estaAbierta ? audioCerrar : audioAbrir;
            audioSource.Play();
        }

        if (estaAbierta)
        {
            StartCoroutine(RotarPuerta(rotacionAbierta, rotacionCerrada));
        }
        else
        {
            StartCoroutine(RotarPuerta(rotacionCerrada, rotacionAbierta));
        }

        estaAbierta = !estaAbierta;
    }

    private System.Collections.IEnumerator RotarPuerta(Quaternion desde, Quaternion hasta)
    {
        enAnimacion = true;

        float tiempo = 0f;
        while (tiempo < duracionApertura)
        {
            transform.rotation = Quaternion.Slerp(desde, hasta, tiempo / duracionApertura);
            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.rotation = hasta;
        enAnimacion = false;
    }

}
