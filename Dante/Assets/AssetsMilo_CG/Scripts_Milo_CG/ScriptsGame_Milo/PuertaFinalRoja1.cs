using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaFinalRoja1 : MonoBehaviour
{
    public float doorOpenAngle = 90f;
    public float smooth = 2f;

    private bool doorOpen = false;

    public void AbrirPuerta()
    {
        if (!doorOpen)
        {
            doorOpen = true;
            Debug.Log("¡Puerta final abriéndose!");
        }
    }

    void Update()
    {
        if (doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }
}
