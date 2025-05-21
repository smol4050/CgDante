using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image damagePanel;
    public float fadeSpeed = 2f; // Velocidad a la que se desvanece
    public float damageOpacityIncrease = 0.2f; // Cuánto sube la opacidad por golpe
    private float tiempoUltimoGolpe;
    public int maxHits = 5;

    private float currentAlpha = 0f;
    private int hitsReceived = 0;
    private bool isDead = false;

    private void Start()
    {
        SetPanelAlpha(0f); // Asegura que comience invisible
    }

    private void Update()
    {
        if (!isDead && currentAlpha > 0f)
        {
            // Solo empieza a desvanecer si pasó cierto tiempo sin recibir daño
            if (Time.time - tiempoUltimoGolpe >= 1.5f)
            {
                currentAlpha -= Time.deltaTime * fadeSpeed;
                currentAlpha = Mathf.Clamp01(currentAlpha);
                SetPanelAlpha(currentAlpha);
            }
        }
    }

    public void RecibirGolpe()
    {
        if (isDead) return;

        hitsReceived++;
        tiempoUltimoGolpe = Time.time;

        currentAlpha += damageOpacityIncrease;
        currentAlpha = Mathf.Clamp01(currentAlpha);
        SetPanelAlpha(currentAlpha);

        if (hitsReceived >= maxHits)
        {
            Morir();
        }
    }

    private void SetPanelAlpha(float alpha)
    {
        if (damagePanel != null)
        {
            Color color = damagePanel.color;
            color.a = alpha;
            damagePanel.color = color;
        }
    }

    private void Morir()
    {
        isDead = true;
        Debug.Log("Jugador ha muerto.");
        // Aquí puedes agregar animación, desactivar control, etc.
    }
}
