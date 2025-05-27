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

    private bool isPanelActive = false;
    private bool isPlayerInside = false;
    private bool hasEnteredCorrectCode = false;

    void Start()
    {
        codePanelUI.SetActive(false);
        inputField.characterLimit = correctCode.Length;
    }

    void Update()
    {
        if (isPlayerInside && !isPanelActive && !hasEnteredCorrectCode && Input.GetKeyDown(KeyCode.E))
        {
            OpenCodePanel();
        }

        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode(inputField.text);
        }
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
            hasEnteredCorrectCode = true;
            CloseCodePanel();
            this.enabled = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCorrectCode)
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            CloseCodePanel();
        }
    }
}
