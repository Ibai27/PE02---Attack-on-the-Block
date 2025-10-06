using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;

    [Header("Sonidos")]
    public AudioClip sonidoRebotePared;
    public AudioClip sonidoReboteEnemigo;
    public AudioClip sonidoGolpeJugador;
    public AudioClip sonidoGameOver;
    public AudioClip sonidoInicio;
    public AudioClip sonidoPowerUp;

    [Header("Configuración del pool")]
    public int poolSize = 5; // cuántos AudioSources activos simultáneamente, se crean distintos audioSources para no petar el sonido

    private List<AudioSource> sources = new List<AudioSource>();

    void Awake()
    {
        if (instancia == null) instancia = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        // Crear pool de AudioSources
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            sources.Add(src);
        }
    }

    public void ReproducirSonido(AudioClip clip, float volumen = 1f)
    {
        if (clip == null) return;

        // Buscar un AudioSource libre
        foreach (AudioSource src in sources)
        {
            if (!src.isPlaying)
            {
                src.PlayOneShot(clip, volumen);
                return;
            }
        }

        // Si todos están ocupados, crea uno temporal
        AudioSource temp = gameObject.AddComponent<AudioSource>();
        temp.PlayOneShot(clip, volumen);
        Destroy(temp, clip.length);
    }
}
