using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un botón interactuable que se hunde al ser presionado y luego se destruye.
/// Implementa la interfaz <see cref="IInteractuable"/>.
/// </summary>
public class InteractionButton : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Distancia que se hunde el botón al ser presionado (en unidades locales).
    /// </summary>
    public float hundimiento = 0.1f;

    /// <summary>
    /// Velocidad a la que el botón se hunde y vuelve a su posición original.
    /// </summary>
    public float velocidad = 5f;

    private Vector3 posicionOriginal;
    private Vector3 posicionHundida;
    private bool enMovimiento = false;

    private GameController_ParaisoOscuro gameCo;

    /// <summary>
    /// Inicializa las posiciones originales y hundidas del botón, y referencia el controlador del juego.
    /// </summary>
    void Start()
    {
        posicionOriginal = transform.localPosition;
        posicionHundida = posicionOriginal + new Vector3(0, -hundimiento, 0);

        gameCo = FindObjectOfType<GameController_ParaisoOscuro>();
    }

    /// <summary>
    /// Método que se llama para activar el botón.
    /// Inicia la corrutina de hundimiento y subida si no está en movimiento.
    /// </summary>
    public void ActivarObjeto()
    {
        if (!enMovimiento)
        {
            StartCoroutine(PresionarBoton());
        }
    }

    /// <summary>
    /// Corrutina que controla la animación de hundir y subir el botón,
    /// ejecuta la lógica personalizada y destruye el botón al finalizar.
    /// </summary>
    /// <returns>IEnumerator para la corrutina.</returns>
    private IEnumerator PresionarBoton()
    {
        enMovimiento = true;

        // Baja el botón interpolando su posición local
        float t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionOriginal, posicionHundida, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }
        transform.localPosition = posicionHundida;

        // Ejecuta la lógica personalizada al presionar el botón
        gameCo.EstadoJugandoTutorial();

        // Espera un breve tiempo antes de subir el botón
        yield return new WaitForSeconds(0.2f);

        // Sube el botón interpolando su posición local a la original
        t = 0f;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(posicionHundida, posicionOriginal, t);
            t += Time.deltaTime * velocidad;
            yield return null;
        }
        transform.localPosition = posicionOriginal;

        enMovimiento = false;

        // Destruye el objeto del botón luego de usarlo
        GameObject.Destroy(gameObject);
    }
}
