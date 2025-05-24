using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Singleton que controla la recolecci�n de corazones en la escena.
/// Actualiza la interfaz y abre la puerta cuando la misi�n se completa.
/// </summary>
public class RecolectorCorazones : MonoBehaviour
{
    /// <summary>
    /// Instancia �nica del singleton.
    /// </summary>
    public static RecolectorCorazones Instance;

    /// <summary>
    /// N�mero de corazones restantes por recolectar.
    /// </summary>
    public int numDddeCorazones;

    /// <summary>
    /// Texto que muestra la misi�n actual.
    /// </summary>
    public TextMeshProUGUI textoMision;

    /// <summary>
    /// Texto que muestra la cantidad de corazones recolectados/restantes.
    /// </summary>
    public TextMeshProUGUI textoCorazones;

    /// <summary>
    /// Referencia al controlador de la puerta que se abrir� al completar la misi�n.
    /// </summary>
    public PuertaController puerta;

    /// <summary>
    /// Inicializa la instancia singleton o destruye duplicados.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Inicializa la cantidad de corazones y actualiza la interfaz.
    /// </summary>
    private void Start()
    {
        numDddeCorazones = GameObject.FindGameObjectsWithTag("Objetivo").Length;
        textoCorazones.text = numDddeCorazones.ToString();
    }

    /// <summary>
    /// Reduce el contador de corazones y actualiza la interfaz.
    /// Abre la puerta y muestra mensaje cuando se recolectan todos.
    /// </summary>
    public void ContarCorazon()
    {
        numDddeCorazones--;
        textoCorazones.text = numDddeCorazones.ToString();

        if (numDddeCorazones <= 0)
        {
            textoMision.text = "�Completaste la misi�n! \nSal de la estacion y sube las escaleras";
            puerta.AbrirPuerta();
        }
    }
}
