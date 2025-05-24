using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Controla la reproducción de un video de introducción y carga la escena principal al finalizar.
/// </summary>
public class IntroVideoController : MonoBehaviour
{
    /// <summary>
    /// Componente VideoPlayer que reproduce el video de introducción.
    /// </summary>
    public VideoPlayer videoPlayer;

    /// <summary>
    /// Nombre de la escena principal que se cargará después de que termine el video.
    /// </summary>
    public string escenaPrincipal = "Waen_CG";

    /// <summary>
    /// Se suscribe al evento que detecta cuando el video finaliza.
    /// </summary>
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    /// <summary>
    /// Método llamado cuando el video termina. Carga la escena principal.
    /// </summary>
    /// <param name="vp">El VideoPlayer que ha terminado la reproducción.</param>
    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(escenaPrincipal);
    }
}
