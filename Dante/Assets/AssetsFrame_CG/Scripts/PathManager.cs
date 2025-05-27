using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip highlightSound;   // sonido al iluminar cada tile
    public AudioClip tickSound;        // sonido de tictac durante la secuencia
    private AudioSource audioSource;

    [Header("Cámaras")]
    public Camera fpsCamera;
    public Camera sequenceCamera;

    [Header("Paths")]
    public List<TilePath> caminos = new List<TilePath>();

    [Header("Movimiento jugador")]
    public MonoBehaviour playerMovement;

    private List<TileScript> currentPathTiles;
    private enum State { WaitingForButton, ShowingSequence, Playing }
    private State state = State.WaitingForButton;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("PathManager requiere un AudioSource en el mismo GameObject.");
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
        yield return null;

        // 4) Iluminar el camino + sonido de highlight
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

                    if (highlightSound != null)
                        audioSource.PlayOneShot(highlightSound);
                }
            }
        }

        // ► Inicia el tictac en bucle
        if (tickSound != null)
        {
            audioSource.clip = tickSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        // 5) Esperar los segundos indicados (con tictac sonando)
        yield return new WaitForSeconds(seconds);

        // ► Para el tictac
        audioSource.loop = false;
        audioSource.Stop();

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

        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(false);
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(true);

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
