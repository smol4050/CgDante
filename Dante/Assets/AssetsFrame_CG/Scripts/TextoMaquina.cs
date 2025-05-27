using System.Collections;
using TMPro;
using UnityEngine;

public class TextoMaquina : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dialogoText;    // Tu TMP Text
    public GameObject dialogPanel;         // Panel que agrupa el texto

    [Header("Contenido")]
    [TextArea(3, 10)]
    public string[] parrafos;              // Los p�rrafos a mostrar

    [Header("Configuraci�n")]
    public float velocidadEscritura = 0.03f;

    [Header("Audio")]
    public AudioClip typingClip;           // Sonido de tecleo
    private AudioSource audioSource;       // �altavoz� para el typingClip

    private int index = 0;
    private bool escribiendo = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Componentes
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("A�ade un AudioSource al mismo GameObject de TextoMaquina.");

        // Validaci�n b�sica
        if (dialogoText == null || dialogPanel == null || parrafos.Length == 0)
        {
            Debug.LogError("Falta asignar referencias o array de p�rrafos vac�o.");
            return;
        }

        // Preparar UI
        dialogPanel.SetActive(true);
        index = 0;
        ShowParagraph();
    }

    void Update()
    {
        // Avanzar al presionar F
        if (!escribiendo && dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ContinueText();
        }
    }

    void ShowParagraph()
    {
        // Detener cualquier tecleo anterior
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Arrancar sonido en bucle
        if (typingClip != null && audioSource != null)
        {
            audioSource.clip = typingClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Iniciar escritura
        dialogoText.text = "";
        typingCoroutine = StartCoroutine(TypeParagraph(parrafos[index]));
    }

    IEnumerator TypeParagraph(string texto)
    {
        escribiendo = true;
        dialogoText.text = "";
        foreach (char letra in texto)
        {
            dialogoText.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
        escribiendo = false;

        // Al terminar de escribir, det�n el sonido
        if (audioSource != null) audioSource.Stop();
    }

    void ContinueText()
    {
        // Si a�n se teclea, completa de golpe y det�n sonido
        if (escribiendo)
        {
            StopCoroutine(typingCoroutine);
            dialogoText.text = parrafos[index];
            escribiendo = false;
            if (audioSource != null) audioSource.Stop();
            return;
        }

        // Pasar al siguiente p�rrafo
        index++;
        if (index < parrafos.Length)
        {
            ShowParagraph();
        }
        else
        {
            // Acab� todo: ocultar panel
            dialogPanel.SetActive(false);
        }
    }
}
