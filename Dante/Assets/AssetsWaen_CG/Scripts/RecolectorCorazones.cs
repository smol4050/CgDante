using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecolectorCorazones : MonoBehaviour
{

    public static RecolectorCorazones Instance;
    

    public int numDddeCorazones;
    public TextMeshProUGUI textoMision;
    public TextMeshProUGUI textoCorazones;

    public PuertaController puerta;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        numDddeCorazones = GameObject.FindGameObjectsWithTag("Objetivo").Length;
        textoCorazones.text =numDddeCorazones.ToString();

    }

    public void ContarCorazon()
    {
        numDddeCorazones--;
        textoCorazones.text = numDddeCorazones.ToString();

        if (numDddeCorazones <= 0)
        {
            textoMision.text = "¡Completaste la misión! \n" +
                "Sal de la estacion y sube las escaleras";
            puerta.AbrirPuerta();
            

        }
    }


}
