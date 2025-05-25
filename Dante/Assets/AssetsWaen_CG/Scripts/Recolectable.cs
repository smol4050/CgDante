using System.Collections;
using UnityEngine;

/// <summary>
/// Script para un objeto recolectable que puede ser examinado antes de ser recogido.
/// Implementa la interfaz <see cref="IInteractuable"/> para integrarse con sistemas de interacción.
/// </summary>
public class Recolectable : MonoBehaviour, IInteractuable
{
    /// <summary>
    /// Indica si el objeto ya ha sido recogido.
    /// </summary>
    private bool _isCollected = false;

    /// <summary>
    /// Indica si el objeto está en proceso de ser examinado.
    /// </summary>
    private bool _beingExamined = false;

    /// <summary>
    /// Referencia al transform de la cámara principal.
    /// </summary>
    private Transform _mainCamera;

    /// <summary>
    /// Posición original del objeto antes de ser examinado.
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// Rotación original del objeto antes de ser examinado.
    /// </summary>
    private Quaternion _originalRotation;

    /// <summary>
    /// Duración en segundos del examen antes de recoger el objeto.
    /// </summary>
    [Header("Configuración de examen")]
    [Tooltip("Duración en segundos del examen antes de recoger el objeto.")]
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
    /// Método llamado cuando el jugador interactúa con el objeto.
    /// Inicia la rutina de examen y recolección si no ha sido recogido o examinado.
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
    /// Corrutina que anima el objeto hacia la cámara, lo rota lentamente y luego lo destruye.
    /// </summary>
    /// <returns>Un <see cref="IEnumerator"/> para controlar la corrutina.</returns>
    private IEnumerator ExaminarYRecolectar()
    {
        GameController.Instance.ReproducirSonidoCorazon();

        float elapsedTime = 0f;
        Vector3 targetPosition = _mainCamera.position + _mainCamera.forward * 0.5f;

        while (elapsedTime < _examineDuration)
        {
            // Movimiento suave hacia la cámara
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            // Rotación continua sobre el eje Y
            transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.Self);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameController.Instance.RecolectarCorazon();
        _isCollected = true;
        Destroy(gameObject);
    }
}
