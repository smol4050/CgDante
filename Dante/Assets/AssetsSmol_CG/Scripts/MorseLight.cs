using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseLight : MonoBehaviour
{
    [Header("Referencia a la Light (Spot o Point)")]
    public Light morseLight;

    [Header("Mensaje a transmitir en Morse")]
    [SerializeField] private string message = "LUX";

    private const float dotDuration = 0.2f;
    private const float dashDuration = 0.6f;
    private const float symbolGap = 0.2f;
    private const float letterGap = 0.6f;
    private const float loopGap = 2.0f;

    private readonly Dictionary<char, string> morseTable = new Dictionary<char, string> {
        {'A', ".-"},   {'B', "-..."}, {'C', "-.-."}, {'D', "-.."},
        {'E', "."},    {'F', "..-."}, {'G', "--."},  {'H', "...."},
        {'I', ".."},   {'J', ".---"}, {'K', "-.-"},  {'L', ".-.."},
        {'M', "--"},   {'N', "-."},   {'O', "---"},  {'P', ".--."},
        {'Q', "--.-"}, {'R', ".-."},  {'S', "..."},  {'T', "-"},
        {'U', "..-"},  {'V', "...-"}, {'W', ".--"},  {'X', "-..-"},
        {'Y', "-.--"}, {'Z', "--.."}
    };

    private void Start()
    {
        StartCoroutine(MorseLoop());
    }

    private IEnumerator MorseLoop()
    {
        while (true)
        {
            
            yield return StartCoroutine(BlinkMorse(message));
            
            yield return new WaitForSeconds(loopGap);
        }
    }

    private IEnumerator BlinkMorse(string text)
    {
        foreach (char c in text.ToUpper())
        {
            if (!morseTable.ContainsKey(c))
                continue;

            string code = morseTable[c];
            foreach (char symbol in code)
            {
                morseLight.enabled = true;
                yield return new WaitForSeconds(symbol == '.' ? dotDuration : dashDuration);
                morseLight.enabled = false;
                yield return new WaitForSeconds(symbolGap);
            }
            yield return new WaitForSeconds(letterGap);
        }
    }
}
