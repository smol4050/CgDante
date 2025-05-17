using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int objetosRecolectados = 0;
    public int totalObjetos = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarObjeto()
    {
        objetosRecolectados++;

        Debug.Log($"Objetos recolectados: {objetosRecolectados}/{totalObjetos}");

        if (objetosRecolectados >= totalObjetos)
        {
            Debug.Log("¡Todos los objetos recolectados!");
            // Activar puerta final o evento
        }
    }


    public void CambiarEscena(string nombreEscena)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
    }
}
