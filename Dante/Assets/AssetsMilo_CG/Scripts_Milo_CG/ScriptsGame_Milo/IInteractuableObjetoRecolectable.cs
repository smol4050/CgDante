using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractuableObjetoRecolectable : MonoBehaviour, IInteractuable
{
    public void ActivarObjeto()
    {
        GameManager.Instance.SumarObjeto(); // Ya hace todo el conteo y logs
        GameObject.Destroy(gameObject);
    }

}
