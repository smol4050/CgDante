using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Collider))]
public class DeathZone : MonoBehaviour
{
    [Header("Path Managers que deben resetearse al morir")]
    public PathManager[] pathManagers; // Asignar manualmente en el Inspector

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("[DeathZone] Jugador murió.");

        // 1. Llama OnPlayerDeath() en cada PathManager asignado
        foreach (var manager in pathManagers)
        {
            if (manager != null)
                manager.OnPlayerDeath();
        }

        // 2. Respawnea al jugador usando CheckpointManager
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RespawnPlayer();
        }
        else
        {
            Debug.LogWarning("[DeathZone] No se encontró CheckpointManager.");
        }
    }
}