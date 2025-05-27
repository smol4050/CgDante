using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Muestra una serie de párrafos en un panel UI con
/// efecto de máquina de escribir y sonido de tecleo.
/// </summary>
public class TextoMaquina : MonoBehaviour
{
    [Header("UI References")]
    /// <summary>Campo de texto TMP donde se escribe el diálogo.</summary>
    public TextMeshProUGUI dialogoText;
    /// <summary>Panel que agrupa el texto y controles.</summary>
    public GameObject dialogPanel;

    [Header("Contenido")]
    /// <summary>Array de párrafos que se mostrarán en secuencia.</summary>
    [TextArea(3, 10)]
    public string[] parrafos;

    [Header("Configuración")]
    /// <summary>Retraso en segundos entre cada carácter.</summary>
    public float velocidadEscritura = 0.03f;

    [Header("Audio")]
    /// <summary>Clip de audio que suena al teclear.</summary>
    public AudioClip typingClip;
    private AudioSource audioSource;

    private int index = 0;
    private bool escribiendo = false;
    private Coroutine typingCoroutine;

    /// <summary>
    /// Inicializa componentes y comienza con el primer párrafo.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Añade un AudioSource al mismo GameObject de TextoMaquina.");

        if (dialogoText == null || dialogPanel == null || parrafos.Length == 0)
        {
            Debug.LogError("Falta asignar referencias o array de párrafos vacío.");
            return;
        }

        dialogPanel.SetActive(true);
        index = 0;
        ShowParagraph();
    }

    /// <summary>
    /// Controla la tecla F para avanzar el diálogo cuando no está escribiendo.
    /// </summary>
    void Update()
    {
        if (!escribiendo && dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ContinueText();
        }
    }

    /// <summary>
    /// Inicia la escritura del párrafo actual con sonido y efecto.
    /// </summary>
    private void ShowParagraph()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (typingClip != null && audioSource != null)
        {
            audioSource.clip = typingClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        dialogoText.text = "";
        typingCoroutine = StartCoroutine(TypeParagraph(parrafos[index]));
    }

    /// <summary>
    /// Corrutina que escribe carácter a carácter con retardo.
    /// </summary>
    /// <param name="texto">El párrafo completo a mostrar.</param>
    private IEnumerator TypeParagraph(string texto)
    {
        escribiendo = true;
        dialogoText.text = "";
        foreach (char letra in texto)
        {
            dialogoText.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
        escribiendo = false;
        if (audioSource != null) audioSource.Stop();
    }

    /// <summary>
    /// Avanza al siguiente párrafo o cierra el panel si ya no hay más.
    /// </summary>
    private void ContinueText()
    {
        // Si aún se teclea, mostramos el texto completo
        if (escribiendo)
        {
            StopCoroutine(typingCoroutine);
            dialogoText.text = parrafos[index];
            escribiendo = false;
            if (audioSource != null) audioSource.Stop();
            return;
        }

        index++;
        if (index < parrafos.Length)
        {
            ShowParagraph();
        }
        else
        {
            dialogPanel.SetActive(false);
        }
    }
}
