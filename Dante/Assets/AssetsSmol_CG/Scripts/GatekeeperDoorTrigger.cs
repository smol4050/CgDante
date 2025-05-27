using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la lógica de las puertas del guardián, determinando si el jugador elige la puerta correcta.
/// </summary>
public class GatekeeperDoorTrigger : MonoBehaviour
{
    /// <summary>
    /// Etiqueta que identifica al jugador.
    /// </summary>
    [Header("Player Trigger Settings")]
    public string playerTag = "Player";

    /// <summary>
    /// Panel mostrado al elegir la puerta correcta.
    /// </summary>
    [Header("Final Settings")]
    public GameObject finalPanel;

    /// <summary>
    /// Nombre de la escena cargada al elegir la puerta incorrecta.
    /// </summary>
    public string wrongSceneName;

    /// <summary>
    /// Indica si ya se ha activado una puerta.
    /// </summary>
    private bool hasTriggered = false;

    /// <summary>
    /// Determina aleatoriamente si la puerta correcta es la izquierda.
    /// </summary>
    private bool correctIsLeft;

    /// <summary>
    /// Inicializa el estado del panel final y decide aleatoriamente la puerta correcta.
    /// </summary>
    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);

        correctIsLeft = Random.value > 0.5f;

        Debug.Log("La puerta correcta es la " + (correctIsLeft ? "IZQUIERDA" : "DERECHA"));
    }

    /// <summary>
    /// Llamado cuando el jugador interactúa con una puerta.
    /// </summary>
    /// <param name="isLeftDoor">Indica si la puerta activada es la izquierda.</param>
    /// <param name="other">Colisionador del objeto que activó la puerta.</param>
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
