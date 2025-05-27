using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Clase estática para gestionar el guardado, carga y borrado de datos en formato JSON.
/// </summary>
public static class JsonGuardado
{

    
    /// <summary>
    /// Ruta completa del archivo donde se guarda el progreso.
    /// </summary>
    private static string rutaArchivo = Application.persistentDataPath + "/progreso.json";

    /// <summary>
    /// Serializa y guarda los datos en un archivo JSON en disco.
    /// </summary>
    /// <param name="datos">Objeto de tipo <see cref="DatosGuardados"/> que contiene la información a guardar.</param>
    public static void GuardarDatos(DatosGuardados datos)
    {
        string json = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Datos guardados en: " + rutaArchivo);
    }

    /// <summary>
    /// Carga los datos desde el archivo JSON si existe; de lo contrario retorna un nuevo objeto <see cref="DatosGuardados"/>.
    /// </summary>
    /// <returns>Objeto <see cref="DatosGuardados"/> cargado desde el archivo o nuevo si no existe.</returns>
    public static DatosGuardados CargarDatos()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            return JsonUtility.FromJson<DatosGuardados>(json);
        }
        else
        {
            Debug.LogWarning("No se encontró archivo de progreso, se crea nuevo.");
             // Asignar un nombre por defecto
            return null;
        }
    }

    /// <summary>
    /// Elimina el archivo de progreso si existe.
    /// </summary>
    public static void BorrarProgreso()
    {
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
            Debug.Log("Archivo de progreso borrado.");
        }
    }

    /// <summary>
    /// Obtiene la ruta completa del archivo JSON donde se guarda el progreso.
    /// </summary>
    /// <returns>Cadena con la ruta completa del archivo JSON.</returns>
    public static string ObtenerRuta()
    {
        return rutaArchivo;
    }
}