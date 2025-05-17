using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoClave : MonoBehaviour, IInteractuable
{
    private bool recolectado = false;
    GameController_ParaisoOscuro Gm;

    void Start()
    {

        Gm = FindObjectOfType<GameController_ParaisoOscuro>();
        if (Gm == null)
        {
            Debug.LogWarning("No se encontró GameController en la escena.");
        }
    }
    public void ActivarObjeto()
    {
        if (recolectado) return;

        recolectado = true;
        Gm.ObjetoRecolectado();

        // Feedback visual/audio
        Debug.Log("Objeto clave recolectado: " + gameObject.name);

        // Desactivar o destruir objeto
        Destroy(gameObject);
    }
}
