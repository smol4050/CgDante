using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioEscena_Video : MonoBehaviour
{
    public void CambioEscena(string nombreEscena)
    {
        // Verifica si el nombre de la escena no est� vac�o
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            // Carga la escena especificada
            UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena est� vac�o o es nulo.");
        }
    }
}
