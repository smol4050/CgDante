using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un bot�n interactuable que se hunde al ser presionado y luego se destruye.
/// Implementa la interfaz <see cref="IInteractuable"/>.
/// </summary>
public class InteractionButton : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Distancia que se hunde el bot�n al ser presionado (en unidades locales).
    /// </summary>
    public float hundimiento = 0.1f;

    /// <summary>
    /// Velocidad a la que el bot�n se hunde y vuelve a su posici�n original.
    /// </summary>
    public float velocidad = 5f;

    private Vector3 posicionOriginal;
    private Vector3 posicionHundida;
    private bool enMovimiento = false;

    private GameController_ParaisoOscuro gameCo;

    /// <summary>
    /// Inicializa las posiciones originales y hundidas del bot�n, y referencia el controlador del juego.
    /// </summary>
    void Start()
    {
        posicionOriginal = transform.localPosition;
        posicionHundida = posicionOriginal + new Vector3(0, -hundimiento, 0);

        gameCo = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    /// <summary>
    /// M�todo que se llama para activar el bot�n.
    /// Inicia la corrutina de hundimiento y subida si no est� en movimiento.
    /// </summary>
    public void ActivarObjeto()
    {
        if (!enMovimiento)
        {
            StartCoroutine(PresionarBoton());
        }
    }

    /// <summary>
    /// Corrutina que controla la animaci�n de hundir y subir el bot�n,
    /// ejecuta la l�gica personalizada y destruye el bot�n al finalizar.
    /// </summary>
    /// <returns>IEnumerator para la corrutina.</returns>
    private IEnumerator PresionarBoton()
    {
        enMovimiento = true;

        // Baja el bot�n interpolando su posici�n local
        float t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionOriginal, posicionHundida, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }
        transform.localPosition = posicionHundida;

        // Ejecuta la l�gica personalizada al presionar el bot�n
        gameCo.EstadoJugandoTutorial();

        // Espera un breve tiempo antes de subir el bot�n
        yield return new WaitForSeconds(0.2f);

        // Sube el bot�n interpolando su posici�n local a la original
        t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionHundida, posicionOriginal, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }
        transform.localPosition = posicionOriginal;

        enMovimiento = false;

        // Destruye el objeto del bot�n luego de usarlo
        GameObject.Destroy(gameObject);
    }
}
