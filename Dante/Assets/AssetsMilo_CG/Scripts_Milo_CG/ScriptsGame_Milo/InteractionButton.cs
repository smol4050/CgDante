using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButton : MonoBehaviour, IInteractuable
{
    public float hundimiento = 0.1f; // Qué tanto se hunde
    public float velocidad = 5f;     // Qué tan rápido baja/sube

    private Vector3 posicionOriginal;
    private Vector3 posicionHundida;
    private bool enMovimiento = false;

    GameController_ParaisoOscuro gameCo;

    void Start()
    {
        posicionOriginal = transform.localPosition;
        posicionHundida = posicionOriginal + new Vector3(0, -hundimiento, 0);

        gameCo = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    public void ActivarObjeto()
    {
        if (!enMovimiento)
        {
            StartCoroutine(PresionarBoton());
        }
    }

    IEnumerator PresionarBoton()
    {
        enMovimiento = true;

        // Baja el botón
        float t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionOriginal, posicionHundida, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }

        transform.localPosition = posicionHundida;

        // Aquí va tu lógica personalizada al presionar
        gameCo.EstadoJugandoTutorial();

        // Espera un momento antes de subir
        yield return new WaitForSeconds(0.2f);

        // Sube el botón
        t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionHundida, posicionOriginal, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }

        transform.localPosition = posicionOriginal;
        enMovimiento = false;
    }
}
