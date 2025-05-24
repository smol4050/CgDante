using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la interacci�n con puertas que requieren completar una misi�n.
/// Muestra un panel si el jugador se acerca a una puerta y a�n no ha completado la misi�n.
/// </summary>
public class MensajePuerta : MonoBehaviour
{
    /// <summary>
    /// Distancia m�xima del rayo para detectar objetos interactuables.
    /// </summary>
    public float distancia = 4f;

    private LayerMask mask;

    /// <summary>
    /// C�mara principal desde donde se lanza el raycast.
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// Indica si la misi�n necesaria para abrir la puerta ha sido completada.
    /// </summary>
    public bool misionCompletada = false;

    /// <summary>
    /// Panel UI que muestra el mensaje cuando se apunta a una puerta bloqueada.
    /// </summary>
    public GameObject panelMensajePuerta;

    private GameObject ultimoReconocido = null;
    private selectorVisual SelectorVisual;

    /// <summary>
    /// Inicializa el layer mask y oculta el panel de mensaje al comenzar.
    /// </summary>
    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect"); // Verifica que tu layer se llame exactamente as�
        panelMensajePuerta.SetActive(false);
    }

    /// <summary>
    /// Actualiza cada frame: lanza raycast desde el centro de la pantalla y detecta objetos interactuables.
    /// </summary>
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

    /// <summary>
    /// Marca visualmente un objeto como seleccionado y muestra el panel si es una puerta bloqueada.
    /// </summary>
    /// <param name="transform">Transform del objeto detectado.</param>
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

    /// <summary>
    /// Desmarca cualquier objeto previamente seleccionado y oculta el panel de mensaje.
    /// </summary>
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

        panelMensajePuerta.SetActive(false);

        ultimoReconocido = null;
        SelectorVisual = null;
    }
}
