using System.Collections;
using UnityEngine;

/// <summary>
/// Script para un objeto recolectable que puede ser examinado antes de ser recogido.
/// Implementa la interfaz <see cref="IInteractuable"/> para integrarse con sistemas de interacci�n.
/// </summary>
public class Recolectable : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Indica si el objeto ya ha sido recogido.
    /// </summary>
    private bool _isCollected = false;

    /// <summary>
    /// Indica si el objeto est� en proceso de ser examinado.
    /// </summary>
    private bool _beingExamined = false;

    /// <summary>
    /// Referencia al transform de la c�mara principal.
    /// </summary>
    private Transform _mainCamera;

    /// <summary>
    /// Posici�n original del objeto antes de ser examinado.
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// Rotaci�n original del objeto antes de ser examinado.
    /// </summary>
    private Quaternion _originalRotation;

    /// <summary>
    /// Duraci�n en segundos del examen antes de recoger el objeto.
    /// </summary>
    [Header("Configuraci�n de examen")]
    [Tooltip("Duraci�n en segundos del examen antes de recoger el objeto.")]
    [SerializeField]
    private float _examineDuration = 4f;

    /// <summary>
    /// Inicializa referencias necesarias.
    /// </summary>
    private void Start()
    {
        _mainCamera = Camera.main.transform;
    }

    /// <summary>
    /// M�todo llamado cuando el jugador interact�a con el objeto.
    /// Inicia la rutina de examen y recolecci�n si no ha sido recogido o examinado.
    /// </summary>
    public void ActivarObjeto()
    {
        if (_isCollected || _beingExamined)
        {
            return;
        }

        _beingExamined = true;
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        StartCoroutine(ExaminarYRecolectar());
    }

    /// <summary>
    /// Corrutina que anima el objeto hacia la c�mara, lo rota lentamente y luego lo destruye.
    /// </summary>
    /// <returns>Un <see cref="IEnumerator"/> para controlar la corrutina.</returns>
    private IEnumerator ExaminarYRecolectar()
    {
        GameController.Instance.ReproducirSonidoCorazon();

        float elapsedTime = 0f;
        Vector3 targetPosition = _mainCamera.position + _mainCamera.forward * 0.5f;

        while (elapsedTime < _examineDuration)
        {
            // Movimiento suave hacia la c�mara
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            // Rotaci�n continua sobre el eje Y
            transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.Self);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameController.Instance.RecolectarCorazon();
        _isCollected = true;
        Destroy(gameObject);
    }
}
