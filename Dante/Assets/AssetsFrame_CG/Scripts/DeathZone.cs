using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zona de muerte que, al entrar el jugador,
/// resetea todos los caminos y lo envía al checkpoint.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DeathZone : MonoBehaviour
{
    /// <summary>
    /// Array de PathManagers que deben resetearse al morir.
    /// </summary>
    [Header("Path Managers que deben resetearse al morir")]
    public PathManager[] pathManagers;

    private PausarReanudar pausarReanudar;

    /// <summary>
    /// Inicializa referencia al sistema de pausa/GameOver.
    /// </summary>
    public void Start()
    {
        pausarReanudar = FindObjectOfType<PausarReanudar>();
        if (pausarReanudar == null)
            Debug.LogWarning("[DeathZone] No se encontró PausarReanudar en la escena.");
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Al colisionar con el jugador, muestra Game Over,
    /// resetea todos los PathManagers y realiza respawn.
    /// </summary>
    /// <param name="other">Collider del jugador (tag "Player").</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("[DeathZone] Jugador murió.");
        pausarReanudar.MostrarGameOver();

        foreach (var manager in pathManagers)
            manager?.OnPlayerDeath();

        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.RespawnPlayer();
        else
            Debug.LogWarning("[DeathZone] No se encontró CheckpointManager.");
    }
}
