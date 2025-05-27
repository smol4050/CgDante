using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton que gestiona la posición de respawn del jugador mediante checkpoints.
/// Persiste entre escenas gracias a <see cref="DontDestroyOnLoad"/>.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    /// <summary>
    /// Instancia global única de <see cref="CheckpointManager"/>.
    /// </summary>
    public static CheckpointManager Instance { get; private set; }

    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Opcional: al cargar la escena, tomar la posición inicial del jugador
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                lastCheckpointPos = player.transform.position;
                lastCheckpointRot = player.transform.rotation;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Actualiza el punto de respawn al transform del checkpoint.
    /// </summary>
    /// <param name="checkpointTransform">
    /// Transform del objeto checkpoint que ha sido activado.
    /// </param>
    public void SetCheckpoint(Transform checkpointTransform)
    {
        lastCheckpointPos = checkpointTransform.position;
        lastCheckpointRot = checkpointTransform.rotation;
        Debug.Log($"[CheckpointManager] Nuevo checkpoint: {lastCheckpointPos}");
    }

    /// <summary>
    /// Teletransporta al jugador al último checkpoint registrado.
    /// </summary>
    public void RespawnPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Desactiva CharacterController para teletransportar
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = lastCheckpointPos;
        player.transform.rotation = lastCheckpointRot;

        if (cc != null) cc.enabled = true;

        Debug.Log("[CheckpointManager] Jugador reaparecido en último checkpoint");
    }
}
