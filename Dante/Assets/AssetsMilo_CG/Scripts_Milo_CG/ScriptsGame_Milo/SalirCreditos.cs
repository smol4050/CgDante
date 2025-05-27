using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalirCreditos : MonoBehaviour
{
    [SerializeField] private string nombreEscenaDestino = "Menu_CG"; // Cambia esto por el nombre real de la escena

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}
