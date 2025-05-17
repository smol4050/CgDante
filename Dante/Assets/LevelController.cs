using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    private void Awake()
    {
        // Implementación de singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Llamar cuando el jugador completa correctamente la secuencia.
    /// </summary>
    public void OnLevelComplete()
    {
        Debug.Log("Nivel completado: avanzando o cargando siguiente escena.");
        // Aquí podrías cargar otra escena
        // Ejemplo:
        // SceneManager.LoadScene("ParadiseScene");
    }

    /// <summary>
    /// Llamar cuando el jugador falla la secuencia.
    /// </summary>
    public void OnLevelFailed()
    {
        Debug.Log("Fallaste el nivel: reiniciando escena actual.");
        // Reinicia la misma escena:
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
