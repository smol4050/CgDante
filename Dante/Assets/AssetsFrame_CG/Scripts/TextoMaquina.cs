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
    public string[] parrafos;              // Los párrafos a mostrar

    [Header("Configuración")]
    public float velocidadEscritura = 0.03f;

    [Header("Audio")]
    public AudioClip typingClip;           // Sonido de tecleo
    private AudioSource audioSource;       // “altavoz” para el typingClip

    private int index = 0;
    private bool escribiendo = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Componentes
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Añade un AudioSource al mismo GameObject de TextoMaquina.");

        // Validación básica
        if (dialogoText == null || dialogPanel == null || parrafos.Length == 0)
        {
            Debug.LogError("Falta asignar referencias o array de párrafos vacío.");
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

        // Al terminar de escribir, detén el sonido
        if (audioSource != null) audioSource.Stop();
    }

    void ContinueText()
    {
        // Si aún se teclea, completa de golpe y detén sonido
        if (escribiendo)
        {
            StopCoroutine(typingCoroutine);
            dialogoText.text = parrafos[index];
            escribiendo = false;
            if (audioSource != null) audioSource.Stop();
            return;
        }

        // Pasar al siguiente párrafo
        index++;
        if (index < parrafos.Length)
        {
            ShowParagraph();
        }
        else
        {
            // Acabó todo: ocultar panel
            dialogPanel.SetActive(false);
        }
    }
}
