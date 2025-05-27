using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la lógica de memoria: muestra y oculta caminos,
/// alterna cámaras y bloquea el movimiento del jugador.
/// </summary>
public class PathManager : MonoBehaviour
{
    [Header("Audio")]
    /// <summary>Sonido al iluminar cada baldosa.</summary>
    public AudioClip highlightSound;
    /// <summary>Sonido de tictac durante la secuencia.</summary>
    public AudioClip tickSound;
    private AudioSource audioSource;

    [Header("Cámaras")]
    /// <summary>Cámara FPS del jugador.</summary>
    public Camera fpsCamera;
    /// <summary>Cámara fija para la secuencia.</summary>
    public Camera sequenceCamera;

    [Header("Paths")]
    /// <summary>Listado de caminos disponibles.</summary>
    public List<TilePath> caminos = new List<TilePath>();

    [Header("Movimiento jugador")]
    /// <summary>Script que controla el movimiento del jugador.</summary>
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

    /// <summary>
    /// Inicia la secuencia de memoria: muestra y luego oculta el camino.
    /// </summary>
    public void StartMemorySequence()
    {
        if (state != State.WaitingForButton)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowThenHideSequence(5f));
    }

    /// <summary>
    /// Coroutine que gestiona:
    /// mostrar el camino, reproducir audio de tictac, esperar y ocultar.
    /// </summary>
    /// <param name="seconds">Tiempo en segundos que permanece visible.</param>
    private IEnumerator ShowThenHideSequence(float seconds)
    {
        state = State.ShowingSequence;

        // 1) Cambiar de cámara
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(false);
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(true);

        // 2) Desactivar movimiento
        if (playerMovement != null) playerMovement.enabled = false;

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
                if (TryGetTileScript(parent, out var t))
                {
                    t.Highlight();
                    currentPathTiles.Add(t);
                    if (highlightSound != null)
                        audioSource.PlayOneShot(highlightSound);
                }
        }

        // ► Inicia tictac en bucle
        if (tickSound != null)
        {
            audioSource.clip = tickSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        // 5) Esperar los segundos indicados
        yield return new WaitForSeconds(seconds);

        // ► Para el tictac
        audioSource.loop = false;
        audioSource.Stop();

        // 6) Apagar solo la visual
        foreach (var t in currentPathTiles)
            t.HideHighlight();

        // 7) Restaurar estado de juego
        if (sequenceCamera != null) sequenceCamera.gameObject.SetActive(false);
        if (fpsCamera != null) fpsCamera.gameObject.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = true;

        state = State.Playing;
    }

    /// <summary>
    /// Resetea al estado inicial cuando el jugador muere,
    /// apaga todas las baldosas y espera nuevo botón.
    /// </summary>
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

    /// <summary>
    /// Obtiene el componente <see cref="TileScript"/> en un GameObject,
    /// incluso si está en un hijo. Emite warning si no lo encuentra.
    /// </summary>
    /// <param name="parent">GameObject contenedor del <see cref="TileScript"/>.</param>
    /// <param name="tile">Salida con la instancia encontrada.</param>
    /// <returns>
    /// True si se encontró el <see cref="TileScript"/>, false en caso contrario.
    /// </returns>
    private bool TryGetTileScript(GameObject parent, out TileScript tile)
    {
        tile = parent.GetComponentInChildren<TileScript>();
        if (tile == null)
            Debug.LogWarning($"TileScript no encontrado en {parent.name}");
        return tile != null;
    }
}
