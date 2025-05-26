using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la selecci�n visual e interacci�n con objetos que implementan la interfaz <see cref="IInteractuable"/>.
/// Realiza un raycast desde el centro de la c�mara para detectar objetos interactuables dentro de una distancia determinada.
/// </summary>
public class SeletionInteractions : MonoBehaviour
{
    /// <summary>
    /// Distancia m�xima para detectar objetos interactuables.
    /// </summary>
    public float distancia = 1.5f;

    /// <summary>
    /// Capa usada para filtrar objetos detectados por el raycast.
    /// </summary>
    private LayerMask mask;

    /// <summary>
    /// C�mara desde donde se lanza el raycast.
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// Objeto UI que muestra el texto "Presiona E" cuando hay un objeto interactuable seleccionado.
    /// </summary>
    public GameObject textPressE;

    /// <summary>
    /// �ltimo objeto reconocido (seleccionado) por el raycast.
    /// </summary>
    GameObject ultimoReconocido = null;

    /// <summary>
    /// Componente selectorVisual del objeto actualmente resaltado.
    /// </summary>
    selectorVisual SelectorVisual;

    /// <summary>
    /// Inicializa la m�scara de capa y oculta el texto de interacci�n al iniciar.
    /// </summary>
    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect"); // Aseg�rate de que los objetos tengan este layer
        textPressE.SetActive(false);
    }

    /// <summary>
    /// Realiza un raycast desde el centro de la pantalla para detectar y seleccionar objetos interactuables,
    /// y maneja la interacci�n al presionar la tecla E.
    /// </summary>
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (ultimoReconocido == null)
        {
            textPressE.SetActive(false);
        }

        // Debug visual del raycast (l�nea verde desde la c�mara hacia adelante)
        Debug.DrawRay(ray.origin, ray.direction * distancia, Color.green);

        if (Physics.Raycast(ray, out hit, distancia, mask))
        {
            Deselect();
            SelectedObject(hit.transform);

            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractuable objeto = hit.collider.GetComponentInParent<IInteractuable>();
                if (objeto != null)
                {
                    objeto.ActivarObjeto();
                    Debug.Log("Interacting with: " + objeto);
                }
            }
        }
        else
        {
            Deselect();
        }
    }

    /// <summary>
    /// Resalta visualmente el objeto seleccionado y muestra el texto de interacci�n.
    /// </summary>
    /// <param name="transform">Transform del objeto seleccionado.</param>
    void SelectedObject(Transform transform)
    {
        SelectorVisual = transform.GetComponent<selectorVisual>();
        if (SelectorVisual != null)
        {
            SelectorVisual.ActivarResaltado();
        }

        ultimoReconocido = transform.gameObject;
        textPressE.SetActive(true);
    }

    /// <summary>
    /// Desactiva el resaltado del �ltimo objeto reconocido y oculta el texto de interacci�n.
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

            textPressE.SetActive(false);
        }

        ultimoReconocido = null;
        SelectorVisual = null;
    }
}

