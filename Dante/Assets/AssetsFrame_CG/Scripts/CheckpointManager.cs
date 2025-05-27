using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Opcional: si quieres que el jugador empiece en la posición inicial de escena
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
    /// Llamar desde cada Checkpoint cuando el jugador lo active.
    /// </summary>
    public void SetCheckpoint(Transform checkpointTransform)
    {
        lastCheckpointPos = checkpointTransform.position;
        lastCheckpointRot = checkpointTransform.rotation;
        Debug.Log($"[CheckpointManager] Nuevo checkpoint: {lastCheckpointPos}");
    }

    /// <summary>
    /// Mueve al jugador al último checkpoint registrado.
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



