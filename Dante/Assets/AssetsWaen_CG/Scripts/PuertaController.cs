using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaController : MonoBehaviour
{

    private bool estaAbierta = false;
    private Quaternion rotacionAbierta;
    private Quaternion rotacionCerrada;

    [SerializeField] private float velocidadApertura = 2f;

    void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0)); // Gira 90 grados en Y

    }

    // Update is called once per frame
    void Update()
    {
        if (estaAbierta)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionAbierta, Time.deltaTime * velocidadApertura);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionCerrada, Time.deltaTime * velocidadApertura);
        }

    }

    public void AbrirPuerta()
    {
        estaAbierta = true;
    }
}
