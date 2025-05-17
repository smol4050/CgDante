using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    [HideInInspector]
    public int index;                       // Índice único asignado en GridManager

    private PatternDisplayer displayer;

    void Start()
    {
        // Busca la instancia de PatternDisplayer en la escena
        displayer = FindObjectOfType<PatternDisplayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo cuenta pasos cuando el jugador puede moverse
        if (!PlayerController.Instance.canMove) return;

        // Asegúrate de que el jugador tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            displayer.RegisterStep(index);
        }
    }
}
