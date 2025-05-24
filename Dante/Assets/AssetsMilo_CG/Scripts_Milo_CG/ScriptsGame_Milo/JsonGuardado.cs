using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonGuardado
{
    private static string rutaArchivo = Application.persistentDataPath + "/progreso.json";

    public static void GuardarDatos(DatosGuardados datos)
    {
        string json = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Datos guardados en: " + rutaArchivo);
    }

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
            return new DatosGuardados();
        }
    }

    public static void BorrarProgreso()
    {
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
            Debug.Log("Archivo de progreso borrado.");
        }
    }

    public static string ObtenerRuta()
    {
        return rutaArchivo;
    }
}
