using UnityEngine;
using TMPro;

/// <summary>
/// Controla la interacción del jugador con un panel de código para abrir puertas.
/// </summary>
public class Panel : MonoBehaviour
{
    /// <summary>
    /// Interfaz de usuario del panel de código.
    /// </summary>
    public GameObject codePanelUI;

    /// <summary>
    /// Campo de entrada para que el jugador ingrese el código.
    /// </summary>
    public TMP_InputField inputField;

    /// <summary>
    /// Código correcto que desbloquea las puertas.
    /// </summary>
    public string correctCode = "1307";

    /// <summary>
    /// Conjunto de puertas que se rotarán al ingresar el código correcto.
    /// </summary>
    public GameObject[] doorsToRotate;

    /// <summary>
    /// Indica si el jugador está dentro del área de activación del panel.
    /// </summary>
    private bool playerInside = false;

    /// <summary>
    /// Inicializa el panel desactivado y configura el límite de caracteres del campo de entrada.
    /// </summary>
    void Start()
    {
        codePanelUI.SetActive(false);
        inputField.characterLimit = correctCode.Length;
    }

    /// <summary>
    /// Maneja la lógica de entrada del jugador para mostrar el panel y verificar el código ingresado.
    /// </summary>
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

    /// <summary>
    /// Detecta cuando el jugador entra en el área de activación del panel.
    /// </summary>
    /// <param name="other">Colisionador que entra en el área de activación.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    /// <summary>
    /// Detecta cuando el jugador sale del área de activación del panel.
    /// </summary>
    /// <param name="other">Colisionador que sale del área de activación.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            codePanelUI.SetActive(false);
            SetCursorState(false); // Bloquea mouse si sale sin completar
        }
    }

    /// <summary>
    /// Cierra el panel de código y bloquea el cursor del jugador.
    /// </summary>
    public void ClosePanel()
    {
        codePanelUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Rota las puertas para simular su apertura.
    /// </summary>
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
    /// Cambia el estado del cursor del jugador.
    /// </summary>
    /// <param name="unlocked">Si es verdadero, desbloquea y muestra el cursor; de lo contrario, lo bloquea y oculta.</param>
    public void SetCursorState(bool unlocked)
    {
        Cursor.visible = unlocked;
        Cursor.lockState = unlocked ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
