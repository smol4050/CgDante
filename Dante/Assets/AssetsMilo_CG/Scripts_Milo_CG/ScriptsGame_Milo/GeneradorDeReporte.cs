using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Clase que genera un reporte del progreso de niveles.
/// </summary>
public static class GeneradorDeReporte
{
    /// <summary>
    /// Genera un string con el reporte del progreso actual.
    /// </summary>
    public static string Generar()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager no está disponible.");
            return "GameManager no disponible.";
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("===== REPORTE DE PROGRESO =====");
        sb.AppendLine($"Nombre Partida: {GameManager.Instance.nombrePartida}\n\n");

        foreach (var nivel in GameManager.Instance.progresoPorNivel)
        {
            sb.AppendLine($"Nivel: {nivel.nombreNivel}");
            sb.AppendLine($"  - Objetos recolectados: {nivel.objetosRecolectados}/{GameManager.Instance.totalObjetosPorNivel}");
            sb.AppendLine($"  - Completado: {(nivel.nivelCompletado ? "Sí" : "No")}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
