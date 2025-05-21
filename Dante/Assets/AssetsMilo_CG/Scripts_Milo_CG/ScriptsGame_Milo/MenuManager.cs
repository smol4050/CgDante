using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string nombreEscenaDestino;
    public AudioClip sonidoBoton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CambiarEscenaInicio(nombreEscenaDestino);
        }
    }

    public void CargarEscena(string EscenaCambio)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CambiarEscena(EscenaCambio);
        }
    }

    public void CambiarEscenaInicio(string nombreEscenaDestino)
    {
        if (!string.IsNullOrEmpty(nombreEscenaDestino))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena a cargar.");
        }
    }

    public void ButtonSonido()
    {
        AudioSource.PlayClipAtPoint(sonidoBoton, Camera.main.transform.position, 50f);
    }
}
