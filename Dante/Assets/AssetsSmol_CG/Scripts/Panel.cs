using UnityEngine;
using TMPro;

public class Panel : MonoBehaviour
{
    public GameObject codePanelUI;
    public TMP_InputField inputField;
    public string correctCode = "1307";
    public GameObject[] doorsToRotate;

    private bool playerInside = false;

    void Start()
    {
        codePanelUI.SetActive(false);
        inputField.characterLimit = correctCode.Length;
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            codePanelUI.SetActive(true);
            SetCursorState(true); // Desbloquea mouse
            inputField.text = "";
            inputField.ActivateInputField();
        }

        if (codePanelUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.text == correctCode)
            {
                OpenDoors();
                codePanelUI.SetActive(false);
                SetCursorState(false); // Bloquea mouse
            }
            else
            {
                inputField.text = "";
                Debug.Log("Código incorrecto");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            codePanelUI.SetActive(false);
            SetCursorState(false); // Bloquea mouse si sale sin completar
        }
    }

    public void ClosePanel()
    {
        codePanelUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OpenDoors()
    {
        foreach (GameObject door in doorsToRotate)
        {
            Vector3 rot = door.transform.eulerAngles;
            rot.y = -53.151f;
            door.transform.eulerAngles = rot;
        }

        Debug.Log("Puertas abiertas");
    }

    /// <summary>
    /// Cambia el estado del cursor.
    /// </summary>
    public void SetCursorState(bool unlocked)
    {
        Cursor.visible = unlocked;
        Cursor.lockState = unlocked ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
