using UnityEngine;
using TMPro;
using System.Collections;

public class GatekeeperDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 0.05f;

    [Header("Dialogue Config")]
    public float alienSpeakDuration = 2f;
    public int maxQuestions = 3;

    private string[] playerQuestions = new string[20]
    {
        "Which door is safe?",
        "Are you the liar?",
        "Is the left door correct?",
        "Would the other gatekeeper lie?",
        "Can I trust you?",
        "Is this the final challenge?",
        "Will I suffer if I'm wrong?",
        "Are you human?",
        "Is truth always painful?",
        "Do you know me?",
        "Does this path matter?",
        "Is fear real?",
        "Should I go back?",
        "Is this a trap?",
        "Am I dreaming?",
        "Do you regret?",
        "Do you hate lies?",
        "Are lies helpful?",
        "Will I be free?",
        "Is this the end?"
    };

    private string[,] gatekeeperResponses = new string[20, 20];
    private bool truthTellerIsLeft;
    private bool doorAIsCorrect;
    private int questionsAsked = 0;
    private bool isFirstInteraction = true;

    void Start()
    {
        AssignRandomRoles();
        FillDummyResponses();
        inputField.onSubmit.AddListener(ProcessQuestion);
        StartCoroutine(IntroDialogue());
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
            dialogueText.text = "Ʌξᚨ₸∑$¢∇…";
            yield return new WaitForSeconds(alienSpeakDuration);
            dialogueText.text = "We shall now speak in your tongue.";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "Ask wisely. You have 3 questions.";
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
            dialogueText.text = "Invalid ID. Enter a number between 0 and 19.";
            return;
        }

        questionsAsked++;
        inputField.text = "";

        StartCoroutine(HandleDialogue(questionID));
    }

    IEnumerator HandleDialogue(int questionID)
    {
        string question = playerQuestions[questionID];
        dialogueText.text = $"Player: {question}";
        yield return new WaitForSeconds(1.5f);

        string leftResponse = GetGatekeeperResponse(questionID, Random.Range(0, 20), truthTellerIsLeft);
        dialogueText.text = $"Left Gatekeeper: {leftResponse}";
        yield return new WaitForSeconds(2f);

        string rightResponse = GetGatekeeperResponse(questionID, Random.Range(0, 20), !truthTellerIsLeft);
        dialogueText.text = $"Right Gatekeeper: {rightResponse}";
        yield return new WaitForSeconds(2f);

        if (questionsAsked >= maxQuestions)
        {
            dialogueText.text = "Choose your door.";
        }
    }

    string GetGatekeeperResponse(int qIndex, int rIndex, bool tellsTruth)
    {
        string response = gatekeeperResponses[qIndex, rIndex];
        return tellsTruth ? response : ScrambleText(response);
    }

    string ScrambleText(string original)
    {
        char[] chars = original.ToCharArray();
        System.Array.Reverse(chars);
        return new string(chars);
    }

    void FillDummyResponses()
    {
        for (int q = 0; q < 20; q++)
        {
            for (int r = 0; r < 20; r++)
            {
                gatekeeperResponses[q, r] = $"Response {r} to Q{q}";
            }
        }
    }
}
