using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class ProgresoNivelGuardado
    {
        public string nombreNivel;
        public int objetosRecolectados;
        public bool nivelCompletado;
    }

    [System.Serializable]
    public class DatosGuardados
    {
        public List<ProgresoNivelGuardado> progresoPorNivel = new List<ProgresoNivelGuardado>();
    }

