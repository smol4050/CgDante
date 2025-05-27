using UnityEngine;
using TMPro;

public class Panel : MonoBehaviour
{
    [Header("Panel UI que se activa al entrar")]
    public GameObject codePanelUI;

    [Header("InputField para ingresar el código")]
    public TMP_InputField inputField;

    [Header("Código correcto")]
    public string correctCode = "042";

    [Header("Puertas que se abrirán al ingresar el código correcto")]
    public GameObject[] doorsToRotate;

    [Header("Zona que activa este panel (opcional si se quiere desactivar después)")]
    public GameObject zoneTrigger;

    private bool hasEnteredCorrectCode = false;

    void Start()
    {
        if (codePanelUI != null)
            codePanelUI.SetActive(false);

        if (inputField != null)
            inputField.characterLimit = correctCode.Length;

        // Asegurarse de que el mouse esté bloqueado al iniciar
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCorrectCode)
        {
            if (codePanelUI != null)
                codePanelUI.SetActive(true);

            if (inputField != null)
            {
                inputField.text = "";
                inputField.ActivateInputField();
            }

            // Desbloquear mouse al abrir panel
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (codePanelUI != null && codePanelUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode(inputField.text);
        }

        // Permitir cerrar el panel manualmente con Esc
        if (codePanelUI != null && codePanelUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    void CheckCode(string entered)
    {
        if (entered == correctCode)
        {
            Debug.Log("Código correcto. Abriendo puertas...");
            RotateDoors();
            hasEnteredCorrectCode = true;

            ClosePanel();

            if (zoneTrigger != null)
                zoneTrigger.SetActive(false);
        }
        else
        {
            Debug.Log("Código incorrecto.");
            inputField.text = "";
        }
    }

    void ClosePanel()
    {
        if (codePanelUI != null)
            codePanelUI.SetActive(false);

        // Volver a bloquear el mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void RotateDoors()
    {
        foreach (GameObject door in doorsToRotate)
        {
            Vector3 newRotation = door.transform.eulerAngles;
            newRotation.y = -53.151f;
            door.transform.eulerAngles = newRotation;
        }
    }
}
