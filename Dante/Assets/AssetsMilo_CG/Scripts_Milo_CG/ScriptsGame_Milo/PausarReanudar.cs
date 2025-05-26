using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la pausa, reanudación del juego, y muestra la pantalla de Game Over.
/// Gestiona la activación/desactivación de menús y controla el estado del cursor y audio.
/// </summary>
public class PausarReanudar : MonoBehaviour
{
    /// <summary>
    /// Objeto del menú de pausa que se muestra o esconde.
    /// </summary>
    public GameObject menuPausa;

    /// <summary>
    /// Objeto que representa la pantalla de Game Over.
    /// </summary>
    public GameObject GameOver;

    /// <summary>
    /// Componente AudioListener para habilitar o deshabilitar el audio.
    /// </summary>
    public AudioListener audioListener;

    /// <summary>
    /// Referencia singleton al GameManager.
    /// </summary>
    GameManager gm;

    /// <summary>
    /// Inicializa el menú de pausa y Game Over como ocultos y ajusta el volumen del audio.
    /// </summary>
    private void Start()
    {
        menuPausa.SetActive(false);
        GameOver.SetActive(false);
        gm = GameManager.Instance;

        AudioListener.volume = 1f; // Asegura que el volumen esté al máximo al iniciar el juego
    }

    /// <summary>
    /// Detecta la tecla Escape para alternar la pausa del juego si no está pausado.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0f)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Alterna entre pausar y reanudar el juego, controla menús, audio, cursor y movimiento del jugador.
    /// </summary>
    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;

        Time.timeScale = isPaused ? 1f : 0f;
        menuPausa.SetActive(!isPaused);

        if (audioListener != null)
            audioListener.enabled = !isPaused;

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

    /// <summary>
    /// Muestra la pantalla de Game Over, pausa el juego, desactiva el audio y permite mover el cursor.
    /// </summary>
    public void MostrarGameOver()
    {
        Time.timeScale = 0f;
        GameOver.SetActive(true);

        if (audioListener != null)
            AudioListener.volume = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.canMove = false;
    }

    /// <summary>
    /// Reanuda el juego desde el estado de pausa, restablece el audio y oculta el menú de pausa.
    /// </summary>
    public void ReanudarJuego()
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);

        if (audioListener != null)
            audioListener.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.Instance.canMove = true;
    }

    /// <summary>
    /// Vuelve al menú principal, reiniciando el estado del nivel actual y restaurando el audio.
    /// </summary>
    public void IrAlMenu()
    {
        Time.timeScale = 1f;

        if (audioListener != null)
            audioListener.enabled = true;

        menuPausa.SetActive(false);

        gm.ReiniciarObjetosDelNivelActual();

        SceneManager.LoadScene("Menu_CG");
    }

    /// <summary>
    /// Reinicia el nivel actual, restablece objetos recolectados y vuelve a cargar la escena.
    /// </summary>
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;

        if (audioListener != null)
            audioListener.enabled = true;

        gm.ReiniciarObjetosDelNivelActual(); // Restablece el contador de objetos recolectados

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
