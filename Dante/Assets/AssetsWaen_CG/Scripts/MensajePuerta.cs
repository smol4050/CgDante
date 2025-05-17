using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensajePuerta : MonoBehaviour
{
    public float distancia = 1.5f;
    private LayerMask mask;
    [SerializeField] private Camera cam;

    public bool misionCompletada = false;

    public GameObject panelMensajePuerta;  // Este es el panel que muestra el mensaje personalizado

    GameObject ultimoReconocido = null;
    selectorVisual SelectorVisual;

    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect"); // Asegúrate que tu layer se llama así
        panelMensajePuerta.SetActive(false);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (ultimoReconocido == null)
        {
            panelMensajePuerta.SetActive(false);
        }

        Debug.DrawRay(ray.origin, ray.direction * distancia, Color.green);

        if (Physics.Raycast(ray, out hit, distancia, mask))
        {
            Deselect();
            SelectedObject(hit.transform);

            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractuable objeto = hit.collider.GetComponent<IInteractuable>();
                if (objeto != null)
                {
                    objeto.ActivarObjeto();
                }
            }
        }
        else
        {
            Deselect();
        }
    }

    void SelectedObject(Transform transform)
    {
        SelectorVisual = transform.GetComponent<selectorVisual>();
        if (SelectorVisual != null)
        {
            SelectorVisual.ActivarResaltado();
        }

        ultimoReconocido = transform.gameObject;

        if (transform.CompareTag("Puerta") && !misionCompletada)
        {
            panelMensajePuerta.SetActive(true);
        }
        else
        {
            panelMensajePuerta.SetActive(false);
        }
    }

    void Deselect()
    {
        if (ultimoReconocido)
        {
            SelectorVisual = ultimoReconocido.GetComponent<selectorVisual>();
            if (SelectorVisual != null)
            {
                SelectorVisual.DesactivarResaltado();
            }
        }

        panelMensajePuerta.SetActive(false);  // Siempre ocultar el panel cuando no apunta a nada

        ultimoReconocido = null;
        SelectorVisual = null;
    }
}
