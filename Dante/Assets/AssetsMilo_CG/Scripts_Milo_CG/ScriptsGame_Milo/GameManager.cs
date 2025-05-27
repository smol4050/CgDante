using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

/// <summary>
/// Manager global para el progreso del juego y manejo de niveles.
/// Implementa patrón singleton para mantener una instancia única.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única del GameManager.
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// Lista que almacena el progreso de cada nivel.
    /// </summary>
    public List<ProgresoNivel> progresoPorNivel = new List<ProgresoNivel>();

    /// <summary>
    /// Número total de objetos a recolectar por nivel.
    /// </summary>
    public int totalObjetosPorNivel = 10;

    /// <summary>
    /// Evento disparado cada vez que se recolecta un objeto.
    /// </summary>
    public event Action OnObjetoRecolectado;
    public string nombrePartida = "PartidaSinNombre";
    MenuManager menuManager;

    /// <summary>
    /// Método Awake que asegura la instancia única y carga o inicializa el progreso.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            DatosGuardados datos = JsonGuardado.CargarDatos();

            if (datos != null)
            {
                CargarProgresoDesdeJson(datos);
                Debug.Log("Progreso cargado desde JSON.");
            }
            else
            {
                // Aquí pedimos nombre al jugador

                menuManager = FindObjectOfType<MenuManager>();
                menuManager.EncenderPanelNombre();

                InicializarProgreso();
                GuardarProgresoEnJson(); // Guardamos de una vez
                Debug.Log("Progreso inicializado con nueva partida.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Inicializa la lista de progreso con los nombres reales de los niveles.
    /// </summary>
    public void InicializarProgreso()
    {
        progresoPorNivel.Clear(); // importante
        progresoPorNivel.Add(new ProgresoNivel("Waen_CG", nombrePartida));
        progresoPorNivel.Add(new ProgresoNivel("Frame_CG", nombrePartida));
        progresoPorNivel.Add(new ProgresoNivel("Milo_CG", nombrePartida));
        progresoPorNivel.Add(new ProgresoNivel("Smol_CG", nombrePartida));
    }

    /// <summary>
    /// Obtiene el progreso correspondiente al nivel actual cargado en la escena.
    /// </summary>
    /// <returns>El objeto ProgresoNivel correspondiente al nivel actual o null si no se encuentra.</returns>
    public ProgresoNivel GetProgresoNivelActual()
    {
        string nivelActual = SceneManager.GetActiveScene().name;
        return progresoPorNivel.Find(p => p.nombreNivel == nivelActual);
    }

    /// <summary>
    /// Incrementa en uno la cantidad de objetos recolectados en el nivel actual.
    /// Dispara el evento OnObjetoRecolectado y muestra mensajes de debug.
    /// </summary>
    public void SumarObjeto()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso == null) return;

        progreso.objetosRecolectados++;
        Debug.Log($"[{progreso.nombreNivel}] Objetos recolectados: {progreso.objetosRecolectados}/{totalObjetosPorNivel}");

        OnObjetoRecolectado?.Invoke();

        if (progreso.objetosRecolectados >= totalObjetosPorNivel)
        {
            Debug.Log("¡Todos los objetos recolectados!");
            // Aquí puedes llamar algo como: GameController.Instance.PuertaFinalDisponible();
        }
    }

    /// <summary>
    /// Obtiene la cantidad de objetos recolectados en el nivel actual.
    /// </summary>
    /// <returns>Número de objetos recolectados, o 0 si no hay progreso para el nivel actual.</returns>
    public int ObtenerObjetosRecolectados()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        return progreso != null ? progreso.objetosRecolectados : 0;
    }

    /// <summary>
    /// Marca el nivel actual como completado y guarda el progreso en JSON.
    /// </summary>
    public void CompletarNivel()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso != null)
        {
            progreso.nivelCompletado = true;
            Debug.Log($"Nivel completado: {progreso.nombreNivel}");
            GuardarProgresoEnJson(); // Guardamos el progreso justo cuando se completa el nivel

            // Llamar al reporte:
            Debug.Log(GeneradorDeReporte.Generar());
        }
    }

    /// <summary>
    /// Obtiene el nombre del siguiente nivel que no ha sido completado.
    /// </summary>
    /// <returns>Nombre del siguiente nivel no completado, o null si todos están completados.</returns>
    public string ObtenerSiguienteNivelNoCompletado()
    {
        foreach (ProgresoNivel nivel in progresoPorNivel)
        {
            if (!nivel.nivelCompletado)
            {
                string res = nivel.nombreNivel;
                if (nivel.nombreNivel == "Waen_CG")
                {
                    res = "IntroVideo";
                }
                return res;
            }
        }

        Debug.Log("Todos los niveles están completados.");
        return null; // Puedes devolver un nombre especial como "Creditos" si quieres
    }

    /// <summary>
    /// Reinicia la cantidad de objetos recolectados del nivel actual a cero.
    /// </summary>
    public void ReiniciarObjetosDelNivelActual()
    {
        ProgresoNivel progreso = GetProgresoNivelActual();
        if (progreso != null)
            progreso.objetosRecolectados = 0;
    }

    /// <summary>
    /// Cambia la escena actual a la escena indicada por nombre.
    /// </summary>
    /// <param name="nombreEscena">Nombre de la escena a cargar.</param>
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    /// <summary>
    /// Guarda el progreso actual en un archivo JSON.
    /// </summary>
    public void GuardarProgresoEnJson()
    {
        DatosGuardados datos = new DatosGuardados();

        foreach (ProgresoNivel prog in progresoPorNivel)
        {
            datos.progresoPorNivel.Add(new ProgresoNivelGuardado
            {
                nombreNivel = prog.nombreNivel,
                objetosRecolectados = prog.objetosRecolectados,
                nivelCompletado = prog.nivelCompletado,
                nombrePartida = nombrePartida
            });
        }

        JsonGuardado.GuardarDatos(datos);
    }

    /// <summary>
    /// Carga el progreso desde un archivo JSON y actualiza la lista interna.
    /// </summary>
    public void CargarProgresoDesdeJson(DatosGuardados datos)
    {
        progresoPorNivel.Clear();
        nombrePartida = datos.nombrePartida; //IMPORTANTE para mantenerlo sincronizado

        foreach (ProgresoNivelGuardado prog in datos.progresoPorNivel)
        {
            progresoPorNivel.Add(new ProgresoNivel(prog.nombreNivel, prog.nombrePartida)
            {
                objetosRecolectados = prog.objetosRecolectados,
                nivelCompletado = prog.nivelCompletado
            });
        }
    }

    //private string ObtenerNombreDesdeJugador()
    //{
    //    // Aquí deberías usar UI real (InputField). Por ahora hacemos algo automático
    //    string nombre = "Partida_";
    //    Debug.Log("Nombre de la nueva partida: " + nombre);
    //    return nombre;
    //}
}

