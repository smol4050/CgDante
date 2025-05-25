using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Controla la reproducci�n de un video de introducci�n y carga la escena principal al finalizar.
/// </summary>
public class IntroVideoController : MonoBehaviour
{
    /// <summary>
    /// Componente VideoPlayer que reproduce el video de introducci�n.
    /// </summary>
    public VideoPlayer videoPlayer;

    /// <summary>
    /// Nombre de la escena principal que se cargar� despu�s de que termine el video.
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
    /// M�todo llamado cuando el video termina. Carga la escena principal.
    /// </summary>
    /// <param name="vp">El VideoPlayer que ha terminado la reproducci�n.</param>
    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(escenaPrincipal);
    }
}
