using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public List<ProgresoNivel> progresoPorNivel = new List<ProgresoNivel>();
    public int totalObjetosPorNivel = 10;

    //  Evento que se dispara cuando se recolecta un objeto
    public event Action OnObjetoRecolectado;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (File.Exists(JsonGuardado.ObtenerRuta()))
            {
                CargarProgresoDesdeJson();
                Debug.Log("Progreso cargado desde JSON.");
            }
            else
            {
                InicializarProgreso();
                Debug.Log("Progreso inicializado.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InicializarProgreso()
    {
        // Agrega aqu� los nombres REALES de tus escenas
        progresoPorNivel.Add(new ProgresoNivel("Waen_CG"));
        progresoPorNivel.Add(new ProgresoNivel("Frame_CG"));
        progresoPorNivel.Add(new ProgresoNivel("Milo_CG"));
        progresoPorNivel.Add(new ProgresoNivel("Smol_CG"));
    }

    public ProgresoNivel GetProgresoNivelActual()
    {
        string nivelActual = SceneManager.GetActiveScene().name;
        return progresoPorNivel.Find(p => p.nombreNivel == nivelActual);
    }

    public void SumarObjeto()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso == null) return;

        progreso.objetosRecolectados++;
        Debug.Log($"[{progreso.nombreNivel}] Objetos recolectados: {progreso.objetosRecolectados}/{totalObjetosPorNivel}");

        OnObjetoRecolectado?.Invoke();

        if (progreso.objetosRecolectados >= totalObjetosPorNivel)
        {
            Debug.Log("�Todos los objetos recolectados!");
            // Aqu� puedes llamar algo como: GameController.Instance.PuertaFinalDisponible();
        }
    }

    public int ObtenerObjetosRecolectados()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        return progreso != null ? progreso.objetosRecolectados : 0;
    }

    public void CompletarNivel()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso != null)
        {
            progreso.nivelCompletado = true;
            Debug.Log($"Nivel completado: {progreso.nombreNivel}");
            GuardarProgresoEnJson(); // Guardamos el progreso justo cuando se completa el nivel
        }
    }

    public string ObtenerSiguienteNivelNoCompletado()
    {
        foreach (ProgresoNivel nivel in progresoPorNivel)
        {
            if (!nivel.nivelCompletado)
            {
                return nivel.nombreNivel;
            }
        }

        Debug.Log("Todos los niveles est�n completados.");
        return null; // Puedes devolver un nombre especial como "Creditos" si quieres
    }

    public void ReiniciarObjetosDelNivelActual()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso != null)
            progreso.objetosRecolectados = 0;
    }

    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }


    public void GuardarProgresoEnJson()
    {
        DatosGuardados datos = new DatosGuardados();

        foreach (ProgresoNivel prog in progresoPorNivel)
        {
            datos.progresoPorNivel.Add(new ProgresoNivelGuardado
            {
                nombreNivel = prog.nombreNivel,
                objetosRecolectados = prog.objetosRecolectados,
                nivelCompletado = prog.nivelCompletado
            });
        }

        JsonGuardado.GuardarDatos(datos);
    }

    public void CargarProgresoDesdeJson()
    {
        DatosGuardados datos = JsonGuardado.CargarDatos();
        progresoPorNivel.Clear();

        foreach (ProgresoNivelGuardado prog in datos.progresoPorNivel)
        {
            progresoPorNivel.Add(new ProgresoNivel(prog.nombreNivel)
            {
                objetosRecolectados = prog.objetosRecolectados,
                nivelCompletado = prog.nivelCompletado
            });
        }
    }
}
    //public static GameManager Instance;

    //public int objetosRecolectados = 0;
    //public int totalObjetos = 40;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject); // Persiste entre escenas
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void SumarObjeto()
    //{
    //    objetosRecolectados++;

    //    Debug.Log($"Objetos recolectados: {objetosRecolectados}/{totalObjetos}");

    //    if (objetosRecolectados >= totalObjetos)
    //    {
    //        Debug.Log("�Todos los objetos recolectados!");
    //        // Activar puerta final o evento
    //    }
    //}


    //public void CambiarEscena(string nombreEscena)
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
    //}
//}
