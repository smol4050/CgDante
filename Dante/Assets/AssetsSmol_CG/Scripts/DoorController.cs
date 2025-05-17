using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorControllerTMP : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField inputField;
    public Button submitButton;

    [Header("Door Rotation Settings")]
    public Transform doorTransform;
    public float openDuration = 1f;
    public float openAngle = 45f;

    private const string correctCode = "LUX";

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitCode);
    }

    private void OnSubmitCode()
    {
        string userInput = inputField.text.Trim().ToUpper();
        if (userInput == correctCode)
        {
            StartCoroutine(RotateDoor());
        }
        else
        {
            Debug.Log("Código incorrecto");
        }
    }

    private IEnumerator RotateDoor()
    {
        float elapsed = 0f;
        Quaternion startRot = doorTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, openAngle, 0);

        while (elapsed < openDuration)
        {
            doorTransform.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / openDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorTransform.localRotation = endRot;
    }
}
