using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public List<ProgresoNivel> progresoPorNivel = new List<ProgresoNivel>();
    public int totalObjetosPorNivel = 10; // Puedes cambiar esto por nivel si deseas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InicializarProgreso();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarProgreso()
    {
        // Agrega aquí los nombres REALES de tus escenas
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

        if (progreso.objetosRecolectados >= totalObjetosPorNivel)
        {
            Debug.Log("¡Todos los objetos recolectados!");
            // Aquí puedes llamar algo como: GameController.Instance.PuertaFinalDisponible();
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
        }
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
    //        Debug.Log("¡Todos los objetos recolectados!");
    //        // Activar puerta final o evento
    //    }
    //}


    //public void CambiarEscena(string nombreEscena)
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
    //}
//}
