using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioEscena_Video : MonoBehaviour
{
    public void CambioEscena(string nombreEscena)
    {
        // Verifica si el nombre de la escena no está vacío
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            // Carga la escena especificada
            UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena está vacío o es nulo.");
        }
    }
}
