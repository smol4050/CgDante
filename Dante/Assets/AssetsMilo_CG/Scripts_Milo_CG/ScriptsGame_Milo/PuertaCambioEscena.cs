using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaCambioEscena : MonoBehaviour, IInteractuable
{
    public void ActivarObjeto()
    {
        SceneManager.LoadScene("Smol_CG"); // Asegúrate que el nombre esté bien escrito
    }
}
