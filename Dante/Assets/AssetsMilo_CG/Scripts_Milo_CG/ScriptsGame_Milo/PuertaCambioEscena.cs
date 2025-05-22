using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaCambioEscena : MonoBehaviour, IInteractuable
{
    public string nombreEscena; // Nombre de la escena a la que quieres cambiar
    public void ActivarObjeto()
    {
        SceneManager.LoadScene(nombreEscena); // Asegúrate que el nombre esté bien escrito
    }
}
