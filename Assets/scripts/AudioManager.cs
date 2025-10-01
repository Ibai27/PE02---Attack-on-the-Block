using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;

    public AudioClip sonidoRebotePared;
    public AudioClip sonidoReboteEnemigo;
    public AudioClip sonidoGolpeJugador;
    public AudioClip sonidoGameOver;
    public AudioClip sonidoInicio;
    public AudioClip sonidoPowerUp;

    private AudioSource audioSource;

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
