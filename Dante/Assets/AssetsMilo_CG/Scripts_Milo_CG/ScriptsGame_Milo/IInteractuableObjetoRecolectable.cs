using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractuableObjetoRecolectable : MonoBehaviour, IInteractuable
{
    GameController_ParaisoOscuro gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController_ParaisoOscuro>();
        if (gameController == null)
        {
            Debug.LogError("No se encontró el GameController_ParaisoOscuro en la escena.");
        }
    }

    public void ActivarObjeto()
    {
        gameController.ObjetoRecolectado();
        GameObject.Destroy(gameObject);

    }

}
