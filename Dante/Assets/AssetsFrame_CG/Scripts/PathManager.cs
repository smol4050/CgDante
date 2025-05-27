using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip highlightSound;
    private AudioSource audioSource;

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

    private void Awake()
    {
        // Obtén la referencia al AudioSource en este mismo GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PathManager requiere un AudioSource en el mismo GameObject.");
        }
    }

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

        // 1) Cambiar de cámara
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(false);
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(true);

        // 2) Desactivar movimiento
        if (playerMovement != null)
            playerMovement.enabled = false;

        // 3) Resetear todas las baldosas
        foreach (var path in caminos)
            foreach (var parent in path.tileParents)
                if (TryGetTileScript(parent, out var t))
                    t.ResetTile();
        yield return null; // esperar un frame

        // 4) Iluminar el camino + sonido
        currentPathTiles = new List<TileScript>();
        if (caminos.Count > 0)
        {
            int idx = Random.Range(0, caminos.Count);
            foreach (var parent in caminos[idx].tileParents)
            {
                if (TryGetTileScript(parent, out var t))
                {
                    t.Highlight();
                    currentPathTiles.Add(t);

                    // Reproducir sonido de highlight
                    if (audioSource != null && highlightSound != null)
                    {
                        audioSource.PlayOneShot(highlightSound);
                    }
                }
            }
        }

        // 5) Esperar los segundos indicados
        yield return new WaitForSeconds(seconds);

        // 6) Apagar solo la parte visual
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

        // Asegurar cámaras
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(false);
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(true);

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
