using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausarReanudar : MonoBehaviour
{
    public GameObject menuPausa;

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;

        Time.timeScale = isPaused ? 1f : 0f;
        menuPausa.SetActive(!isPaused);
    }

    public void ReanudarJuego() // (opcional)
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
    }
}
