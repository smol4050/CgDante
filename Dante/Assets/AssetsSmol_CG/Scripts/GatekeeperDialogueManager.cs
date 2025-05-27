using UnityEngine;
using TMPro;
using System.Collections;

public class GatekeeperDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public Canvas dialogueCanvas;
    public GameObject dialoguePanel;
    public TMP_InputField questionInputField;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 0.05f;

    [Header("Dialogue Config")]
    public float alienSpeakDuration = 2f;
    public int maxQuestions = 3;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip alienVoiceClip;

    [Header("Player Trigger Settings")]
    public string playerTag = "Player";

    [Header("Gatekeeper References")]
    public GameObject leftGatekeeper;
    public GameObject rightGatekeeper;

    [Header("Door Colliders (Trigger Zones)")]
    public Collider leftDoorTrigger;
    public Collider rightDoorTrigger;

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
    private int questionsAsked = 0;
    private bool leftStartsFirst;
    private bool hasTriggered = false;

    void Start()
    {
        dialogueCanvas.enabled = true;
        dialoguePanel.SetActive(false);
        questionInputField.onSubmit.AddListener(ProcessQuestion);
        AssignRandomRoles();
    }

    void AssignRandomRoles()
    {
        truthTellerIsLeft = Random.value > 0.5f;
        leftStartsFirst = Random.value > 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag(playerTag)) return;

        // Solo reacciona si el jugador entra en una de las puertas asignadas
        if (other == leftDoorTrigger || other == rightDoorTrigger)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (hasTriggered) return;

        hasTriggered = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(IntroDialogue());
    }

    IEnumerator IntroDialogue()
    {
        yield return StartCoroutine(TypeText("ʘ¥ɸⱣɅ∇⌂ψ…", alienSpeakDuration));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TypeText("We shall now speak in your tongue.", 2f));
        yield return new WaitForSeconds(0.5f);
        dialogueText.text = "Ask wisely. You have 3 questions.\nUse ID 0-9.";
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
        yield return StartCoroutine(TypeText($"Player: {question}", 1.5f));

        int styleIndex = Random.Range(0, 4);

        string firstResponse = GetGatekeeperResponse(questionID, styleIndex, leftStartsFirst ? truthTellerIsLeft : !truthTellerIsLeft);
        yield return StartCoroutine(TypeText($"{(leftStartsFirst ? "Left" : "Right")} Gatekeeper: {firstResponse}", 2f));

        string secondResponse = GetGatekeeperResponse(questionID, styleIndex, !leftStartsFirst ? truthTellerIsLeft : !truthTellerIsLeft);
        yield return StartCoroutine(TypeText($"{(!leftStartsFirst ? "Left" : "Right")} Gatekeeper: {secondResponse}", 2f));

        if (questionsAsked >= maxQuestions)
        {
            dialogueText.text = "That is all. Choose your path.";
            dialoguePanel.SetActive(false);
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

    IEnumerator TypeText(string text, float duration)
    {
        if (voiceSource != null && alienVoiceClip != null)
        {
            voiceSource.clip = alienVoiceClip;
            voiceSource.loop = true;
            voiceSource.Play();
        }

        dialogueText.text = "";
        float delay = duration / Mathf.Max(1, text.Length);

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();
        }
    }
}
