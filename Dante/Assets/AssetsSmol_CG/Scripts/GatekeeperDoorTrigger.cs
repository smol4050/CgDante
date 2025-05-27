using UnityEngine;
using UnityEngine.SceneManagement;

public class GatekeeperDoorTrigger : MonoBehaviour
{
    [Header("Player Trigger Settings")]
    public string playerTag = "Player";

    [Header("Final Settings")]
    public GameObject finalPanel;          // Panel mostrado al elegir la puerta correcta
    public string wrongSceneName;          // Escena cargada al elegir la puerta incorrecta

    private bool hasTriggered = false;
    private bool correctIsLeft;

    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);

        // Decide aleatoriamente cuál puerta es la correcta
        correctIsLeft = Random.value > 0.5f;

        Debug.Log("La puerta correcta es la " + (correctIsLeft ? "IZQUIERDA" : "DERECHA"));
    }

    // Llamado por cada puerta cuando el jugador entra
    public void DoorTriggered(bool isLeftDoor, Collider other)
    {
        if (hasTriggered || !other.CompareTag(playerTag)) return;

        hasTriggered = true;

        if (isLeftDoor == correctIsLeft)
        {
            if (finalPanel != null)
                finalPanel.SetActive(true);

            Debug.Log("Final verdadero alcanzado.");
        }
        else
        {
            Debug.Log("Final incorrecto. Cargando escena equivocada...");
            SceneManager.LoadScene(wrongSceneName);
        }
    }
}
