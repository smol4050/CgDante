using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausarReanudar : MonoBehaviour
{
    public GameObject menuPausa;
    private AudioListener audioListener;

    private void Start()
    {
        // Asegúrate de que el menú de pausa esté oculto al inicio
        menuPausa.SetActive(false);
        audioListener = FindObjectOfType<AudioListener>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0f)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;

        Time.timeScale = isPaused ? 1f : 0f;
        menuPausa.SetActive(!isPaused);

        if (audioListener != null)
        {
            audioListener.enabled = !isPaused;
        }

        if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerController.Instance.canMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerController.Instance.canMove = true;
        }
    }

    public void ReanudarJuego() // (opcional)
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);

        if (audioListener != null)
        {
            audioListener.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerController.Instance.canMove = true;
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f; // Asegúrate de reactivar el tiempo al cambiar de escena
        if (audioListener != null)
            audioListener.enabled = true;

        SceneManager.LoadScene("Menu_CG"); // <-- Cambia esto por el nombre real de tu escena de menú
    }
}
