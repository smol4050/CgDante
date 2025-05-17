using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDisplayer : MonoBehaviour
{
    [Header("Referencias")]
    public GridManager gridManager;

    [Header("Tiempos")]
    public float highlightTime = 0.6f;
    public float pauseTime = 0.2f;

    [Header("Secuencia")]
    public int patternLength = 5;

    [HideInInspector]
    public List<int> pattern = new List<int>();

    private int currentStep = 0;

    void Start()
    {
        StartCoroutine(InitializeAndShow());
    }

    private IEnumerator InitializeAndShow()
    {
        // Esperar un frame para que GridManager haya corrido
        yield return null;

        // Validar grilla
        if (gridManager == null || gridManager.tiles.Count == 0)
        {
            Debug.LogError("PatternDisplayer: GridManager no configurado o vacío.");
            yield break;
        }

        // Generar un camino contiguo que cruce de abajo hacia arriba
        pattern = GenerateContiguousPath(patternLength);

        // Bloquear movimiento y resetear contador
        PlayerController.Instance.canMove = false;
        currentStep = 0;

        // Mostrar la secuencia uno a uno
        foreach (int idx in pattern)
        {
            var tile = gridManager.tiles[idx];
            tile.Highlight();
            yield return new WaitForSeconds(highlightTime);
            tile.Unhighlight();
            yield return new WaitForSeconds(pauseTime);
        }

        // Asegurarse de que ningún tile quede iluminado
        foreach (var t in gridManager.tiles)
            t.Unhighlight();

        // Permitir al jugador moverse
        PlayerController.Instance.canMove = true;
    }

    /// <summary>
    /// Genera un camino contiguo que parte de y=0 y termina en y=size-1.
    /// </summary>
    private List<int> GenerateContiguousPath(int length)
    {
        int N = gridManager.size;
        int x = Random.Range(0, N);
        int y = 0;
        List<int> path = new List<int> { y * N + x };

        Vector2Int[] moves = {
            Vector2Int.up,    // subir
            Vector2Int.left,  // izquierda
            Vector2Int.right  // derecha
        };

        // Añadir pasos hasta alcanzar la longitud deseada (o tope de altura)
        while (path.Count < length && y < N - 1)
        {
            List<Vector2Int> valids = new List<Vector2Int>();
            foreach (var m in moves)
            {
                int nx = x + m.x, ny = y + m.y;
                if (nx >= 0 && nx < N && ny >= 0 && ny < N)
                    valids.Add(m);
            }
            if (valids.Count == 0) break;

            var mv = valids[Random.Range(0, valids.Count)];
            x += mv.x; y += mv.y;
            path.Add(y * N + x);
        }

        // Si aún no llegamos al tope, avanzar directamente hacia arriba
        while (y < N - 1)
        {
            y++;
            path.Add(y * N + x);
        }

        return path;
    }

    /// <summary>
    /// Llamado por TileTrigger cuando el jugador pisa una baldosa.
    /// </summary>
    public void RegisterStep(int tileIndex)
    {
        if (tileIndex == pattern[currentStep])
        {
            currentStep++;
            if (currentStep >= pattern.Count)
                LevelController.Instance.OnLevelComplete();
        }
        else
        {
            LevelController.Instance.OnLevelFailed();
        }
    }
}
