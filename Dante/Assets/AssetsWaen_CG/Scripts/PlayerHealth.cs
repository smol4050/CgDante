using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controla la salud del jugador mediante un sistema de golpes acumulados.
/// Muestra un panel rojo que aumenta su opacidad según el daño recibido.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// Imagen del panel rojo que representa el daño visualmente.
    /// </summary>
    public Image damagePanel;

    /// <summary>
    /// Velocidad a la que se desvanece el panel de daño cuando no se recibe daño.
    /// </summary>
    public float fadeSpeed = 2f;

    /// <summary>
    /// Incremento de opacidad por cada golpe recibido.
    /// (No se usa directamente; se calcula en proporción a maxHits).
    /// </summary>
    public float damageOpacityIncrease = 0.2f;

    /// <summary>
    /// Número máximo de golpes antes de morir.
    /// </summary>
    public int maxHits = 5;

    /// <summary>
    /// Referencia al script que controla el menú de pausa y game over.
    /// </summary>
    public PausarReanudar pausarReanudar;

    private float currentAlpha = 0f;
    private int hitsReceived = 0;
    private bool isDead = false;
    private float tiempoUltimoGolpe;

    /// <summary>
    /// Inicializa el panel de daño en estado invisible.
    /// </summary>
    private void Start()
    {
        SetPanelAlpha(0f);
    }

    /// <summary>
    /// Reduce la opacidad del panel si no se recibe daño después de un tiempo.
    /// </summary>
    private void Update()
    {
        if (!isDead && currentAlpha > 0f)
        {
            // Solo desvanece si pasó más de 1.5s desde el último golpe
            if (Time.time - tiempoUltimoGolpe >= 1.5f)
            {
                currentAlpha -= Time.deltaTime * fadeSpeed;
                currentAlpha = Mathf.Clamp01(currentAlpha);
                SetPanelAlpha(currentAlpha);
            }
        }
    }

    /// <summary>
    /// Llamado cuando el jugador recibe un golpe. Aumenta el daño visual.
    /// Si se supera el número de golpes máximos, se activa el game over.
    /// </summary>
    public void RecibirGolpe()
    {
        if (isDead) return;

        hitsReceived++;
        tiempoUltimoGolpe = Time.time;

        // Ajustar opacidad proporcional al número de golpes
        currentAlpha = (float)hitsReceived / maxHits;
        currentAlpha = Mathf.Clamp01(currentAlpha);
        SetPanelAlpha(currentAlpha);

        if (hitsReceived >= maxHits)
        {
            Morir();
        }
    }

    /// <summary>
    /// Cambia la opacidad del panel de daño.
    /// </summary>
    /// <param name="alpha">Nuevo valor de alfa (0-1).</param>
    private void SetPanelAlpha(float alpha)
    {
        if (damagePanel != null)
        {
            Color color = damagePanel.color;
            color.a = alpha;
            damagePanel.color = color;
        }
    }

    /// <summary>
    /// Lógica de muerte del jugador. Notifica al sistema de pausa para mostrar el panel de Game Over.
    /// </summary>
    private void Morir()
    {
        isDead = true;
        Debug.Log("Jugador ha muerto.");

        if (pausarReanudar != null)
        {
            pausarReanudar.MostrarGameOver();
        }
        else
        {
            Debug.LogWarning("No se encontró el script PausarReanudar en la escena.");
        }
    }
}
