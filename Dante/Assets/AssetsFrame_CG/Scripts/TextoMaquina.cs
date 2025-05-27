using System.Collections;
using TMPro;
using UnityEngine;

public class TextoMaquina : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dialogoText;    // Tu TMP Text
    public GameObject dialogPanel;         // Panel padre que agrupa texto

    [Header("Contenido")]
    [TextArea(3, 10)]
    public string[] parrafos;              // Los párrafos a mostrar

    [Header("Configuración")]
    public float velocidadEscritura = 0.03f;

    private int index = 0;
    private bool escribiendo = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Validaciones básicas
        if (dialogoText == null || dialogPanel == null || parrafos.Length == 0)
        {
            Debug.LogError("Falta asignar dialogoText, dialogPanel o no hay párrafos.");
            return;
        }

        dialogPanel.SetActive(true);
        index = 0;
        ShowParagraph();
    }

    void Update()
    {
        // Avanzar con la tecla F
        if (!escribiendo && dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ContinueText();
        }
    }

    void ShowParagraph()
    {
        dialogoText.text = "";
        // Si hay una corrutina activa, la detenemos
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

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
    }

    void ContinueText()
    {
        // Si todavía está escribiendo, completamos al instante
        if (escribiendo)
        {
            StopCoroutine(typingCoroutine);
            dialogoText.text = parrafos[index];
            escribiendo = false;
            return;
        }

        // Avanzar al siguiente párrafo
        index++;
        if (index < parrafos.Length)
        {
            ShowParagraph();
        }
        else
        {
            // Todos los párrafos mostrados: ocultar panel
            dialogPanel.SetActive(false);
        }
    }
}
