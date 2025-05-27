using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiarEscenaAlEntrar : MonoBehaviour
{
    // Nombre o índice de la escena a cargar
    public string nombreEscena;

    private void OnTriggerEnter(Collider other)
    {
        
        // Puedes añadir una condición para que solo el jugador active el cambio
        if (other.CompareTag("Player"))
        { 
            GameManager.Instance.CompletarNivel();
            SceneManager.LoadScene(nombreEscena);
        }
        
    }

    
    
        
    
}
