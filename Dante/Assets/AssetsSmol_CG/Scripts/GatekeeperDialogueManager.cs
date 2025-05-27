using UnityEngine;
using TMPro;
using System.Collections;

public class GatekeeperDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public Canvas dialogueCanvas;
    public TMP_InputField questionInputField;
    public TMP_InputField codeInputField;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 0.05f;

    [Header("Dialogue Config")]
    public float alienSpeakDuration = 2f;
    public int maxQuestions = 3;
    public string correctCode = "042";

    [Header("Doors to Rotate")]
    public GameObject[] doors;

    [Header("Raycast")]
    public Transform rayOrigin;
    public float rayDistance = 3f;
    public LayerMask panelLayer;
    public string panelObjectName = "PanelFBX";

    private string[] playerQuestions = new string[10]
    {
        "Which door is safe?", "Are you the liar?", "Would the other gatekeeper lie?",
        "Can I trust you?", "Will I suffer if I'm wrong?", "Is truth always painful?",
        "Should I go back?", "Is this a trap?", "Do you regret?", "Will I be free?"
    };

    private string[,] gatekeeperResponses = new string[10, 4]
    {
        {
            "Only the brave find salvation beyond the fog.",
            "The safe door is hidden in plain sight.",
            "lol idk but maybe the left one?",
            "Safety is a construct. Are you seeking it, or fleeing from danger?"
        },
        {
            "Lies are many, but truth wears no mask.",
            "I might be. Or maybe not.",
            "heh nope. maybe. yes? no?",
            "To ask if I lie is to doubt the question itself."
        },
        {
            "What he says bends like the flame in wind.",
            "His words mirror mine, but backwards.",
            "The other guy talks weird, not me.",
            "Would you trust a reflection of a shadow?"
        },
        {
            "Only trust earned by blood and fire matters.",
            "Trust is weightless here.",
            "You seem nice. I think?",
            "What is trust, if not hope wearing armor?"
        },
        {
            "Pain is the teacher of those who listen.",
            "Wrong turns echo endlessly here.",
            "owowowow if yes",
            "Suffering sharpens choice like a whetstone to a blade."
        },
        {
            "Truth carves deeper than any blade.",
            "Pain often walks hand-in-hand with honesty.",
            "sometimes... yeah :(",
            "Pain is not truth, but it is often where truth lives."
        },
        {
            "The path forward only opens to those who burn the bridge behind.",
            "You can go back, but it won’t be the same.",
            "reverse.exe not found",
            "What lies behind holds nothing but faded echoes."
        },
        {
            "Every step here is watched by forgotten gods.",
            "There’s always a trap. Even in trust.",
            "yep. jk. or am i?",
            "The question is not if this is a trap, but whether you deserve to escape it."
        },
        {
            "Regret builds the very stones of this realm.",
            "We all regret. Even the gatekeepers.",
            "eh... not really",
            "Regret is memory's way of seeking justice."
        },
        {
            "Freedom lies past the black gate, if your soul is light enough.",
            "One door opens. One binds.",
            "you might. or might not.",
            "Freedom is earned in silence, not choice."
        }
    };

    private bool truthTellerIsLeft;
    private bool doorAIsCorrect;
    private int questionsAsked = 0;
    private bool isFirstInteraction = true;

    void Start()
    {
        dialogueCanvas.enabled = false;
        AssignRandomRoles();
        questionInputField.onSubmit.AddListener(ProcessQuestion);
        codeInputField.onSubmit.AddListener(ProcessCode);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, panelLayer))
            {
                if (hit.collider.gameObject.name == panelObjectName)
                {
                    ShowDialoguePanel();
                }
            }
        }
    }

    void ShowDialoguePanel()
    {
        if (!dialogueCanvas.enabled)
        {
            dialogueCanvas.enabled = true;
            StartCoroutine(IntroDialogue());
        }
    }

    void AssignRandomRoles()
    {
        truthTellerIsLeft = Random.value > 0.5f;
        doorAIsCorrect = Random.value > 0.5f;
    }

    IEnumerator IntroDialogue()
    {
        if (isFirstInteraction)
        {
            isFirstInteraction = false;
            dialogueText.text = "ʘ¥ɸⱣɅ∇⌂ψ…";
            yield return new WaitForSeconds(alienSpeakDuration);
            dialogueText.text = "We shall now speak in your tongue.";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "Ask wisely. You have 3 questions.\nUse ID 0-9.";
        }
    }

    void ProcessQuestion(string input)
    {
        if (questionsAsked >= maxQuestions)
        {
            dialogueText.text = "You have asked all your questions.";
            return;
        }

        if (!int.TryParse(input, out int questionID) || questionID < 0 || questionID >= playerQuestions.Length)
        {
            dialogueText.text = "Invalid ID. Enter a number between 0 and 9.";
            return;
        }

        questionsAsked++;
        questionInputField.text = "";
        StartCoroutine(HandleDialogue(questionID));
    }

    IEnumerator HandleDialogue(int questionID)
    {
        string question = playerQuestions[questionID];
        dialogueText.text = $"Player: {question}";
        yield return new WaitForSeconds(1.5f);

        int styleIndex = Random.Range(0, 4);
        string leftResponse = GetGatekeeperResponse(questionID, styleIndex, truthTellerIsLeft);
        dialogueText.text = $"Left Gatekeeper: {leftResponse}";
        yield return new WaitForSeconds(2f);

        string rightResponse = GetGatekeeperResponse(questionID, styleIndex, !truthTellerIsLeft);
        dialogueText.text = $"Right Gatekeeper: {rightResponse}";
        yield return new WaitForSeconds(2f);

        if (questionsAsked >= maxQuestions)
        {
            dialogueText.text = "Now, enter the code to proceed.";
        }
    }

    string GetGatekeeperResponse(int qIndex, int styleIndex, bool tellsTruth)
    {
        string response = gatekeeperResponses[qIndex, styleIndex];
        return tellsTruth ? response : ScrambleText(response);
    }

    string ScrambleText(string original)
    {
        char[] chars = original.ToCharArray();
        System.Array.Reverse(chars);
        return new string(chars);
    }

    void ProcessCode(string input)
    {
        if (input == correctCode)
        {
            dialogueText.text = "Code accepted. Opening doors...";
            RotateDoors();
        }
        else
        {
            dialogueText.text = "Incorrect code. Try again.";
        }
        codeInputField.text = "";
    }

    void RotateDoors()
    {
        foreach (GameObject door in doors)
        {
            Vector3 newRotation = door.transform.eulerAngles;
            newRotation.y = -53.151f;
            door.transform.eulerAngles = newRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueCanvas.enabled)
        {
            ShowDialoguePanel();
        }
    }
}
