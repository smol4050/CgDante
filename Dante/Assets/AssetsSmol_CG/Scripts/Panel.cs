using UnityEngine;
using TMPro;

public class Panel : MonoBehaviour
{
    [Header("UI")]
    public GameObject codePanelUI;
    public TMP_InputField inputField;

    [Header("Configuración del código")]
    public string correctCode = "042";

    [Header("Puertas a abrir")]
    public GameObject[] doorsToRotate;

    [Header("Interacción")]
    public LayerMask panelLayer;

    private Camera mainCamera;
    private bool isPanelActive = false;

    void Start()
    {
        mainCamera = Camera.main;
        codePanelUI.SetActive(false);
        inputField.characterLimit = correctCode.Length;
    }

    void Update()
    {
        if (!isPanelActive && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerLookingAtPanel())
            {
                OpenCodePanel();
            }
        }

        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode(inputField.text);
        }
    }

    bool PlayerLookingAtPanel()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, 4f, panelLayer) && hit.collider.gameObject == gameObject;
    }

    void OpenCodePanel()
    {
        isPanelActive = true;
        codePanelUI.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void CloseCodePanel()
    {
        isPanelActive = false;
        codePanelUI.SetActive(false);
    }

    void CheckCode(string entered)
    {
        if (entered == correctCode)
        {
            Debug.Log("Código correcto. Abriendo puertas...");
            RotateDoors();
            CloseCodePanel();
        }
        else
        {
            Debug.Log("Código incorrecto.");
            inputField.text = "";
        }
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
