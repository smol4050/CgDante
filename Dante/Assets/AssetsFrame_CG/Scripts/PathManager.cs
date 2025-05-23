using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("Cámaras")]
    public Camera fpsCamera;
    public Camera sequenceCamera;

    [Header("Paths")]
    public List<TilePath> caminos = new List<TilePath>();

    [Header("Movimiento jugador")]
    public MonoBehaviour playerMovement; // tu script de movimiento

    private List<TileScript> currentPathTiles;
    private enum State { WaitingForButton, ShowingSequence, Playing }
    private State state = State.WaitingForButton;

    public void StartMemorySequence()
    {
        if (state != State.WaitingForButton)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowThenHideSequence(5f));
    }

    private IEnumerator ShowThenHideSequence(float seconds)
    {
        state = State.ShowingSequence;

        // 1) Cambiar de cámara (GameObject)
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(false);
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(true);

        // 2) Desactivar movimiento
        if (playerMovement != null)
            playerMovement.enabled = false;

        // 3) Resetea todas las baldosas
        foreach (var path in caminos)
            foreach (var parent in path.tileParents)
                if (TryGetTileScript(parent, out var t))
                    t.ResetTile();
        yield return null; // esperar un frame

        // 4) Ilumina el camino
        currentPathTiles = new List<TileScript>();
        if (caminos.Count > 0)
        {
            int idx = Random.Range(0, caminos.Count);
            foreach (var parent in caminos[idx].tileParents)
                if (TryGetTileScript(parent, out var t))
                {
                    t.Highlight();
                    currentPathTiles.Add(t);
                }
        }

        // 5) Espera los segundos
        yield return new WaitForSeconds(seconds);

        // 6) Apaga solo la parte visual (mantiene isTrigger = false)
        foreach (var t in currentPathTiles)
            t.HideHighlight();

        // 7) Restaurar estado de juego
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(false);
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = true;

        state = State.Playing;
    }

    public void OnPlayerDeath()
    {
        state = State.WaitingForButton;

        // Asegura que FPS esté activa y secuencia apagada
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(false);
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(true);

        // Desactivar movimiento
        if (playerMovement != null) playerMovement.enabled = false;

        // Reset visual y triggers
        foreach (var path in caminos)
            foreach (var parent in path.tileParents)
                if (TryGetTileScript(parent, out var t))
                    t.ResetTile();
    }

    private bool TryGetTileScript(GameObject parent, out TileScript tile)
    {
        tile = parent.GetComponentInChildren<TileScript>();
        if (tile == null)
            Debug.LogWarning($"TileScript no encontrado en {parent.name}");
        return tile != null;
    }
}
