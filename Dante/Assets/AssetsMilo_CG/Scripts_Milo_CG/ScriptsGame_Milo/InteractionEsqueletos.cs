using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEsqueletos : MonoBehaviour, IInteractuable

{
    EnemyController_Paraiso enemigoControllerPuertas;
    MeshRenderer meshRenderer;
    Collider SuCollider;
    AudioSource audioSource;

    private Coroutine rutinaAparicion;

    void Start()
    {
        enemigoControllerPuertas = FindObjectOfType<EnemyController_Paraiso>();
        meshRenderer = GetComponent<MeshRenderer>();
        SuCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        // Oculta solo visualmente el esqueleto
        DesactivarVisual();
    }

    void Update()
    {
        if (enemigoControllerPuertas.JugadorPerdio)
        {
            if (rutinaAparicion != null)
            {
                StopCoroutine(rutinaAparicion);
                rutinaAparicion = null;
                DesactivarVisual();
            }
        }
    }

    public void IniciarEnemigoEsqueletos()
    {
        rutinaAparicion = StartCoroutine(AparicionAleatoria());
    }

    IEnumerator AparicionAleatoria()
    {
        while (true)
        {
            float tiempoEspera = Random.Range(10f, 20f);
            yield return new WaitForSeconds(tiempoEspera);

            ActivarEsqueleto();
        }
    }

    public void ActivarEsqueleto()
    {
        meshRenderer.enabled = true;
        SuCollider.enabled = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void DesactivarVisual()
    {
        meshRenderer.enabled = false;
        SuCollider.enabled = false;
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void DesactivarEsqueletos()
    {
        if (rutinaAparicion != null)
        {
            StopCoroutine(rutinaAparicion);
            rutinaAparicion = null;
        }

        DesactivarVisual(); // en lugar de apagar todo el GameObject
    }

    public void ActivarObjeto()
    {
        DesactivarVisual(); // esto se llama al presionar 'E' según tu lógica
    }
}
