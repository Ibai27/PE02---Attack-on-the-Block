using UnityEngine;
using System.Collections; // Para IEnumerator
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject panelGameOver;          // Panel de Game Over
    public MovimientoJugador Pjugador;        // Jugador
    public Cronometro cronometro;             // Cronómetro
    public SpawnerEnemigos Spawner;           // Referencia al spawner de enemigos

    [Header("Sistema de Vidas")]
    public int vidasInicial = 3;              // Vidas al inicio
    public float tiempoInvencibilidad = 1.5f; // Tiempo de invulnerabilidad después de recibir daño

    [HideInInspector]
    public int vidas;                         // Vidas actuales
    private bool jugadorInvencible = false;
    private bool juegoTerminado = false;

    void Start()
    {
        vidas = vidasInicial;

        // Reproducir sonido de inicio
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoInicio);

    }

    public void FinDelJuego()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;

        // Parar cronómetro y guardar récord
        cronometro.PararCronometro();
        cronometro.GuardarRecord();

        // Sonido Game Over
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoGameOver);

        // Mostrar panel Game Over
        if (panelGameOver != null)
            panelGameOver.SetActive(true);

        // Congelar tiempo y mostrar cursor
        Time.timeScale = 0f;
        Pjugador.MostrarCursor();

        // Ocultar jugador
        Pjugador.gameObject.SetActive(false);

        // Detener todos los enemigos instanciados
        var enemigosInstanciados = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (var e in enemigosInstanciados)
        {
            if (e != null)
            {
                var me = e.GetComponent<MovimientoEnemigo>();
                if (me != null) me.enabled = false;
            }
        }
    }

   public void ReiniciarJuego()
    {
        Debug.Log("Reiniciando juego...");

        // Restaurar tiempo
        Time.timeScale = 1f;

        // Reiniciar jugador
        Pjugador.ResetearPosicion();

        // Reiniciar cronómetro
        cronometro.ReiniciarCronometro();

        // Reiniciar vidas
        vidas = vidasInicial;
        jugadorInvencible = false;

        // Ocultar panel Game Over
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        // Destruir todos los enemigos actuales
        var enemigosExistentes = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (var e in enemigosExistentes)
            Destroy(e);

        // Ahora dejamos que el Spawner cree automáticamente el enemigo inicial
        if (Spawner != null)
            Spawner.InstanciarEnemigoInicial();

        // Sonido de inicio
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirSonido(AudioManager.instancia.sonidoInicio);

        juegoTerminado = false;
    }
    private IEnumerator InvencibilidadTemporal()
    {
        jugadorInvencible = true;

        // Feedback visual (parpadeo)
        SpriteRenderer sr = Pjugador.GetComponent<SpriteRenderer>();
        Color original = sr.color;

        float tiempoParpadeo = 0.1f;
        float tiempoAcumulado = 0f;
        while (tiempoAcumulado < tiempoInvencibilidad)
        {
            sr.color = Color.yellow;
            yield return new WaitForSeconds(tiempoParpadeo);
            sr.color = original;
            yield return new WaitForSeconds(tiempoParpadeo);

            tiempoAcumulado += tiempoParpadeo * 2;
        }

        jugadorInvencible = false;
    }

    public void QuitarVida()
    {
        if (!jugadorInvencible)
        {
            vidas--;
            Debug.Log("Vida perdida. Vidas restantes: " + vidas);

            // Activar invencibilidad temporal
            StartCoroutine(InvencibilidadTemporal());

            // Comprobar si se acabaron las vidas
            if (vidas <= 0)
            {
                FinDelJuego();
            }
        }
    }
}
