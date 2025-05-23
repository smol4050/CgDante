using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgresoNivel
    {
        public string nombreNivel;
        public int objetosRecolectados;
        public bool nivelCompletado;

        public ProgresoNivel(string nombre)
        {
            nombreNivel = nombre;
            objetosRecolectados = 0;
            nivelCompletado = false;
        }
    }

