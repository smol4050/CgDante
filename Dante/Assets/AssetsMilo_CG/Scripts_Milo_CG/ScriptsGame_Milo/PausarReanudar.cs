using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausarReanudar : MonoBehaviour
{
    public GameObject menuPausa; // Este también será el panel de Game Over (o puedes usar otro)
    public GameObject GameOver;
    private AudioListener audioListener;

    private void Start()
    {
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

    public void MostrarGameOver()
    {
        Time.timeScale = 0f;
        GameOver.SetActive(true); // O un panel de game over específico

        if (audioListener != null)
            audioListener.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.Instance.canMove = false;
    }

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

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        if (audioListener != null)
            audioListener.enabled = true;

        SceneManager.LoadScene("Menu_CG");
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        if (audioListener != null)
            audioListener.enabled = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
