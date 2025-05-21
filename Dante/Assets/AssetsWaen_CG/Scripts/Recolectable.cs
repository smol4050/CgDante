using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectable : MonoBehaviour, IInteractuable
{
    private bool _isCollected = false;
    private Transform _mainCamera;
    private bool _beingExamined = false;

    private float _examineDuration = 4f;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;



    void Start()
    {
        _mainCamera = Camera.main.transform;

    }

    void Update()
    {
        
    }

    public void ActivarObjeto()
    {
        if (_isCollected || _beingExamined) return;

        _beingExamined = true;
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        StartCoroutine(ExaminarYRecolectar());
    }

    private IEnumerator ExaminarYRecolectar()
    {
        GameController.Instance.ReproducirSonidoCorazon();
        float elapsedTime = 0f;
        Vector3 targetPosition = _mainCamera.position + _mainCamera.forward * 0.5f;

        while (elapsedTime < _examineDuration)
        {
            // Movimiento hacia la cámara
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

            // Rotación lenta sobre el eje Y (como girando para ser examinado)
            transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.Self);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameController.Instance.RecolectarCorazon();
        Destroy(gameObject);
    }
}
